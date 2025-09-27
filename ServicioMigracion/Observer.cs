using System.Collections.Generic;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using ProyectoDistribuidora.Data;
using ProyectoDistribuidora.Models.Legacy;
using ProyectoDistribuidora.ServicioMigracion;

/// <summary>
/// Clase Observer que se encarga de monitorear la tabla de logs en la base de datos Legacy
/// y procesar las operaciones registradas en segundo plano mediante un servicio hospedado,
/// para luego mantener actualizada la base de datos GestionPedidos
/// Hereda de BackgroundService para ejecutarse como un servicio en segundo plano en ASP.NET Core.
/// </summary>
public class Observer : BackgroundService
{
    // Proveedor de servicios para obtener instancias de servicios y dependencias dentro del alcance del observer.
    private readonly IServiceProvider _serviceProvider;

    // Logger para registrar información y errores relacionados con la ejecución del observer.
    private readonly ILogger<Observer> _logger;

    // Fecha y hora de la última ejecución de la limpieza de logs antiguos.
    // Se utiliza para evitar limpiar registros innecesariamente en cada iteración.
    private DateTime _ultimaLimpieza = DateTime.MinValue;

    // Diccionario que mapea combinaciones de tablas y operaciones a los servicios correspondientes y métodos a ejecutar.
    // La clave del diccionario sigue el formato: "NombreTabla-Operación" (por ejemplo: "Usuarios-INSERT").
    // El valor asociado contiene la información del tipo de servicio (ServiceType) y el método específico (MethodName) a ejecutar.
    private readonly Dictionary<string, ServiceMapping> _operationMappings = new Dictionary<string, ServiceMapping>
    {
        // Mapeo para la tabla 'Usuarios':
        // Se ejecutarán diferentes métodos según la operación realizada en la tabla.
        { "Usuarios-INSERT", new ServiceMapping { ServiceType = typeof(LoginServicio), MethodName = "AltaUsuario" } },
        { "Usuarios-UPDATE", new ServiceMapping { ServiceType = typeof(LoginServicio), MethodName = "ModificarUsuario" } },
        { "Usuarios-DELETE", new ServiceMapping { ServiceType = typeof(LoginServicio), MethodName = "BajaUsuario" } },

        // Mapeo para la tabla 'Clientes':
        // Cuando se inserta, actualiza o elimina un cliente, se invocarán los métodos correspondientes en ClienteServicio.
        { "Clientes-INSERT", new ServiceMapping { ServiceType = typeof(ClienteServicio), MethodName = "AltaCliente" } },
        { "Clientes-UPDATE", new ServiceMapping { ServiceType = typeof(ClienteServicio), MethodName = "ModificarCliente" } },
        { "Clientes-DELETE", new ServiceMapping { ServiceType = typeof(ClienteServicio), MethodName = "BajaCliente" } },

        // Mapeo para la tabla 'Stock':
        // Se encarga de operaciones sobre los productos en stock.
        { "Stock-INSERT", new ServiceMapping { ServiceType = typeof(StockServicio), MethodName = "AltaStock" } },
        { "Stock-UPDATE", new ServiceMapping { ServiceType = typeof(StockServicio), MethodName = "ModificarStock" } },
        { "Stock-DELETE", new ServiceMapping { ServiceType = typeof(StockServicio), MethodName = "BajaStock" } },

        // Mapeo para la tabla 'Pedidos':
        // Los registros de pedidos tendrán las operaciones de inserción, actualización y eliminación.
        { "Pedidos-INSERT", new ServiceMapping { ServiceType = typeof(PedidoServicio), MethodName = "AltaPedido" } },
        { "Pedidos-UPDATE", new ServiceMapping { ServiceType = typeof(PedidoServicio), MethodName = "ModificarPedido" } },
        { "Pedidos-DELETE", new ServiceMapping { ServiceType = typeof(PedidoServicio), MethodName = "BajaPedido" } },

        // Mapeo para la tabla 'Linea_Pedidos':
        // La línea de pedidos tiene operaciones específicas distintas a las de 'Pedidos'.
        { "Linea_Pedidos-INSERT", new ServiceMapping { ServiceType = typeof(PedidoServicio), MethodName = "AltaLineaPedidos" } },
        { "Linea_Pedidos-UPDATE", new ServiceMapping { ServiceType = typeof(PedidoServicio), MethodName = "ModificarLineaPedidos" } },
        { "Linea_Pedidos-DELETE", new ServiceMapping { ServiceType = typeof(PedidoServicio), MethodName = "BajaLineaPedidos" } }
    };

    public Observer(IServiceProvider serviceProvider, ILogger<Observer> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    //Se llama automaticamente con el servicio al tener el nombre ExecuteAsync
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Iniciando el LogObserverService.");
        try
        {
            // Mueve los registros con X tiempo en la tabla logs hacia la tabla auditoria para evitar sobrecarga en observer
            // (por el momento esta limpiando registros con más de 2 días)
            await MoverLogsTablaAuditoria();
            _ultimaLimpieza = DateTime.Now;

            while (!stoppingToken.IsCancellationRequested)
            {
                await ProcesarLogsAsync();

                // Revisar si ya pasaron 12 horas desde la última limpieza
                if (DateTime.Now - _ultimaLimpieza >= TimeSpan.FromHours(12))
                {
                    await MoverLogsTablaAuditoria();
                    _ultimaLimpieza = DateTime.Now; // Actualizar la última vez de limpieza
                }
                // Esperar 60 segundos antes de la próxima ejecución
                await Task.Delay(10000, stoppingToken);
            }
        }
        catch (Exception ex)
        {
            // Loguea el error, pero no detiene el servicio permanentemente
            _logger.LogError($"Error en ObserverLlamado: {ex.Message}");
        }
        _logger.LogInformation("LogObserverService detenido.");
    }

    /// <summary>
    /// Procesa los logs que NO están procesados (IsProcessed = false),
    /// y que estén dentro de la última hora (Timestamp >= DateTime.Now.AddHours(-1)).
    /// Procesa en paralelo usando Task.WhenAll.
    /// </summary>
    private async Task ProcesarLogsAsync()
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var legacyDbContext = scope.ServiceProvider.GetRequiredService<LegacyContext>();

            var oneHourAgo = DateTime.Now.AddHours(-1);

            // Traemos en una sola pasada los logs pendientes
            var pendingLogs = await legacyDbContext.Logs
                .Where(log => !log.IsProcessed && log.Timestamp >= oneHourAgo)
                .OrderBy(log => log.Timestamp)
                .ToListAsync();

            if (!pendingLogs.Any())
            {
                _logger.LogInformation("No se encontraron logs pendientes en la última hora.");
                return;
            }

            _logger.LogInformation($"Cantidad de registros pendientes: {pendingLogs.Count}");

            // Creamos una lista de tareas para procesar en paralelo
            var tasks = new List<Task>();

            foreach (var logItem in pendingLogs)
            {
                // Por cada log, lanzamos una tarea que creará su propio contexto y procesará
                tasks.Add(ProcesarUnicoLogAsync(logItem));
            }

            // Esperamos a que todas las tareas finalicen
            await Task.WhenAll(tasks);

            // Cada Task crea su propio dbContext y hace su propio SaveChanges.
        }
    }

    /// <summary>
    /// Procesa un único log de forma independiente.
    /// Crea su propio scope/dbContext para evitar conflictos de concurrencia.
    /// </summary>
    private async Task ProcesarUnicoLogAsync(Log log)
    {
        try
        {
            // Crear un nuevo scope de servicio para obtener un nuevo contexto de base de datos
            using var scope = _serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<LegacyContext>();

            _logger.LogWarning($"Tabla '{log.TableName}' en la operación '{log.Operation}' con id '{log.RecordId}'.");

            // Buscar el log actual en la base de datos utilizando su LogId
            var LogActual = await dbContext.Logs.FindAsync(log.LogId);
            if (LogActual == null)
            {
                _logger.LogError($"No se encontró el log con Id={log.LogId} en la base.");
                return;// Salir si el log no existe en la base de datos
            }

            // Verificar si el log ya ha sido procesado previamente
            if (LogActual.IsProcessed)
            {
                _logger.LogInformation($"El log con Id={log.LogId} ya estaba procesado. Se omite.");
                return;// Evitar reprocesar registros ya procesados
            }

            // Construir la clave para buscar en el diccionario de mapeos (Tabla-Operación)
            var key = $"{LogActual.TableName}-{LogActual.Operation}";
            if (!_operationMappings.TryGetValue(key, out var mapping))
            {
                _logger.LogError($"No hay mapping configurado para la clave '{key}'.");
                return;// Salir si no existe un mapeo para la combinación tabla-operación
            }

            // Obtener la instancia del servicio correspondiente a partir del proveedor de servicios
            var serviceInstance = scope.ServiceProvider.GetService(mapping.ServiceType);
            if (serviceInstance == null)
            {
                _logger.LogError($"No se pudo resolver el servicio {mapping.ServiceType.Name}.");
                return;// Salir si el servicio requerido no está disponible
            }

            // Obtener la referencia al método a ejecutar utilizando reflexión
            MethodInfo methodInfo = mapping.ServiceType.GetMethod(
                mapping.MethodName,
                BindingFlags.Public | BindingFlags.Instance
            );
            if (methodInfo == null)
            {
                _logger.LogError($"No se encontró el método {mapping.MethodName} en {mapping.ServiceType.Name}.");
                return;// Salir si el método no existe en el servicio
            }

            // Construir los parámetros para la invocación del método según la configuración del log
            object[] parameters;
            try
            {
                parameters = BuildParameters(LogActual);
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, $"Error al parsear los parámetros para log Id={log.LogId}.");
                return; // Salir si los parámetros no pudieron ser generados correctamente sin procesar el log
            }

            // Invocar el método de manera asíncrona y esperar su finalización
            var invocationTask = (Task)methodInfo.Invoke(serviceInstance, parameters);
            await invocationTask;

            // Si la ejecución fue exitosa, marcar el log como procesado
            LogActual.IsProcessed = true;
            await dbContext.SaveChangesAsync();// Guardar los cambios en la base de datos
        }
        catch (Exception ex)
        {
            // Capturar y registrar cualquier error que ocurra durante el procesamiento
            _logger.LogError($"Error procesando log Id={log.LogId}: {ex.Message}");
        }
    }

    private object[] BuildParameters(Log log)
    {
        // Ejemplo de if/else
        if (log.TableName == "Usuarios")//---------Usar para tablas con clave compuesta con ID numerico y SecondKey alfanumerica
        {
            // Para "Usuarios": ID numérico + 'SecondKey'
            if (!int.TryParse(log.RecordId, out int numericId))
            {
                throw new ArgumentException($"No se pudo convertir '{log.RecordId}' a int para Usuarios.");
            }
            return new object[] { numericId, log.SecondKey };
        }
        else if (log.TableName == "Linea_Pedidos")//---------Usar para tablas con dos claves numericas
        {
            // Ejemplo: dos claves numéricas
            if (!int.TryParse(log.RecordId, out int firstId))
            {
                throw new ArgumentException($"No se pudo convertir '{log.RecordId}' a int para LineaPedidos (clave 1).");
            }
            if (!int.TryParse(log.SecondKey, out int secondId))
            {
                throw new ArgumentException($"No se pudo convertir '{log.SecondKey}' a int para LineaPedidos (clave 2).");
            }
            return new object[] { firstId, secondId };
        }
        else if (log.TableName == "Stock")  //---------Usar para tablas con ID alfanumérico
        {
            // Para "Stock": se mantiene como string (RecordId alfanumérico)
            return new object[] { log.RecordId };
        }
        else //---------Usar para tablas con ID numerico
        {
            // Para las demás tablas con ID numérico (Clientes, Pedidos, etc.)
            if (!int.TryParse(log.RecordId, out int numericId))
            {
                throw new ArgumentException($"No se pudo convertir '{log.RecordId}' a int para la tabla '{log.TableName}'.");
            }
            return new object[] { numericId };
        }
    }
    private async Task MoverLogsTablaAuditoria()
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var legacyDbContext = scope.ServiceProvider.GetRequiredService<LegacyContext>();

            _logger.LogInformation("Moviendo registros antiguos de la tabla Logs a Auditoria (mayores a 48 horas).");

            var twoDaysAgo = DateTime.Now.AddDays(-2);

            // Buscar logs antiguos
            var oldLogs = await legacyDbContext.Logs
                .Where(log => log.Timestamp < twoDaysAgo)
                .ToListAsync();

            if (!oldLogs.Any())
            {
                _logger.LogInformation("No se encontraron registros antiguos para mover.");
                return;
            }

            // Mapear e insertar en Auditoria
            var auditoriaEntries = oldLogs.Select(l => new Auditoria
            {
                LogId = l.LogId,
                TableName = l.TableName,
                Operation = l.Operation,
                RecordId = l.RecordId,
                SecondKey = l.SecondKey,
                Timestamp = l.Timestamp,
                IsProcessed = l.IsProcessed
                // Agrega más campos si tu tabla Auditoria los requiere
            }).ToList();

            legacyDbContext.Auditoria.AddRange(auditoriaEntries);

            // Eliminar de Logs
            legacyDbContext.Logs.RemoveRange(oldLogs);

            // Guardar cambios en una sola transacción
            await legacyDbContext.SaveChangesAsync();

            _logger.LogInformation(
                $"{auditoriaEntries.Count} registros antiguos fueron movidos a Auditoria y eliminados de Logs."
            );
        }
    }

    /// <summary>
    /// Dejo el metodo comentado en caso de que quieran borrar los logs y no auditarlos
    /// Elimina los logs que tienen más de 2 días de antigüedad (48 horas).
    /// </summary>
    /*private async Task LimpiarLogsViejosAsync()
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var legacyDbContext = scope.ServiceProvider.GetRequiredService<LegacyContext>();

            _logger.LogInformation("Eliminando registros antiguos de la tabla Logs (mayores a 48 horas).");

            var twoDaysAgo = DateTime.Now.AddDays(-2);

            var oldLogs = legacyDbContext.Logs
                .Where(log => log.Timestamp < twoDaysAgo);

            legacyDbContext.Logs.RemoveRange(oldLogs);
            await legacyDbContext.SaveChangesAsync();

            _logger.LogInformation("Registros antiguos de la tabla Logs eliminados.");
        }
    }*/
}

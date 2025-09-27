using ProyectoDistribuidora.Models.Legacy;
using ProyectoDistribuidora.Compartida.Interfaces.IRepositorio;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using ProyectoDistribuidora.Data;
using Microsoft.Data.SqlClient;
using ProyectoDistribuidora.Compartida.Exception; // <-- Asegúrate de tener aquí tu PedidoException
using System;
using System.Data;
using ProyectoDistribuidora.Compartida.Auxiliar;

namespace ProyectoDistribuidora.Compartida.Repositorios
{
    public class LPedidoRepositorio : LIPedidoRepositorio
    {
        private readonly LegacyContext _context;
        private readonly ILogger<LPedidoRepositorio> _logger;

        public LPedidoRepositorio(LegacyContext context, ILogger<LPedidoRepositorio> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IEnumerable<Pedido>> GetAll()
        {
            try
            {
                // Obtiene todos los pedidos de la base de datos
                return await _context.Pedidos.ToListAsync();
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Error de SQL al obtener la lista de pedidos.");
                throw new PedidoException("Error de SQL al obtener la lista de pedidos.", ex);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al obtener la lista de pedidos.");
                throw new PedidoException("Error inesperado al obtener la lista de pedidos.", ex);
            }
        }
        public async Task<IEnumerable<Vendedor>> GetVendedores()
        {
            return await _context.Vendedores.ToListAsync();
        }
        public async Task<Pedido> GetById(long idPedido)
        {
            try
            {
                // Obtiene un pedido por su ID
                return await _context.Pedidos
                                     .FirstOrDefaultAsync(p => p.IdPedido == idPedido);
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, $"Error de SQL al buscar el pedido con IdPedido={idPedido}.");
                throw new PedidoException($"Error de SQL al buscar el pedido con IdPedido={idPedido}.", ex);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, $"Error inesperado al buscar el pedido con IdPedido={idPedido}.");
                throw new PedidoException($"Error inesperado al buscar el pedido con IdPedido={idPedido}.", ex);
            }
        }

        public async Task AddPedido(Pedido pedido)
        {
            try
            {
                _context.Pedidos.Add(pedido);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException dbEx)
            {
                _logger.LogError(dbEx, "Error al agregar el pedido a la base de datos (Legacy).");
                throw new PedidoException("Error al agregar el pedido (Legacy) a la base de datos.", dbEx);
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Error de SQL al agregar el pedido (Legacy).");
                throw new PedidoException("Error de SQL al agregar el pedido (Legacy).", ex);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al agregar el pedido (Legacy).");
                throw new PedidoException("Error inesperado al agregar el pedido (Legacy).", ex);
            }
        }

        public async Task InsertarLineaPedido(int idPedido, LineaPedido nuevaLinea)
        {
            try
            {
                var parameters = new[]
                {
                    new SqlParameter("@IdPedido", idPedido),
                    new SqlParameter("@Codigo", nuevaLinea.Codigo),
                    new SqlParameter("@Cantidad", nuevaLinea.Cantidad),
                    new SqlParameter("@PrecioUnitario", nuevaLinea.PrecioUnitario)
                };

                await _context.Database.ExecuteSqlRawAsync(
                    "EXEC InsertarLineaPedido @IdPedido, @Codigo, @Cantidad, @PrecioUnitario",
                    parameters
                );
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, $"Error SQL al insertar la línea de pedido (IdPedido={idPedido}).");
                throw new PedidoException($"Error SQL al insertar la línea de pedido (IdPedido={idPedido}).", ex);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, $"Error inesperado al insertar la línea de pedido (IdPedido={idPedido}).");
                throw new PedidoException($"Error inesperado al insertar la línea de pedido (IdPedido={idPedido}).", ex);
            }
        }
        public async Task ActualizarLineasPedido(int idPedido, List<LineaPedido> lineasPedido)
        {
            if (lineasPedido == null || !lineasPedido.Any())
            {
                throw new PedidoException("La lista de líneas de pedido no puede ser nula o estar vacía.");
            }

            try
            {
                // Crear una tabla en memoria que coincida con el tipo de tabla SQL
                var dataTable = new DataTable();
                dataTable.Columns.Add("Codigo", typeof(string));
                dataTable.Columns.Add("Cantidad", typeof(int));
                dataTable.Columns.Add("PrecioUnitario", typeof(decimal));

                foreach (var linea in lineasPedido)
                {
                    dataTable.Rows.Add(linea.Codigo, linea.Cantidad, linea.PrecioUnitario);
                }

                // Configurar los parámetros del procedimiento almacenado
                var idPedidoParam = new SqlParameter("@IdPedido", SqlDbType.Int)
                {
                    Value = idPedido
                };

                var lineasPedidoParam = new SqlParameter("@LineasPedido", SqlDbType.Structured)
                {
                    TypeName = "LineaPedidoTipo",
                    Value = dataTable
                };

                // Ejecutar el procedimiento almacenado
                await _context.Database.ExecuteSqlRawAsync(
                    "EXEC ActualizarLineasPedido @IdPedido, @LineasPedido",
                    idPedidoParam,
                    lineasPedidoParam
                );
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, $"Error SQL al actualizar las líneas de pedido para el IdPedido={idPedido}.");
                throw new PedidoException($"Error SQL al actualizar las líneas de pedido para el IdPedido={idPedido}.", ex);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, $"Error inesperado al actualizar las líneas de pedido para el IdPedido={idPedido}.");
                throw new PedidoException($"Error inesperado al actualizar las líneas de pedido para el IdPedido={idPedido}.", ex);
            }
        }

        public async Task Update(Pedido pedido)
        {
            if (pedido == null)
            {
                throw new ArgumentNullException(nameof(pedido), "El pedido no puede ser nulo.");
            }

            try
            {
                _context.Pedidos.Update(pedido);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, $"Error al actualizar el pedido con IdPedido {pedido.IdPedido} (Legacy).");
                // Reemplazamos el throw con una excepción de dominio:
                throw new PedidoException($"Error al actualizar el pedido con IdPedido={pedido.IdPedido} (Legacy).", ex);
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, $"Error SQL al actualizar el pedido con IdPedido={pedido.IdPedido} (Legacy).");
                throw new PedidoException($"Error SQL al actualizar el pedido con IdPedido={pedido.IdPedido} (Legacy).", ex);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, $"Error inesperado al actualizar el pedido con IdPedido={pedido.IdPedido} (Legacy).");
                throw new PedidoException($"Error inesperado al actualizar el pedido con IdPedido={pedido.IdPedido} (Legacy).", ex);
            }
        }

        public async Task<ResultadoCambioEstado> CambiarEstadoPedido(int idPedido, string nuevoEstado)
        {
            if (idPedido <= 0)
            {
                throw new ArgumentException("El ID del pedido debe ser mayor a 0.", nameof(idPedido));
            }

            if (string.IsNullOrWhiteSpace(nuevoEstado))
            {
                throw new ArgumentException("El nuevo estado no puede ser vacío o nulo.", nameof(nuevoEstado));
            }

            try
            {
                // Definir el parámetro de salida correctamente como BIT
                var hayStockInsuficienteParam = new SqlParameter
                {
                    ParameterName = "@HayStockInsuficiente",
                    SqlDbType = SqlDbType.Bit,
                    Direction = ParameterDirection.Output
                };

                var parameters = new[]
                {
            new SqlParameter("@IdPedido", idPedido),
            new SqlParameter("@NuevoEstado", nuevoEstado),
            hayStockInsuficienteParam
        };

                await _context.Database.ExecuteSqlRawAsync(
                    "EXEC CambiarEstadoPedido @IdPedido, @NuevoEstado, @HayStockInsuficiente OUTPUT",
                    parameters
                );

                // Convertir el parámetro de salida correctamente, manejando DBNull
                bool hayStockInsuficiente = hayStockInsuficienteParam.Value != DBNull.Value && Convert.ToBoolean(hayStockInsuficienteParam.Value);

                return new ResultadoCambioEstado
                {
                            Advertencia = hayStockInsuficiente,
                            Mensaje = hayStockInsuficiente
                  ? "El pedido se actualizó, pero hay stock insuficiente para uno o más productos."
                  : "El estado del pedido se actualizó correctamente."
                };
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, $"Error al cambiar el estado del pedido con Id {idPedido} (Legacy).");
                throw new PedidoException($"Error al cambiar el estado del pedido (Id={idPedido}) (Legacy).", ex);
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, $"Error SQL al cambiar el estado del pedido con Id={idPedido} (Legacy).");
                throw new PedidoException($"Error SQL al cambiar el estado del pedido (Id={idPedido}) (Legacy).", ex);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, $"Error inesperado al cambiar el estado del pedido con Id={idPedido} (Legacy).");
                throw new PedidoException($"Error inesperado al cambiar el estado del pedido (Id={idPedido}) (Legacy).", ex);
            }
        }


        public async Task<LineaPedido> GetLineaPedido(int idLineaPedido, int idPedido)
        {
            try
            {
                return await _context.Linea_Pedidos
                    .FirstOrDefaultAsync(lp => lp.IdLineaPedido == idLineaPedido
                                            && lp.IdPedido == idPedido);
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, $"Error SQL al obtener la línea de pedido (IdLineaPedido={idLineaPedido}, IdPedido={idPedido}).");
                throw new PedidoException($"Error SQL al obtener la línea de pedido (IdLineaPedido={idLineaPedido}, IdPedido={idPedido}).", ex);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, $"Error inesperado al obtener la línea de pedido (IdLineaPedido={idLineaPedido}, IdPedido={idPedido}).");
                throw new PedidoException($"Error inesperado al obtener la línea de pedido (IdLineaPedido={idLineaPedido}, IdPedido={idPedido}).", ex);
            }
        }
    }
}

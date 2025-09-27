using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ProyectoDistribuidora.Compartida.DTO.Pedido;
using ProyectoDistribuidora.Compartida.Exception; // Aquí debe existir tu PedidoException
using ProyectoDistribuidora.Compartida.Interfaces.IRepositorio;
using ProyectoDistribuidora.Data;
using ProyectoDistribuidora.Models.GestionPedidos;
using ProyectoDistribuidora.Models.Legacy;

namespace ProyectoDistribuidora.Repositorios.GestionPedidos
{
    public class PedidoRepositorio : IPedidoRepositorio
    {
        private readonly ILogger<PedidoRepositorio> _logger;
        private readonly GestionPedidosContext _context;

        public PedidoRepositorio(GestionPedidosContext context, ILogger<PedidoRepositorio> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task Add(PedidoGP pedido)
        {
            if (pedido == null)
            {
                _logger.LogWarning("El pedido proporcionado es nulo.");
                throw new ArgumentNullException(nameof(pedido), "El pedido no puede ser nulo.");
            }

            try
            {
                Console.WriteLine($"Agregando pedido con NroPedido: {pedido.IdPedido}, ClienteId: {pedido.IdCliente}");

                await _context.Pedidos.AddAsync(pedido);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException dbEx)
            {
                _logger.LogError(dbEx, "Error al agregar el pedido a la base de datos.");
                throw new PedidoException("Error al agregar el pedido a la base de datos.", dbEx);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al agregar el pedido.");
                throw new PedidoException("Error inesperado al agregar el pedido.", ex);
            }
        }

        public async Task Delete(PedidoGP pedido)
        {
            if (pedido == null)
            {
                _logger.LogWarning("El pedido proporcionado es nulo.");
                throw new ArgumentNullException(nameof(pedido), "El pedido no puede ser nulo.");
            }

            try
            {
                Console.WriteLine($"Eliminando pedido con NroPedido: {pedido.IdPedido}, ClienteId: {pedido.IdCliente}");

                _context.Pedidos.Remove(pedido);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException dbEx)
            {
                _logger.LogError(dbEx, "Error al eliminar el pedido de la base de datos.");
                throw new PedidoException("Error al eliminar el pedido de la base de datos.", dbEx);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al eliminar el pedido.");
                throw new PedidoException("Error inesperado al eliminar el pedido.", ex);
            }
        }

        public async Task<IEnumerable<PedidoGP>> GetAll()
        {
            try
            {
                var pedidos = await _context.Pedidos.ToListAsync();

                if (pedidos == null || !pedidos.Any())
                {
                    throw PedidoException.NoEncontrado(0); // Para indicar que no se encontraron pedidos
                }

                return pedidos;
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Error de base de datos al obtener los pedidos.");
                throw new PedidoException("Error de base de datos al obtener los pedidos.", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al obtener todos los pedidos.");
                throw new PedidoException("Error inesperado al obtener los pedidos.", ex);
            }
        }

        public async Task<IEnumerable<LineaPedidosGP>> GetLineasPorPedido(int idPedido)
        {
            try
            {
                return await _context.LineaPedidos
                    .Where(lp => lp.IdPedido == idPedido)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al obtener las líneas para el pedido con Id {idPedido}.");
                throw new PedidoException($"Error al obtener las líneas para el pedido con Id {idPedido}.", ex);
            }
        }

        public async Task<PedidoGP> GetById(long nroPedido)
        {
            if (nroPedido <= 0)
            {
                _logger.LogWarning("El NroPedido proporcionado no es válido.");
                throw new ArgumentException("El número de pedido debe ser mayor que 0.", nameof(nroPedido));
            }

            try
            {
                var pedido = await _context.Pedidos
                                            .FirstOrDefaultAsync(p => p.IdPedido == nroPedido);

                if (pedido == null)
                {
                    _logger.LogInformation($"No se encontró el pedido con NroPedido {nroPedido}.");
                    return null;
                }

                return pedido;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error SQL al buscar el pedido por NroPedido: {ex.Message} Detalles: {ex.StackTrace}");
                throw new PedidoException($"Error al buscar el pedido con NroPedido {nroPedido}.", ex);
            }
        }

        public async Task Update(PedidoGP pedido)
        {
            if (pedido == null)
            {
                _logger.LogWarning("El pedido proporcionado es nulo.");
                throw new ArgumentNullException(nameof(pedido), "El pedido no puede ser nulo.");
            }

            try
            {
                Console.WriteLine($"Actualizando pedido con NroPedido: {pedido.IdPedido}, ClienteId: {pedido.IdCliente}");

                _context.Pedidos.Update(pedido);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException dbEx)
            {
                _logger.LogError(dbEx, "Error al actualizar el pedido en la base de datos.");
                throw new PedidoException("Error al actualizar el pedido en la base de datos.", dbEx);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al actualizar el pedido.");
                throw new PedidoException("Error inesperado al actualizar el pedido.", ex);
            }
        }

        public async Task<IEnumerable<PedidoGP>> GetPedidosPorCliente(long idCliente)
        {
            if (idCliente <= 0)
            {
                _logger.LogWarning("El idCliente proporcionado no es válido.");
                throw new ArgumentException("El idCliente debe ser mayor que 0.", nameof(idCliente));
            }

            try
            {
                var pedidos = await _context.Pedidos
                                            .Where(p => p.IdCliente == idCliente)
                                            .ToListAsync();

                if (!pedidos.Any())
                {
                    _logger.LogInformation($"No se encontraron pedidos para el cliente con IdCliente {idCliente}.");
                }

                return pedidos;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error SQL al buscar pedidos por IdCliente: {ex.Message} Detalles: {ex.StackTrace}");
                throw new PedidoException($"Error al buscar pedidos para el cliente con IdCliente {idCliente}.", ex);
            }
        }

        public async Task<IEnumerable<PedidoGP>> GetPedidosPorVendedor(long idVendedor)
        {
            try
            {
                return await _context.Pedidos
                    .Where(p => p.IdVendedor == idVendedor)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al obtener pedidos asignados al vendedor con IdVendedor {idVendedor}.");
                throw new PedidoException($"Error al obtener pedidos para el vendedor con IdVendedor {idVendedor}.", ex);
            }
        }

        public async Task<IEnumerable<PedidoGP>> GetPedidosPorEstado(string estado)
        {
            try
            {
                return await _context.Pedidos
                    .Where(p => p.Estado == estado)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al obtener los pedidos con el estado '{estado}'.");
                throw new PedidoException($"Error al obtener los pedidos con el estado '{estado}'.", ex);
            }
        }

        public async Task<IEnumerable<PedidoGP>> GetPedidosPorFechaEntrega(DateTime fechaInicio, DateTime fechaFin)
        {
            try
            {
                return await _context.Pedidos
                    .Where(p => p.FechaEntrega >= fechaInicio && p.FechaEntrega <= fechaFin)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al obtener pedidos entre las fechas {fechaInicio:yyyy-MM-dd} y {fechaFin:yyyy-MM-dd}.");
                throw new PedidoException($"Error al obtener pedidos entre las fechas {fechaInicio:yyyy-MM-dd} y {fechaFin:yyyy-MM-dd}.", ex);
            }
        }

        public async Task<LineaPedidosGP> InsertLineaPedido(LineaPedidosGP lineaPedido)
        {
            try
            {
                _context.LineaPedidos.Add(lineaPedido);
                await _context.SaveChangesAsync();
                return lineaPedido;
            }
            catch (DbUpdateException dbEx)
            {
                _logger.LogError(dbEx, "Error al insertar la línea de pedido en la base de datos.");
                throw new PedidoException("Error al insertar la línea de pedido.", dbEx);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al insertar la línea de pedido.");
                throw new PedidoException("Error inesperado al insertar la línea de pedido.", ex);
            }
        }

        public async Task<LineaPedidosGP> UpdateLineaPedido(LineaPedidosGP lineaPedido)
        {
            try
            {
                _context.LineaPedidos.Update(lineaPedido);
                await _context.SaveChangesAsync();
                return lineaPedido;
            }
            catch (DbUpdateException dbEx)
            {
                _logger.LogError(dbEx, "Error al actualizar la línea de pedido en la base de datos.");
                throw new PedidoException("Error al actualizar la línea de pedido.", dbEx);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al actualizar la línea de pedido.");
                throw new PedidoException("Error inesperado al actualizar la línea de pedido.", ex);
            }
        }

        public async Task DeleteLineaPedido(int idLineaPedido, int idPedido)
        {
            try
            {
                var lp = await _context.LineaPedidos
                    .FirstOrDefaultAsync(x => x.IdLineaPedido == idLineaPedido && x.IdPedido == idPedido);

                if (lp != null)
                {
                    _context.LineaPedidos.Remove(lp);
                    await _context.SaveChangesAsync();
                }
                // Si no existe, simplemente no hace nada
            }
            catch (DbUpdateException dbEx)
            {
                _logger.LogError(dbEx, $"Error al eliminar la línea de pedido (IdLineaPedido={idLineaPedido}, IdPedido={idPedido}).");
                throw new PedidoException($"Error al eliminar la línea de pedido (IdLineaPedido={idLineaPedido}, IdPedido={idPedido}).", dbEx);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error inesperado al eliminar la línea de pedido (IdLineaPedido={idLineaPedido}, IdPedido={idPedido}).");
                throw new PedidoException($"Error inesperado al eliminar la línea de pedido (IdLineaPedido={idLineaPedido}, IdPedido={idPedido}).", ex);
            }
        }

        public async Task<LineaPedidosGP> GetLineaPedido(int idLineaPedido, int idPedido)
        {
            try
            {
                return await _context.LineaPedidos
                    .FirstOrDefaultAsync(x => x.IdLineaPedido == idLineaPedido 
                                            && x.IdPedido == idPedido);
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

        public async Task<List<LineaPedidoConProductoDTO>> GetLineasConProducto(int idPedido)
        {
            try
            {
                return await _context.LineaPedidos
                            .Where(lp => lp.IdPedido == idPedido)
                            .Join(_context.Stock,
                                  lp => lp.Codigo,
                                  s => s.Codigo,
                                  (lp, s) => new LineaPedidoConProductoDTO
                                  {
                                      lineaPedido = lp,
                                      DescripcionProducto = s.Descripcion
                                  })
                            .ToListAsync();
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, $"Error SQL al obtener las línea de pedido con IdPedido={idPedido}).");
                throw new PedidoException($"Error SQL al obtener las línea de pedido con IdPedido={idPedido}).", ex);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, $"Error inesperado al obtener la línea de pedido con IdPedido={idPedido}).");
                throw new PedidoException($"Error inesperado al obtener la línea de pedido con IdPedido={idPedido}).", ex);
            }
        }
    }
}

using ProyectoDistribuidora.Compartida.Exception;
using ProyectoDistribuidora.Compartida.Interfaces.IRepositorio;
using ProyectoDistribuidora.Models.GestionPedidos;

namespace ProyectoDistribuidora.CasosDeUso.Pedido
{
    public class CUObtenerPedidosPorFechaEntrega
    {
        private readonly IPedidoRepositorio _pedidoRepositorio;

        public CUObtenerPedidosPorFechaEntrega(IPedidoRepositorio pedidoRepositorio)
        {
            _pedidoRepositorio = pedidoRepositorio ?? throw new ArgumentNullException(nameof(pedidoRepositorio));
        }

        public async Task<IEnumerable<PedidoGP>> Execute(DateTime fechaInicio, DateTime fechaFin)
        {
            // Validar que la fecha de inicio no sea mayor que la fecha de fin
            if (fechaInicio > fechaFin)
            {
                throw new PedidoException("La fecha de inicio no puede ser mayor que la fecha de fin.");
            }

            // Validar que las fechas no sean muy antiguas o futuras (opcional)
            if (fechaInicio < DateTime.UtcNow.AddYears(-10) || fechaFin > DateTime.UtcNow.AddYears(10))
            {
                throw new PedidoException("El rango de fechas proporcionado no es válido.");
            }

            // Obtener los pedidos dentro del rango de fechas especificado
            var pedidos = await _pedidoRepositorio.GetPedidosPorFechaEntrega(fechaInicio, fechaFin);

            if (pedidos == null || !pedidos.Any())
            {
                throw new PedidoException($"No se encontraron pedidos con fecha de entrega entre {fechaInicio:yyyy-MM-dd} y {fechaFin:yyyy-MM-dd}.");
            }

            return pedidos;
        }
    }
}

using ProyectoDistribuidora.Compartida.Exception;
using ProyectoDistribuidora.Compartida.Interfaces.IRepositorio;
using ProyectoDistribuidora.Models.GestionPedidos;

namespace ProyectoDistribuidora.CasosDeUso.Pedido
{
    public class CUObtenerPedidosPorEstado
    {
        private readonly IPedidoRepositorio _pedidoRepositorio;

        public CUObtenerPedidosPorEstado(IPedidoRepositorio pedidoRepositorio)
        {
            _pedidoRepositorio = pedidoRepositorio ?? throw new ArgumentNullException(nameof(pedidoRepositorio));
        }

        public async Task<IEnumerable<PedidoGP>> Execute(string estado)
        {
            // Validar el estado
            if (string.IsNullOrWhiteSpace(estado))
            {
                throw new PedidoException("El estado del pedido no puede estar vacío o solo contener espacios en blanco.");
            }

            // Obtener los pedidos con el estado especificado
            var pedidos = await _pedidoRepositorio.GetPedidosPorEstado(estado);

            if (pedidos == null || !pedidos.Any())
            {
                throw new PedidoException($"No se encontraron pedidos con el estado '{estado}'.");
            }

            return pedidos;
        }
    }
}

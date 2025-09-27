using ProyectoDistribuidora.Compartida.Exception;
using ProyectoDistribuidora.Compartida.Interfaces.IRepositorio;
using ProyectoDistribuidora.Models.GestionPedidos;
using ProyectoDistribuidora.Models.Legacy;

namespace ProyectoDistribuidora.CasoDeUso.Pedido
{
    public class CUObtenerLineasDePedido
    {
        private readonly IPedidoRepositorio _lineaPedidoRepositorio;

        public CUObtenerLineasDePedido(IPedidoRepositorio lineaPedidoRepositorio)
        {
            _lineaPedidoRepositorio = lineaPedidoRepositorio ?? throw new ArgumentNullException(nameof(lineaPedidoRepositorio));
        }

        public async Task<IEnumerable<LineaPedidosGP>> Execute(int idPedido)
        {
            // Validar el ID del pedido
            if (idPedido <= 0)
            {
                throw new PedidoException($"El IdPedido '{idPedido}' no es válido. Debe ser mayor a 0.");
            }

            try
            {
                return await _lineaPedidoRepositorio.GetLineasPorPedido(idPedido);
            }
            catch (Exception ex)
            {
                throw new PedidoException($"Error al obtener las líneas para el pedido con Id {idPedido}.", ex);
            }
        }
    }

}

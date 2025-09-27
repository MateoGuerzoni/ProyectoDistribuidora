using ProyectoDistribuidora.Compartida.Exception;
using ProyectoDistribuidora.Compartida.Interfaces.IRepositorio;
using System.Threading.Tasks;

namespace ProyectoDistribuidora.CasoDeUso.Pedido
{
    public class CUBajaLineaPedido
    {
        private readonly IPedidoRepositorio _pedidoRepositorio;

        public CUBajaLineaPedido(IPedidoRepositorio pedidoRepositorio)
        {
            _pedidoRepositorio = pedidoRepositorio;
        }

        public async Task Execute(int idLineaPedido, int idPedido)
        {
            // Validaciones básicas
            if (idLineaPedido <= 0 || idPedido <= 0)
            {
                throw new PedidoException($"Los identificadores no pueden ser cero o negativos. " +
                                          $"idLineaPedido={idLineaPedido}, idPedido={idPedido}.");
            }

            await _pedidoRepositorio.DeleteLineaPedido(idLineaPedido, idPedido);
        }
    }
}

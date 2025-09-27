using ProyectoDistribuidora.Compartida.Exception;
using ProyectoDistribuidora.Compartida.Interfaces.IRepositorio;

namespace ProyectoDistribuidora.CasoDeUso.Pedido
{
    public class CUBajaPedido
    {
        private readonly IPedidoRepositorio _pedidoRepositorio;

        public CUBajaPedido(IPedidoRepositorio pedidoRepositorio)
        {
            _pedidoRepositorio = pedidoRepositorio;
        }

        public async Task Execute(long idPedido)
        {
            // Validar el Id del pedido
            if (idPedido <= 0)
            {
                throw new PedidoException($"El IdPedido '{idPedido}' no puede ser cero o negativo.");
            }

            // Buscar el pedido existente en la base de datos
            var pedidoExistente = await _pedidoRepositorio.GetById(idPedido);
            if (pedidoExistente == null)
            {
                throw new PedidoException($"El pedido con IdPedido {idPedido} no existe.");
            }

            // Llama al repositorio para eliminar el pedido
            await _pedidoRepositorio.Delete(pedidoExistente);
        }
    }

}

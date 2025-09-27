using ProyectoDistribuidora.Compartida.Exception;
using ProyectoDistribuidora.Compartida.Interfaces.IRepositorio;
using ProyectoDistribuidora.Models.GestionPedidos;

namespace ProyectoDistribuidora.CasoDeUso.Pedido
{
    
    public class CUObtenerLineaDePedidoById
    {
        private readonly IPedidoRepositorio _pedidoRepositorio;

        public CUObtenerLineaDePedidoById(IPedidoRepositorio pedidoRepositorio)
        {
            _pedidoRepositorio = pedidoRepositorio;
        }

        public async Task<LineaPedidosGP> Execute(int idLineaPedido, int idPedido)
        {
            // Validar que los IDs sean mayores a 0
            if (idLineaPedido <= 0 || idPedido <= 0)
            {
                throw new PedidoException($"Los identificadores de la línea de pedido y del pedido deben ser mayores a 0. idLineaPedido={idLineaPedido}, idPedido={idPedido}.");
            }

            // Obtener la línea de pedido desde el repositorio
            var lineaPedido = await _pedidoRepositorio.GetLineaPedido(idLineaPedido, idPedido);

            // Si no existe, lanzar excepción de dominio
            if (lineaPedido == null)
            {
                throw new PedidoException($"No se encontró la línea de pedido con idLineaPedido={idLineaPedido}, idPedido={idPedido}.");
            }

            //Retornar la entidad
            return lineaPedido;
        }
    }
}
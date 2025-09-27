using ProyectoDistribuidora.Compartida.Exception;
using ProyectoDistribuidora.Compartida.Interfaces.IRepositorio;
using ProyectoDistribuidora.Models.Legacy;

namespace ProyectoDistribuidora.CasoDeUso.Pedido
{
    public class LCUObtenerPedidoById
    {
        private readonly LIPedidoRepositorio _pedidoRepositorio;

        // Constructor para inyectar el repositorio de pedidos
        public LCUObtenerPedidoById(LIPedidoRepositorio pedidoRepositorio)
        {
            _pedidoRepositorio = pedidoRepositorio;
        }

        // Método para ejecutar la lógica de obtener un pedido por ID
        public async Task<Models.Legacy.Pedido> Execute(long idPedido)
        {
            // Validar que el ID del pedido sea mayor que 0
            if (idPedido <= 0)
            {
                throw new PedidoException("El ID del pedido debe ser mayor que 0.");
            }

            // Obtener el pedido desde el repositorio
            var pedido = await _pedidoRepositorio.GetById(idPedido);

            // Si no se encuentra el pedido, lanzar una excepción
            if (pedido == null)
            {
                throw new PedidoException($"No se encontró un pedido con el ID {idPedido}.");
            }

            // Retornar el pedido encontrado
            return pedido;
        }
    }
}

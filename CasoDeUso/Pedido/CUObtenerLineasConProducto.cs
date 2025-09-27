using ProyectoDistribuidora.Compartida.DTO.Pedido;
using ProyectoDistribuidora.Compartida.Exception;
using ProyectoDistribuidora.Compartida.Interfaces.IRepositorio;
using ProyectoDistribuidora.Models.GestionPedidos;

namespace ProyectoDistribuidora.CasoDeUso.Pedido
{
    public class CUObtenerLineasConProducto
    {
        private readonly IPedidoRepositorio _lineaPedidoRepositorio;

        public CUObtenerLineasConProducto(IPedidoRepositorio lineaPedidoRepositorio)
        {
            _lineaPedidoRepositorio = lineaPedidoRepositorio ?? throw new ArgumentNullException(nameof(lineaPedidoRepositorio));
        }

        public async Task<List<LineaPedidoConProductoDTO>> Execute(int idPedido)
        {
            // Validar el ID del pedido
            if (idPedido <= 0)
            {
                throw new PedidoException($"El IdPedido '{idPedido}' no es válido. Debe ser mayor a 0.");
            }

            try
            {
                return await _lineaPedidoRepositorio.GetLineasConProducto(idPedido);
            }
            catch (Exception ex)
            {
                throw new PedidoException($"Error al obtener las líneas para el pedido con Id {idPedido}.", ex);
            }
        }
    }
}

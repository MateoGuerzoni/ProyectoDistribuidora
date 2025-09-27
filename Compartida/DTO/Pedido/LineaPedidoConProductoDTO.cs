using ProyectoDistribuidora.Models.GestionPedidos;

namespace ProyectoDistribuidora.Compartida.DTO.Pedido
{
    public class LineaPedidoConProductoDTO
    {
        public LineaPedidosGP lineaPedido { get; set; }
        public string DescripcionProducto { get; set; }
    }
}

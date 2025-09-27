using ProyectoDistribuidora.Models.Legacy;
using System.ComponentModel.DataAnnotations;

namespace ProyectoDistribuidora.Compartida.DTO.Pedido
{
    public class PedidosDTO
    {
        public Models.Legacy.Pedido Pedido { get; set; }
        public List<LineaPedido> LineasPedido { get; set; }
    }
}
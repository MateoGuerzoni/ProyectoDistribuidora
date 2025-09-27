using ProyectoDistribuidora.Compartida.DTO.Pedido;
using ProyectoDistribuidora.Models.GestionPedidos;
using ProyectoDistribuidora.Models.Legacy;

namespace ProyectoDistribuidora.Compartida.Interfaces.IRepositorio
{
    public interface IPedidoRepositorio
    {
        Task Add(PedidoGP pedido);
        Task Delete(PedidoGP pedido);
        Task<IEnumerable<PedidoGP>> GetAll();
        Task<PedidoGP> GetById(long nroPedido);
        Task Update(PedidoGP pedido);
        Task<IEnumerable<LineaPedidosGP>> GetLineasPorPedido(int idPedido);
        Task<IEnumerable<PedidoGP>> GetPedidosPorCliente(long idCliente);
        Task<IEnumerable<PedidoGP>> GetPedidosPorVendedor(long idVendedor);
        Task<IEnumerable<PedidoGP>> GetPedidosPorEstado(string estado);
        Task<IEnumerable<PedidoGP>> GetPedidosPorFechaEntrega(DateTime fechaInicio, DateTime fechaFin);
        // Alta de línea de pedido
        Task<LineaPedidosGP> InsertLineaPedido(LineaPedidosGP lineaPedido);
        // Modificar línea de pedido
        Task<LineaPedidosGP> UpdateLineaPedido(LineaPedidosGP lineaPedido);
        // Eliminar línea de pedido
        Task DeleteLineaPedido(int idLineaPedido, int idPedido);
        Task<LineaPedidosGP> GetLineaPedido(int idLineaPedido, int idPedido);
        Task<List<LineaPedidoConProductoDTO>> GetLineasConProducto(int idPedido);
    }
}

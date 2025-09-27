using ProyectoDistribuidora.Compartida.Auxiliar;
using ProyectoDistribuidora.Models.GestionPedidos;
using ProyectoDistribuidora.Models.Legacy;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProyectoDistribuidora.Compartida.Interfaces.IRepositorio
{
    public interface LIPedidoRepositorio
    {
        Task<IEnumerable<Pedido>> GetAll(); // Obtener todos los pedidos
        Task<Pedido> GetById(long idPedido); // Obtener un pedido por ID
        Task AddPedido(Pedido pedido);
        Task InsertarLineaPedido(int idPedido, LineaPedido nuevaLinea);
        Task<ResultadoCambioEstado> CambiarEstadoPedido(int idPedido, string nuevoEstado);
        Task Update(Pedido pedido);
        Task<LineaPedido> GetLineaPedido(int idLineaPedido, int idPedido);
        Task ActualizarLineasPedido(int idPedido, List<LineaPedido> lineasPedido);
        Task<IEnumerable<Vendedor>> GetVendedores();
    }
}

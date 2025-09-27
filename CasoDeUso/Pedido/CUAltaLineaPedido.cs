using ProyectoDistribuidora.Compartida.DTO;
using ProyectoDistribuidora.Compartida.Exception;
using ProyectoDistribuidora.Compartida.Interfaces.IRepositorio;
using ProyectoDistribuidora.Models.GestionPedidos;
using ProyectoDistribuidora.Models.Legacy;
using System.Threading.Tasks;

namespace ProyectoDistribuidora.CasoDeUso.Pedido
{
    public class CUAltaLineaPedido
    {
        private readonly IPedidoRepositorio _pedidoRepositorio;

        public CUAltaLineaPedido(IPedidoRepositorio pedidoRepositorio)
        {
            _pedidoRepositorio = pedidoRepositorio;
        }

        public async Task Execute(LineaPedidoDTO LineaPedidoDTO, int idPedido)
        {
            // Validación básica
            if (LineaPedidoDTO == null)
            {
                throw new PedidoException("La información de la línea de pedido (lineaPedidoDTO) no puede ser nula.");
            }

            if (idPedido <= 0)
            {
                throw new PedidoException($"El idPedido '{idPedido}' no es válido. Debe ser mayor a 0.");
            }

            // 1) Verificar si ya existe la línea de pedido
            var existe = await _pedidoRepositorio.GetLineaPedido(LineaPedidoDTO.IdLineaPedido, idPedido);
            if (existe != null)
            {
                // Reemplazamos la System.Exception por PedidoException
                throw new PedidoException($"La línea de pedido {LineaPedidoDTO.IdLineaPedido}-{idPedido} ya existe.");
            }

            // 2) Crear la entidad
            var nuevaLinea = new LineaPedidosGP
            {
                IdLineaPedido = LineaPedidoDTO.IdLineaPedido,
                IdPedido = idPedido,
                Codigo = LineaPedidoDTO.Codigo,
                Cantidad = LineaPedidoDTO.Cantidad,
                PrecioUnitario = LineaPedidoDTO.PrecioUnitario,
                Subtotal = LineaPedidoDTO.Subtotal
            };

            // 3) Insertar
            await _pedidoRepositorio.InsertLineaPedido(nuevaLinea);
        }
    }
}
using ProyectoDistribuidora.Compartida.DTO;
using ProyectoDistribuidora.Compartida.DTO.Pedido;
using ProyectoDistribuidora.Compartida.Exception;
using ProyectoDistribuidora.Compartida.Interfaces.IRepositorio;
using ProyectoDistribuidora.Models.GestionPedidos;
using ProyectoDistribuidora.Models.Legacy;

namespace ProyectoDistribuidora.CasoDeUso.Pedido
{
    public class CUModificarLineaPedido
    {
        private readonly IPedidoRepositorio _pedidoRepositorio;

        public CUModificarLineaPedido(IPedidoRepositorio pedidoRepositorio)
        {
            _pedidoRepositorio = pedidoRepositorio;
        }

        public async Task<LineaPedidosGP> Execute(LineaPedidoDTO lineaPedidoDTO)
        {
            // 1) Validar el DTO
            if (lineaPedidoDTO == null)
            {
                throw new PedidoException("La información de la línea de pedido (lineaPedidoDTO) no puede ser nula.");
            }

            // 2) Buscar la línea de pedido existente en la base de datos (GestiónPedidos)
            var lineaExistente = await _pedidoRepositorio.GetLineaPedido(
                lineaPedidoDTO.IdLineaPedido,
                lineaPedidoDTO.IdPedido                
            );

            if (lineaExistente == null)
            {
                throw new PedidoException(
                    $"No existe la línea de pedido con IdPedido={lineaPedidoDTO.IdPedido} e IdLineaPedido={lineaPedidoDTO.IdLineaPedido}."
                );
            }

            // 3) Actualizar las propiedades de la línea
            lineaExistente.Codigo = lineaPedidoDTO.Codigo;
            lineaExistente.Cantidad = lineaPedidoDTO.Cantidad;
            lineaExistente.PrecioUnitario = lineaPedidoDTO.PrecioUnitario;
            lineaExistente.Subtotal = lineaPedidoDTO.Subtotal;

            // 4) Guardar los cambios en la base de datos
            await _pedidoRepositorio.UpdateLineaPedido(lineaExistente);

            // 5) Retorna la entidad modificada
            return lineaExistente;
        }
    }
}

using ProyectoDistribuidora.Compartida.DTO.Pedido;
using ProyectoDistribuidora.Compartida.Exception;
using ProyectoDistribuidora.Compartida.Interfaces.IRepositorio;
using ProyectoDistribuidora.Models.Legacy;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectoDistribuidora.CasosDeUso.Pedido
{
    public class LCUAltaPedido
    {
        private readonly LIPedidoRepositorio _pedidoRepositorio;

        public LCUAltaPedido(LIPedidoRepositorio pedidoRepositorio)
        {
            _pedidoRepositorio = pedidoRepositorio ?? throw new ArgumentNullException(nameof(pedidoRepositorio));
        }

        public async Task Execute(PedidosDTO nuevoPedidoDTO)
        {
            // Validar que el DTO no sea nulo
            if (nuevoPedidoDTO == null)
            {
                throw new PedidoException("El DTO de pedido no puede ser nulo.");
            }

            // Validar que el pedido no sea nulo
            if (nuevoPedidoDTO.Pedido == null)
            {
                throw new PedidoException("El pedido no puede ser nulo dentro del DTO.");
            }

            // Validar que existan líneas de pedido
            if (nuevoPedidoDTO.LineasPedido == null || !nuevoPedidoDTO.LineasPedido.Any())
            {
                throw new PedidoException("El pedido debe tener al menos una línea asociada.");
            }

            // Validaciones adicionales sobre el pedido
            if (nuevoPedidoDTO.Pedido.Total < 0)
            {
                throw new PedidoException("El total del pedido no puede ser negativo.");
            }

            if (string.IsNullOrWhiteSpace(nuevoPedidoDTO.Pedido.Estado))
            {
                throw new PedidoException("El estado del pedido no puede estar vacío.");
            }

            if (nuevoPedidoDTO.Pedido.FechaEntrega < nuevoPedidoDTO.Pedido.FechaCreacion)
            {
                throw new PedidoException("La fecha de entrega no puede ser anterior a la fecha de creación.");
            }

            // Agregar el pedido principal
            await _pedidoRepositorio.AddPedido(nuevoPedidoDTO.Pedido);

            // Recorrer y agregar las líneas de pedido con validaciones
            foreach (var linea in nuevoPedidoDTO.LineasPedido)
            {
                if (linea == null)
                {
                    throw new PedidoException("Una de las líneas de pedido es nula.");
                }

                if (linea.Cantidad <= 0)
                {
                    throw new PedidoException($"La línea de pedido con código {linea.Codigo} debe tener una cantidad mayor a 0.");
                }

                if (linea.PrecioUnitario < 0)
                {
                    throw new PedidoException($"La línea de pedido con código {linea.Codigo} tiene un precio unitario negativo.");
                }

                await _pedidoRepositorio.InsertarLineaPedido(nuevoPedidoDTO.Pedido.IdPedido, linea);
            }
        }
    }
}

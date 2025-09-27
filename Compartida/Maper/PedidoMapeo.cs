using ProyectoDistribuidora.Compartida.DTO.Pedido;
using ProyectoDistribuidora.Compartida.DTO; // Para LineaPedidoDTO
using ProyectoDistribuidora.Models.GestionPedidos;
using ProyectoDistribuidora.Models.Legacy;

namespace ProyectoDistribuidora.Compartida.Maper
{
    public class PedidoMapeo
    {
        // Mapea la clase legacy Pedido a DTO
        public static PedidoDTO MapLegacyToDTO(Pedido pedidoViejo)
        {
            if (pedidoViejo == null)
            {
                throw new ArgumentNullException(nameof(pedidoViejo), "El pedido no puede ser nulo");
            }

            return new PedidoDTO
            {
                IdPedido = pedidoViejo.IdPedido,
                IdVendedor = pedidoViejo.IdVendedor,
                IdCliente = pedidoViejo.IdCliente,
                IdAdministracion = pedidoViejo.IdAdministracion,
                IdSupervisor = pedidoViejo.IdSupervisor,
                FechaCreacion = pedidoViejo.FechaCreacion,
                FechaEntrega = pedidoViejo.FechaEntrega,
                FechaEntregado = pedidoViejo.FechaEntregado,
                Estado = pedidoViejo.Estado,
                Direccion = pedidoViejo.Direccion,
                IdContacto = pedidoViejo.IdContacto,
                MetodoPago = pedidoViejo.MetodoPago,
                Total = pedidoViejo.Total,
                Comentarios = pedidoViejo.Comentarios
            };
        }
        public static Pedido MapDTOToLegacy(PedidoDTO pedidoViejo)
        {
            if (pedidoViejo == null)
            {
                throw new ArgumentNullException(nameof(pedidoViejo), "El pedido no puede ser nulo");
            }

            return new Pedido
            {
                IdPedido = pedidoViejo.IdPedido,
                IdVendedor = pedidoViejo.IdVendedor,
                IdCliente = pedidoViejo.IdCliente,
                IdAdministracion = pedidoViejo.IdAdministracion,
                IdSupervisor = pedidoViejo.IdSupervisor,
                FechaCreacion = pedidoViejo.FechaCreacion,
                FechaEntrega = pedidoViejo.FechaEntrega,
                FechaEntregado = pedidoViejo.FechaEntregado,
                Estado = pedidoViejo.Estado,
                Direccion = pedidoViejo.Direccion,
                IdContacto = pedidoViejo.IdContacto,
                MetodoPago = pedidoViejo.MetodoPago,
                Total = pedidoViejo.Total,
                Comentarios = pedidoViejo.Comentarios
            };
        }

        // Mapea la clase GP Pedido a DTO
        public static PedidoDTO MapGestionPedidosToDTO(PedidoGP pedidoNuevo)
        {
            if (pedidoNuevo == null)
            {
                throw new ArgumentNullException(nameof(pedidoNuevo), "El pedido no puede ser nulo");
            }

            return new PedidoDTO
            {
                IdPedido = pedidoNuevo.IdPedido,
                IdVendedor = pedidoNuevo.IdVendedor,
                IdCliente = pedidoNuevo.IdCliente,
                IdAdministracion = pedidoNuevo.IdAdministracion,
                IdSupervisor = pedidoNuevo.IdSupervisor,
                FechaCreacion = pedidoNuevo.FechaCreacion,
                FechaEntrega = pedidoNuevo.FechaEntrega,
                FechaEntregado = pedidoNuevo.FechaEntregado,
                Estado = pedidoNuevo.Estado,
                Direccion = pedidoNuevo.Direccion,
                IdContacto = pedidoNuevo.IdContacto,
                MetodoPago = pedidoNuevo.MetodoPago,
                Total = pedidoNuevo.Total,
                Comentarios = pedidoNuevo.Comentarios
            };
        }

        // Mapea LineaPedidosGP a LineaPedidoDTO
        public static LineaPedidoDTO MapLineaPedidosToDTO(LineaPedidosGP lineaPedidosGP)
        {
            if (lineaPedidosGP == null)
            {
                throw new ArgumentNullException(nameof(lineaPedidosGP), "La línea de pedido no puede ser nula");
            }

            return new LineaPedidoDTO
            {
                IdPedido = lineaPedidosGP.IdPedido,
                IdLineaPedido = lineaPedidosGP.IdLineaPedido,
                Codigo = lineaPedidosGP.Codigo,
                Cantidad = lineaPedidosGP.Cantidad,
                PrecioUnitario = lineaPedidosGP.PrecioUnitario,
                Subtotal = lineaPedidosGP.Subtotal
            };
        }
        // Mapea LineaPedidosLegacy a LineaPedidoDTO
        public static LineaPedidoDTO MapLineaPedidosLegacyToDTO(LineaPedido lineaPedidosLegacy)
        {
            if (lineaPedidosLegacy == null)
            {
                throw new ArgumentNullException(nameof(lineaPedidosLegacy), "La línea de pedido no puede ser nula");
            }

            return new LineaPedidoDTO
            {
                IdPedido = lineaPedidosLegacy.IdPedido,
                IdLineaPedido = lineaPedidosLegacy.IdLineaPedido,
                Codigo = lineaPedidosLegacy.Codigo,
                Cantidad = lineaPedidosLegacy.Cantidad,
                PrecioUnitario = lineaPedidosLegacy.PrecioUnitario,
                Subtotal = lineaPedidosLegacy.Subtotal
            };
        }
        // Mapea LineaPedidoDTO a LineaPedidosGP
        public static LineaPedidosGP MapLineaPedidoDTOToEntity(LineaPedidoDTO lineaPedidoDTO)
        {
            if (lineaPedidoDTO == null)
            {
                throw new ArgumentNullException(nameof(lineaPedidoDTO), "El DTO de línea de pedido no puede ser nulo");
            }

            return new LineaPedidosGP
            {
                IdPedido = lineaPedidoDTO.IdPedido,
                IdLineaPedido = lineaPedidoDTO.IdLineaPedido,
                Codigo = lineaPedidoDTO.Codigo,
                Cantidad = lineaPedidoDTO.Cantidad,
                PrecioUnitario = lineaPedidoDTO.PrecioUnitario,
                Subtotal = lineaPedidoDTO.Subtotal
            };
        }

    }
}

using ProyectoDistribuidora.Compartida.DTO.Pedido;
using ProyectoDistribuidora.Compartida.Exception;
using ProyectoDistribuidora.Compartida.Interfaces.IRepositorio;
using ProyectoDistribuidora.Models.GestionPedidos;

namespace ProyectoDistribuidora.CasoDeUso.Pedido
{
    public class CUAltaPedido
    {
        private readonly IPedidoRepositorio _pedidoRepositorio;

        // Constructor para inyectar el repositorio de pedidos
        public CUAltaPedido(IPedidoRepositorio pedidoRepositorio)
        {
            _pedidoRepositorio = pedidoRepositorio;
        }

        // Método para ejecutar la lógica de alta del pedido
        public async Task<PedidoGP> Execute(PedidoDTO nuevoPedidoDTO)
        {
            // Validar el DTO
            if (nuevoPedidoDTO == null)
            {
                throw new PedidoException("El objeto PedidoDTO no puede ser nulo.");
            }

            // Ejemplo de validación adicional (ajusta a tus reglas de negocio)
            if (nuevoPedidoDTO.IdPedido <= 0)
            {
                throw new PedidoException($"El IdPedido '{nuevoPedidoDTO.IdPedido}' no es válido.");
            }

            // Mapear el DTO a la entidad PedidoGP
            var nuevoPedido = new PedidoGP
            {
                IdPedido = nuevoPedidoDTO.IdPedido,
                IdVendedor = nuevoPedidoDTO.IdVendedor,
                IdCliente = nuevoPedidoDTO.IdCliente,
                IdAdministracion = nuevoPedidoDTO.IdAdministracion,
                FechaCreacion = nuevoPedidoDTO.FechaCreacion,
                FechaEntregado = nuevoPedidoDTO.FechaEntregado,
                Estado = nuevoPedidoDTO.Estado,
                Direccion = nuevoPedidoDTO.Direccion,
                MetodoPago = nuevoPedidoDTO.MetodoPago,
                Total = nuevoPedidoDTO.Total,
                Comentarios = nuevoPedidoDTO.Comentarios
            };

            // Llamar al repositorio para dar de alta el pedido
            await _pedidoRepositorio.Add(nuevoPedido);

            return nuevoPedido; // Retorna la entidad Pedido creada
        }
    }

}
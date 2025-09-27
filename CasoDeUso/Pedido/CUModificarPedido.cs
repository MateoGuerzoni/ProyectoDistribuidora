using ProyectoDistribuidora.Compartida.DTO.Pedido;
using ProyectoDistribuidora.Compartida.Exception;
using ProyectoDistribuidora.Compartida.Interfaces.IRepositorio;
using ProyectoDistribuidora.Models.GestionPedidos;

namespace ProyectoDistribuidora.CasoDeUso.Pedido
{
    public class CUModificarPedido
    {
        private readonly IPedidoRepositorio _pedidoRepositorio;

        // Constructor con la inyección del repositorio
        public CUModificarPedido(IPedidoRepositorio pedidoRepositorio)
        {
            _pedidoRepositorio = pedidoRepositorio;
        }

        // Método para modificar un pedido
        public async Task<PedidoGP> Execute(PedidoDTO pedidoDTO)
        {
            // Validar el DTO
            if (pedidoDTO == null)
            {
                throw new PedidoException("El pedido no puede ser nulo.");
            }

            // Validar ID del pedido
            if (pedidoDTO.IdPedido <= 0)
            {
                throw new PedidoException($"El IdPedido '{pedidoDTO.IdPedido}' no es válido. Debe ser mayor a 0.");
            }

            // Validar datos críticos del pedido
            if (pedidoDTO.Total < 0)
            {
                throw new PedidoException("El total del pedido no puede ser un valor negativo.");
            }

            if (string.IsNullOrWhiteSpace(pedidoDTO.Estado))
            {
                throw new PedidoException("El estado del pedido no puede estar vacío.");
            }

            if (pedidoDTO.FechaEntrega < pedidoDTO.FechaCreacion)
            {
                throw new PedidoException("La fecha de entrega no puede ser anterior a la fecha de creación.");
            }

            // Buscar el pedido existente en la base de datos
            PedidoGP pedidoExistente = await _pedidoRepositorio.GetById(pedidoDTO.IdPedido);
            if (pedidoExistente == null)
            {
                throw new PedidoException($"El pedido con IdPedido {pedidoDTO.IdPedido} no existe.");
            }

            // Actualizar las propiedades del pedido
            pedidoExistente.IdVendedor = pedidoDTO.IdVendedor;
            pedidoExistente.IdCliente = pedidoDTO.IdCliente;
            pedidoExistente.IdAdministracion = pedidoDTO.IdAdministracion;
            pedidoExistente.IdSupervisor = pedidoDTO.IdSupervisor;
            pedidoExistente.FechaCreacion = pedidoDTO.FechaCreacion;
            pedidoExistente.FechaEntrega = pedidoDTO.FechaEntrega;
            pedidoExistente.FechaEntregado = pedidoDTO.FechaEntregado;
            pedidoExistente.Estado = pedidoDTO.Estado;
            pedidoExistente.Direccion = pedidoDTO.Direccion;
            pedidoExistente.IdContacto = pedidoDTO.IdContacto;
            pedidoExistente.MetodoPago = pedidoDTO.MetodoPago;
            pedidoExistente.Total = pedidoDTO.Total;
            pedidoExistente.Comentarios = pedidoDTO.Comentarios;

            // Guardar los cambios en la base de datos
            await _pedidoRepositorio.Update(pedidoExistente);

            return pedidoExistente; // Retorna la entidad modificada
        }
    }

}

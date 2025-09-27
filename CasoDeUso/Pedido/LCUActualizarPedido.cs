using ProyectoDistribuidora.Compartida.DTO.Pedido;
using ProyectoDistribuidora.Compartida.Exception;
using ProyectoDistribuidora.Compartida.Interfaces.IRepositorio;
using ProyectoDistribuidora.Models.Legacy;
using System;
using System.Threading.Tasks;

namespace ProyectoDistribuidora.CasosDeUso.Pedido
{
    public class LCUActualizarPedido
    {
        private readonly LIPedidoRepositorio _pedidoRepositorio;

        public LCUActualizarPedido(LIPedidoRepositorio pedidoRepositorio)
        {
            _pedidoRepositorio = pedidoRepositorio ?? throw new ArgumentNullException(nameof(pedidoRepositorio));
        }

        public async Task Execute(int idPedido, PedidosDTO pedidoActualizado)
        {
            // Validar entrada de datos
            if (pedidoActualizado == null)
            {
                throw new PedidoException("El pedido actualizado no puede ser nulo.");
            }

            if (idPedido <= 0 || idPedido != pedidoActualizado.Pedido.IdPedido)
            {
                throw new PedidoException("Los datos del pedido no son válidos o el IdPedido no coincide.");
            }

            // Obtener el pedido existente en la base de datos
            var pedidoExistente = await _pedidoRepositorio.GetById(idPedido);
            if (pedidoExistente == null)
            {
                throw new PedidoException($"El pedido con IdPedido {idPedido} no se encontró.");
            }

            // Validaciones adicionales de negocio
            if (pedidoActualizado.Pedido.Total < 0)
            {
                throw new PedidoException("El total del pedido no puede ser negativo.");
            }

            if (string.IsNullOrWhiteSpace(pedidoActualizado.Pedido.Estado))
            {
                throw new PedidoException("El estado del pedido no puede estar vacío.");
            }

            if (pedidoActualizado.Pedido.FechaEntrega < pedidoActualizado.Pedido.FechaCreacion)
            {
                throw new PedidoException("La fecha de entrega no puede ser anterior a la fecha de creación.");
            }



            // Actualizar los campos permitidos
            pedidoExistente.IdVendedor = pedidoActualizado.Pedido.IdVendedor;
            pedidoExistente.IdCliente = pedidoActualizado.Pedido.IdCliente;
            pedidoExistente.IdAdministracion = pedidoActualizado.Pedido.IdAdministracion;
            pedidoExistente.IdSupervisor = pedidoActualizado.Pedido.IdSupervisor;
            pedidoExistente.FechaCreacion = pedidoActualizado.Pedido.FechaCreacion;
            pedidoExistente.FechaEntrega = pedidoActualizado.Pedido.FechaEntrega;
            pedidoExistente.FechaEntregado = pedidoActualizado.Pedido.FechaEntregado;
            pedidoExistente.Estado = pedidoActualizado.Pedido.Estado;
            pedidoExistente.Direccion = pedidoActualizado.Pedido.Direccion;
            pedidoExistente.IdContacto = pedidoActualizado.Pedido.IdContacto;
            pedidoExistente.MetodoPago = pedidoActualizado.Pedido.MetodoPago;
            pedidoExistente.Total = pedidoActualizado.Pedido.Total;
            pedidoExistente.Comentarios = pedidoActualizado.Pedido.Comentarios;
            pedidoExistente.TarjetaID = pedidoActualizado.Pedido.TarjetaID;

            // Guardar cambios en el repositorio
            await _pedidoRepositorio.Update(pedidoExistente);

            await _pedidoRepositorio.ActualizarLineasPedido(idPedido, pedidoActualizado.LineasPedido);

        }
    }
}

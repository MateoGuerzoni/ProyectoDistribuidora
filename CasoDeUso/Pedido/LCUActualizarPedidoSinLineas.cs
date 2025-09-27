using ProyectoDistribuidora.Compartida.DTO.Pedido;
using ProyectoDistribuidora.Compartida.Exception;
using ProyectoDistribuidora.Compartida.Interfaces.IRepositorio;
using ProyectoDistribuidora.Models.Legacy;
using System;
using System.Threading.Tasks;

namespace ProyectoDistribuidora.CasosDeUso.Pedido
{
    public class LCUActualizarPedidoSinLineas
    {
        private readonly LIPedidoRepositorio _pedidoRepositorio;

        public LCUActualizarPedidoSinLineas(LIPedidoRepositorio pedidoRepositorio)
        {
            _pedidoRepositorio = pedidoRepositorio ?? throw new ArgumentNullException(nameof(pedidoRepositorio));
        }

        public async Task Execute(int idPedido, Models.Legacy.Pedido pedidoActualizado)
        {
            // Validar entrada de datos
            if (pedidoActualizado == null)
            {
                throw new PedidoException("El pedido actualizado no puede ser nulo.");
            }

            // Obtener el pedido existente en la base de datos
            var pedidoExistente = await _pedidoRepositorio.GetById(idPedido);
            if (pedidoExistente == null)
            {
                throw new PedidoException($"El pedido con IdPedido {idPedido} no se encontró.");
            }

            // Validaciones adicionales de negocio
            if (pedidoActualizado.Total < 0)
            {
                throw new PedidoException("El total del pedido no puede ser negativo.");
            }
            if (pedidoActualizado.FechaEntrega < pedidoActualizado.FechaCreacion)
            {
                throw new PedidoException("La fecha de entrega no puede ser anterior a la fecha de creación.");
            }

            // Actualizar los campos permitidos
            pedidoExistente.IdVendedor = pedidoActualizado.IdVendedor;
            pedidoExistente.IdCliente = pedidoActualizado.IdCliente;
            pedidoExistente.IdAdministracion = pedidoActualizado.IdAdministracion;
            pedidoExistente.IdSupervisor = pedidoActualizado.IdSupervisor;
            pedidoExistente.FechaCreacion = pedidoActualizado.FechaCreacion;
            pedidoExistente.FechaEntrega = pedidoActualizado.FechaEntrega;
            pedidoExistente.FechaEntregado = pedidoActualizado.FechaEntregado;
            //pedidoExistente.Estado = pedidoActualizado.Estado;
            pedidoExistente.Direccion = pedidoActualizado.Direccion;
            pedidoExistente.IdContacto = pedidoActualizado.IdContacto;
            pedidoExistente.MetodoPago = pedidoActualizado.MetodoPago;
            pedidoExistente.Total = pedidoActualizado.Total;
            pedidoExistente.Comentarios = pedidoActualizado.Comentarios;
            pedidoExistente.TarjetaID = pedidoActualizado.TarjetaID;

            // Guardar cambios en el repositorio
            await _pedidoRepositorio.Update(pedidoExistente);

        }
    }
}

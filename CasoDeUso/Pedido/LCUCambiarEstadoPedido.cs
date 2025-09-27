using ProyectoDistribuidora.Compartida.Auxiliar;
using ProyectoDistribuidora.Compartida.Exception;
using ProyectoDistribuidora.Compartida.Interfaces.IRepositorio;
using System;
using System.Threading.Tasks;

namespace ProyectoDistribuidora.CasosDeUso.Pedido
{
    public class LCUCambiarEstadoPedido
    {
        private readonly LIPedidoRepositorio _pedidoRepositorio;

        public LCUCambiarEstadoPedido(LIPedidoRepositorio pedidoRepositorio)
        {
            _pedidoRepositorio = pedidoRepositorio ?? throw new ArgumentNullException(nameof(pedidoRepositorio));
        }

        public async Task<ResultadoCambioEstado> Execute(int idPedido, string nuevoEstado)
        {
            // Validar el ID del pedido
            if (idPedido <= 0)
            {
                throw new PedidoException("El ID del pedido debe ser mayor que 0.");
            }

            // Validar el estado
            if (string.IsNullOrWhiteSpace(nuevoEstado))
            {
                throw new PedidoException("El nuevo estado del pedido no puede estar vacío o nulo.");
            }

            // Validar estados permitidos
            var estadosValidos = new List<string> { "Pendiente", "Preparando", "Entregado", "Cancelado", "En viaje" };
            if (!estadosValidos.Contains(nuevoEstado, StringComparer.OrdinalIgnoreCase))
            {
                throw new PedidoException($"El estado '{nuevoEstado}' no es válido. Estados permitidos: {string.Join(", ", estadosValidos)}");
            }

            // Actualizar el estado del pedido y recibir mensaje de stock
            ResultadoCambioEstado resultado = await _pedidoRepositorio.CambiarEstadoPedido(idPedido, nuevoEstado);

            // Retornar el resultado (si hay mensaje de stock insuficiente, se devolverá al controlador)
            return resultado;
        }
    }
}

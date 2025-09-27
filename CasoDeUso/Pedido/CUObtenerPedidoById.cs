using ProyectoDistribuidora.Compartida.Exception;
using ProyectoDistribuidora.Compartida.Interfaces.IRepositorio;
using ProyectoDistribuidora.Models.GestionPedidos;
using System;
using System.Threading.Tasks;

namespace ProyectoDistribuidora.CasosDeUso.Pedido
{
    public class CUObtenerPedidoById
    {
        private readonly IPedidoRepositorio _pedidoRepositorio;

        public CUObtenerPedidoById(IPedidoRepositorio pedidoRepositorio)
        {
            _pedidoRepositorio = pedidoRepositorio ?? throw new ArgumentNullException(nameof(pedidoRepositorio));
        }

        public async Task<PedidoGP> Execute(long nroPedido)
        {
            // Validar el número de pedido
            if (nroPedido <= 0)
            {
                throw new PedidoException($"El número de pedido '{nroPedido}' no es válido. Debe ser mayor que 0.");
            }

            // Buscar el pedido en la base de datos
            var pedido = await _pedidoRepositorio.GetById(nroPedido);

            if (pedido == null)
            {
                throw new PedidoException($"No se encontró el pedido con número {nroPedido}.");
            }

            return pedido;
        }
    }
}

using ProyectoDistribuidora.Compartida.Exception;
using ProyectoDistribuidora.Compartida.Interfaces.IRepositorio;
using ProyectoDistribuidora.Models.GestionPedidos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProyectoDistribuidora.CasosDeUso.Pedido
{
    public class CUObtenerPedidosPorCliente
    {
        private readonly IPedidoRepositorio _pedidoRepositorio;

        public CUObtenerPedidosPorCliente(IPedidoRepositorio pedidoRepositorio)
        {
            _pedidoRepositorio = pedidoRepositorio ?? throw new ArgumentNullException(nameof(pedidoRepositorio));
        }

        public async Task<IEnumerable<PedidoGP>> Execute(long idCliente)
        {
            // Validar el ID del cliente
            if (idCliente <= 0)
            {
                throw new PedidoException($"El idCliente '{idCliente}' no es válido. Debe ser mayor que 0.");
            }

            // Obtener los pedidos asociados al cliente
            var pedidos = await _pedidoRepositorio.GetPedidosPorCliente(idCliente);

            if (pedidos == null || !pedidos.Any())
            {
                throw new PedidoException($"No se encontraron pedidos para el cliente con IdCliente {idCliente}.");
            }

            return pedidos;
        }
    }
}

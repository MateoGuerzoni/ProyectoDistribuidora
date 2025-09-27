using ProyectoDistribuidora.Compartida.Exception;
using ProyectoDistribuidora.Compartida.Interfaces.IRepositorio;
using ProyectoDistribuidora.Models.GestionPedidos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProyectoDistribuidora.CasosDeUso.Pedido
{
    public class CUObtenerPedidosPorVendedor
    {
        private readonly IPedidoRepositorio _pedidoRepositorio;

        public CUObtenerPedidosPorVendedor(IPedidoRepositorio pedidoRepositorio)
        {
            _pedidoRepositorio = pedidoRepositorio ?? throw new ArgumentNullException(nameof(pedidoRepositorio));
        }

        public async Task<IEnumerable<PedidoGP>> Execute(long idVendedor)
        {
            // Validar el ID del vendedor
            if (idVendedor <= 0)
            {
                throw new PedidoException($"El ID del vendedor '{idVendedor}' no es válido. Debe ser mayor que 0.");
            }

            // Obtener los pedidos asignados al vendedor
            var pedidos = await _pedidoRepositorio.GetPedidosPorVendedor(idVendedor);

            if (pedidos == null || !pedidos.Any())
            {
                throw new PedidoException($"No se encontraron pedidos asignados al vendedor con ID {idVendedor}.");
            }

            return pedidos;
        }
    }
}

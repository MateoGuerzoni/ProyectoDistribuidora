using ProyectoDistribuidora.CasoDeUso.Pedido;
using ProyectoDistribuidora.CasosDeUso.Pedido;
using ProyectoDistribuidora.Compartida.Auxiliar;
using ProyectoDistribuidora.Compartida.DTO;
using ProyectoDistribuidora.Compartida.DTO.Pedido;
using ProyectoDistribuidora.Compartida.Exception;
using ProyectoDistribuidora.Compartida.Maper;
using ProyectoDistribuidora.Models.GestionPedidos;
using ProyectoDistribuidora.Models.Legacy;
using System.Collections.Generic;

namespace ProyectoDistribuidora.ServicioMigracion
{
    public class PedidoServicio
    {
        private readonly CUAltaPedido _CUAltaPedido;
        private readonly CUObtenerPedidos _CUObtenerPedidos;
        private readonly LCUObtenerPedidoById _LCUObtenerPedidoById;
        private readonly CUModificarPedido _CUModificarPedido;
        private readonly CUBajaPedido _CUBajaPedido;
        private readonly LCUAltaPedido _LCUAltaPedido;
        private readonly LCUInsertarLineaPedido _LCUInsertarLineaPedido;
        private readonly CUObtenerPedidosPorCliente _CUObtenerPedidosPorCliente;
        private readonly CUObtenerLineasDePedido _CUObtenerLineasDePedido;
        private readonly CUObtenerPedidoById _CUObtenerPedidoById;
        private readonly CUObtenerPedidosPorVendedor _CUObtenerPedidosPorVendedor;
        private readonly CUObtenerPedidosPorEstado _CUObtenerPedidosPorEstado;
        private readonly CUObtenerPedidosPorFechaEntrega _CUObtenerPedidosPorFechaEntrega;
        private readonly LCUCambiarEstadoPedido _LCUCambiarEstadoPedido;
        private readonly LCUActualizarPedido _LCUActualizarPedido;
        private readonly CUAltaLineaPedido _CUAltaLineaPedido;
        private readonly CUModificarLineaPedido _CUModificarLineaPedido;
        private readonly CUBajaLineaPedido _CUBajaLineaPedido;
        private readonly LCUObtenerLineaDePedidoById _LCUObtenerLineaDePedidoById;
        private readonly CUObtenerLineaDePedidoById _CUObtenerLineaDePedidoById;
        private readonly LCUActualizarPedidoSinLineas _LCUActualizarPedidoSinLineas;
        private readonly LCUObtenerVendedores _LCUObtenerVendedores;
        private readonly CUObtenerLineasConProducto _CUObtenerLineasConProducto;

        // Constructor para inyectar los casos de uso
        public PedidoServicio(CUObtenerPedidos obtenerPedidos, CUAltaPedido altaPedidoGestionPedidos,
                              LCUObtenerPedidoById obtenerPedidoPorId, CUModificarPedido modificarPedido,
                              CUBajaPedido bajaPedido, CUObtenerLineasDePedido obtenerLineasDePedido, 
                              CUObtenerPedidosPorCliente obtenerPedidosPorCliente, CUObtenerPedidoById _obtenerPedidoPorId,
                              CUObtenerPedidosPorVendedor obtenerPedidosPorVendedor, CUObtenerPedidosPorEstado obtenerPedidosPorEstado,
                              CUObtenerPedidosPorFechaEntrega obtenerPedidosPorFechaEntrega, LCUCambiarEstadoPedido cambiarEstadoPedidoCU,
                              LCUActualizarPedido actualizarPedidoCU, LCUAltaPedido altaPedidoCU, LCUInsertarLineaPedido insertarLineaPedido,
                              CUAltaLineaPedido altaLineaPedido, CUModificarLineaPedido modificarLineaPedido, CUBajaLineaPedido bajaLineaPedido,
                              LCUObtenerLineaDePedidoById obtenerLineaDePedidoByIdLegacy, CUObtenerLineaDePedidoById obtenerLineaDePedidoById,
                              LCUActualizarPedidoSinLineas actualizarPedidoSinLineas, LCUObtenerVendedores lCUObtenerVendedores,
                              CUObtenerLineasConProducto obtenerLineasConProducto)
        {
            _CUObtenerPedidos = obtenerPedidos;
            _CUAltaPedido = altaPedidoGestionPedidos;
            _LCUObtenerPedidoById = obtenerPedidoPorId;
            _LCUAltaPedido = altaPedidoCU;
            _LCUActualizarPedido = actualizarPedidoCU;
            _LCUInsertarLineaPedido = insertarLineaPedido;
            _CUModificarPedido = modificarPedido;
            _CUBajaPedido = bajaPedido;
            _CUObtenerLineasDePedido = obtenerLineasDePedido;
            _CUObtenerPedidosPorCliente = obtenerPedidosPorCliente;
            _CUObtenerPedidoById = _obtenerPedidoPorId;
            _CUObtenerPedidosPorVendedor = obtenerPedidosPorVendedor;
            _CUObtenerPedidosPorEstado = obtenerPedidosPorEstado;
            _CUObtenerPedidosPorFechaEntrega = obtenerPedidosPorFechaEntrega;
            _LCUCambiarEstadoPedido = cambiarEstadoPedidoCU;
            _CUAltaLineaPedido = altaLineaPedido;
            _CUModificarLineaPedido = modificarLineaPedido;
            _CUBajaLineaPedido = bajaLineaPedido;
            _LCUObtenerLineaDePedidoById = obtenerLineaDePedidoByIdLegacy;
            _CUObtenerLineaDePedidoById = obtenerLineaDePedidoById;
            _LCUActualizarPedidoSinLineas = actualizarPedidoSinLineas;
            _LCUObtenerVendedores = lCUObtenerVendedores;
            _CUObtenerLineasConProducto = obtenerLineasConProducto;
        }

        // Método para obtener todos los pedidos de la clase legacy
        public async Task<IEnumerable<PedidoDTO>> ObtenerPedidos()
        {
             var pedidos = await _CUObtenerPedidos.Execute();
             return pedidos.Select(pedido => PedidoMapeo.MapGestionPedidosToDTO(pedido));
        }

         // Método para obtener un pedido por ID de la clase legacy
         public async Task<PedidoDTO> LObtenerPedidoById(long nroPedido)
         {
             var pedido = await _LCUObtenerPedidoById.Execute(nroPedido);
             return PedidoMapeo.MapLegacyToDTO(pedido);
         }
        public async Task<IEnumerable<LineaPedidoDTO>> ObtenerLineasDePedido(int idPedido)
        {
            // Llama al caso de uso para obtener las líneas de pedido
            var lineas = await _CUObtenerLineasDePedido.Execute(idPedido);

            // Mapea las líneas a DTOs para devolverlas
            return lineas.Select(linea => PedidoMapeo.MapLineaPedidosToDTO(linea));
        }
        public async Task<IEnumerable<PedidoGP>> ObtenerPedidosPorVendedor(long idVendedor)
        {
            return await _CUObtenerPedidosPorVendedor.Execute(idVendedor);
        }
        public async Task ActualizarPedido(int idPedido, PedidosDTO pedidoActualizado)
        {
            await _LCUActualizarPedido.Execute(idPedido, pedidoActualizado);
        }
        public async Task<ResultadoCambioEstado> ActualizarPedidoSinLineas(int idPedido, PedidoDTO pedidoActualizado)
        {
            Pedido pedido = PedidoMapeo.MapDTOToLegacy(pedidoActualizado);
            await _LCUActualizarPedidoSinLineas.Execute(idPedido, pedido);
            if (string.IsNullOrWhiteSpace(pedidoActualizado.Estado))
            {
                throw new PedidoException("El estado del pedido no puede estar vacío.");
            }
            ResultadoCambioEstado retornoEstado = await CambiarEstadoPedido(idPedido, pedidoActualizado.Estado);
            return retornoEstado;
        }

        public async Task<IEnumerable<PedidoGP>> ObtenerPedidosPorFechaEntrega(DateTime fechaInicio, DateTime fechaFin)
        {
            return await _CUObtenerPedidosPorFechaEntrega.Execute(fechaInicio, fechaFin);
        }
        // Método para dar de alta un pedido en la clase GestiónPedidos
        public async Task<PedidoDTO> AltaPedido(int nroPedido)
        {
            var pedidoDTO = await LObtenerPedidoById(nroPedido);
            PedidoGP pedidoCreado = await _CUAltaPedido.Execute(pedidoDTO);
            return PedidoMapeo.MapGestionPedidosToDTO(pedidoCreado);
        }
        public async Task LAltaPedido(PedidosDTO nuevoPedido)
        {
            await _LCUAltaPedido.Execute(nuevoPedido);
        }
        public async Task LAgregarLineaPedido(int idPedido, LineaPedido nuevaLinea)
        {
            await _LCUInsertarLineaPedido.Execute(idPedido, nuevaLinea);
        }
        public async Task<IEnumerable<PedidoGP>> ObtenerPedidosPorEstado(string estado)
        {
            return await _CUObtenerPedidosPorEstado.Execute(estado);
        }
        // Método para modificar un pedido en la clase GestiónPedidos
        public async Task<PedidoDTO> ModificarPedido(long nroPedido)
        {
            var pedidoDTO = await LObtenerPedidoById(nroPedido);
            PedidoGP pedidoModificado = await _CUModificarPedido.Execute(pedidoDTO);
            return PedidoMapeo.MapGestionPedidosToDTO(pedidoModificado);
        }
        public async Task<IEnumerable<PedidoGP>> ObtenerPedidosPorCliente(long idCliente)
        {
            return await _CUObtenerPedidosPorCliente.Execute(idCliente);
        }
        public async Task<PedidoGP> ObtenerPedidoById(long nroPedido)
        {
            return await _CUObtenerPedidoById.Execute(nroPedido);
        }
        public async Task<ResultadoCambioEstado> CambiarEstadoPedido(int idPedido, string nuevoEstado)
        {
            // Llamar al método Execute y devolver su resultado
            ResultadoCambioEstado resultado = await _LCUCambiarEstadoPedido.Execute(idPedido, nuevoEstado);

            return resultado; // Devolver el mensaje recibido de Execute
        }
        // Método para dar de baja un pedido en la clase GestiónPedidos
        public async Task BajaPedido(long nroPedido)
        {
            await _CUBajaPedido.Execute(nroPedido);
        }
        public async Task AltaLineaPedidos(int idPedido, int idLineaPedido)
        {
            LineaPedidoDTO lineaDePedidoDTO = await LObtenerLineaPedidoById(idLineaPedido, idPedido);
            await _CUAltaLineaPedido.Execute(lineaDePedidoDTO, idPedido);
        }
        public async Task<LineaPedidoDTO> LObtenerLineaPedidoById(int idLineaPedido, int idPedido)
        {
            LineaPedido lineaPedido = await _LCUObtenerLineaDePedidoById.Execute(idLineaPedido, idPedido);
            return PedidoMapeo.MapLineaPedidosLegacyToDTO(lineaPedido);
        }
        public async Task<LineaPedidoDTO> ObtenerLineaPedidoById(int idLineaPedido, int idPedido)
        {
            LineaPedidosGP lineaPedido = await _CUObtenerLineaDePedidoById.Execute(idLineaPedido, idPedido);
            return PedidoMapeo.MapLineaPedidosToDTO(lineaPedido);
        }
        public async Task ModificarLineaPedidos(int idPedido, int idLineaPedido)
        {
            LineaPedidoDTO lineaDePedidoDTO = await LObtenerLineaPedidoById(idLineaPedido, idPedido);
            await _CUModificarLineaPedido.Execute(lineaDePedidoDTO);
        }
        public async Task BajaLineaPedidos(int idPedido, int idLineaPedido)
        {
            await _CUBajaLineaPedido.Execute(idLineaPedido, idPedido);
        }

        public async Task<IEnumerable<Vendedor>> ObtenerVendedores()
        {
            IEnumerable<Vendedor> vendedores = await _LCUObtenerVendedores.Execute();
            return vendedores;
        }

        public async Task<List<LineaPedidoConProductoDTO>> ObtenerLineasDePedidoConProducto(int idPedido)
        {
            // Llama al caso de uso para obtener las líneas de pedido
            List<LineaPedidoConProductoDTO> lineas = await _CUObtenerLineasConProducto.Execute(idPedido);

            return lineas;
        }
    }
}

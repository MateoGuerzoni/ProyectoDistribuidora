using ProyectoDistribuidora.CasoDeUso.Stock;
using ProyectoDistribuidora.Compartida.DTO.Stock;
using ProyectoDistribuidora.Compartida.Maper;
using ProyectoDistribuidora.Models.GestionPedidos;
using ProyectoDistribuidora.Models.Legacy;

namespace ProyectoDistribuidora.ServicioMigracion
{
    public class StockServicio
    {
        private readonly CUAltaStock _CUAltaStock;
        private readonly LCUObtenerStockById _LCUObtenerStockById;
        private readonly CUModificarStock _CUModificarStock;
        private readonly CUBajaStock _CUBajaStock;
        private readonly CUObtenerStocks _CUObtenerStocks;
        private readonly CUObtenerStockById _CUObtenerStockById;
        private readonly CUObtenerStocksPorDescripcion _CUObtenerStocksPorDescripcion;
        private readonly LCUBajaStock _LCUBajaStock;
        private readonly LCUModificarStock _LCUModificarStock;
        private readonly LCUAltaStock _LCUAltaStock;
        // Constructor para inyectar los casos de uso
        public StockServicio(CUAltaStock altaStockGestionPedidos,
                             LCUObtenerStockById obtenerStockPorId, CUModificarStock modificarStock,
                             CUBajaStock bajaStock, CUObtenerStocks obtenerStocks, LCUBajaStock bajaLegacyStock,
                             CUObtenerStockById obtenerStockById, CUObtenerStocksPorDescripcion obtenerStocksPorDesc, 
                             LCUModificarStock modificarStockLegacy, LCUAltaStock altaStockLegacy)
        {
            _CUAltaStock = altaStockGestionPedidos;
            _LCUObtenerStockById = obtenerStockPorId;
            _CUBajaStock = bajaStock;
            _CUModificarStock = modificarStock;
            _CUObtenerStocks = obtenerStocks;
            _LCUBajaStock = bajaLegacyStock;
            _CUObtenerStockById = obtenerStockById;
            _CUObtenerStocksPorDescripcion = obtenerStocksPorDesc;
            _LCUModificarStock = modificarStockLegacy;
            _LCUAltaStock = altaStockLegacy;
        }

        // Método para obtener un stock por ID de la clase legacy
        public async Task<StockDTO> LObtenerStockById(string nroStock)
        {
            // Llama al caso de uso para obtener un stock por ID
            var stock = await _LCUObtenerStockById.Execute(nroStock);

            // Mapea el stock de la entidad legacy a StockDTO
            return StockMapeo.MapLegacyToDTO(stock);
        }

        // Método para dar de alta un stock en la clase GestiónPedidos
        public async Task<StockDTO> AltaStock(string nroStock)
        {
            // Llama a ObtenerStockLegacyById pasándole el nroStock
            var stockDTO = await LObtenerStockById(nroStock);

            // Llama al caso de uso para dar de alta un nuevo stock
            StockGP stockCreado = await _CUAltaStock.Execute(stockDTO);
            return StockMapeo.MapGestionPedidosToDTO(stockCreado); // Retorna el DTO del stock creado
        }

        // Método para modificar un stock en la clase GestiónPedidos
        public async Task<StockDTO> ModificarStock(string nroStock)
        {
            StockDTO stockDTO = await LObtenerStockById(nroStock);
            StockGP stockModificado = await _CUModificarStock.Execute(stockDTO);
            return StockMapeo.MapGestionPedidosToDTO(stockModificado); // Retorna el DTO del stock modificado
        }
        // Método para obtener todos los stocks de la base GestiónPedidos
        public async Task<IEnumerable<StockDTO>> ObtenerStocks()
        {
            // Llama al caso de uso para obtener los stocks
            var stocks = await _CUObtenerStocks.Execute();

            // Mapea los stocks de la entidad GestionPedidos a StockDTO
            return stocks.Select(stock => StockMapeo.MapGestionPedidosToDTO(stock));
        }

        // Método para dar de baja un stock en la clase GestiónPedidos
        public async Task BajaStock(string nroStock)
        {
            await _CUBajaStock.Execute(nroStock); // No es necesario devolver nada en este caso
        }

        public async Task<StockDTO> ObtenerStockById(string codigo)
        {
            // Llama al caso de uso para obtener el stock
            var stock = await _CUObtenerStockById.Execute(codigo);

            // Si no se encuentra, retorna null
            if (stock == null)
            {
                return null;
            }

            // Mapea el stock de la entidad GestionPedidos a StockDTO
            return StockMapeo.MapGestionPedidosToDTO(stock);
        }

        internal async Task<IEnumerable<StockDTO>> ObtenerStocksPorDescripcion(string desc)
        {
            // Llama al caso de uso para obtener los stocks
            var stocks = await _CUObtenerStocksPorDescripcion.Execute(desc);

            // Mapea los stocks de la entidad GestionPedidos a StockDTO
            return stocks.Select(stock => StockMapeo.MapGestionPedidosToDTO(stock));
        }

        // Método para dar de baja un stock en la base Legacy
        public async Task<bool> LBajaStock(string codigo)
        {
            // Llama al repositorio para eliminar el stock
            return await _LCUBajaStock.Execute(codigo);
        }

        // Método para modificar un stock en la base Legacy
        public async Task<bool> LModificarStock(string codigo, StockDTO stock)
        {
            Stock stockParam = StockMapeo.MapDTOToLegacy(stock);
            // Llama al caso de uso para modificar el stock
            return await _LCUModificarStock.Execute(codigo, stockParam);
        }

        // Método para dar de alta un stock en la base Legacy
        public async Task<StockDTO> LAltaStock(StockDTO stock)
        {
            Stock stockParam = StockMapeo.MapDTOToLegacy(stock);
            // Llama al caso de uso para dar de alta el stock
            var stockCreado = await _LCUAltaStock.Execute(stockParam);

            // Mapea el stock creado a DTO antes de retornarlo
            return StockMapeo.MapLegacyToDTO(stockCreado);
        }


    }

}

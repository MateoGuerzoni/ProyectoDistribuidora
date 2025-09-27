using ProyectoDistribuidora.Compartida.Exception;
using ProyectoDistribuidora.Compartida.Interfaces.IRepositorio;

namespace ProyectoDistribuidora.CasoDeUso.Stock
{
    public class LCUObtenerStockById
    {
        private readonly LIStockRepositorio _stockRepositorio;

        // Constructor para inyectar el repositorio de stock
        public LCUObtenerStockById(LIStockRepositorio stockRepositorio)
        {
            _stockRepositorio = stockRepositorio;
        }

        // Método para ejecutar la lógica de obtener un stock por ID
        public async Task<Models.Legacy.Stock> Execute(string codigoStock)
        {
            if (string.IsNullOrWhiteSpace(codigoStock))
            {
                throw new StockException("El código del stock no puede estar vacío.");
            }

            // Llama al repositorio para obtener el stock desde la base de datos legacy
            var stock = await _stockRepositorio.GetById(codigoStock);
            if (stock == null)
            {
                throw new StockException($"El stock con código {codigoStock} no existe.");
            }

            return stock;
        }
    }

}

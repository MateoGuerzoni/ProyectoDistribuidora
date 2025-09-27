using ProyectoDistribuidora.Compartida.Exception;
using ProyectoDistribuidora.Compartida.Interfaces.IRepositorio;
using ProyectoDistribuidora.Models.Legacy;

namespace ProyectoDistribuidora.CasoDeUso.Stock
{
    public class LCUModificarStock
    {
        private readonly LIStockRepositorio _stockLegacyRepositorio;

        public LCUModificarStock(LIStockRepositorio stockLegacyRepositorio)
        {
            _stockLegacyRepositorio = stockLegacyRepositorio ?? throw new ArgumentNullException(nameof(stockLegacyRepositorio));
        }

        public async Task<bool> Execute(string codigo, Models.Legacy.Stock stock)
        {
            if (string.IsNullOrWhiteSpace(codigo))
            {
                throw new StockException("El código del stock no puede estar vacío.");
            }

            if (codigo != stock.Codigo)
            {
                throw new StockException("El código del stock no coincide con el código proporcionado.");
            }

            // Llama al repositorio para actualizar el stock
            return await _stockLegacyRepositorio.Update(stock);
        }
    }
}

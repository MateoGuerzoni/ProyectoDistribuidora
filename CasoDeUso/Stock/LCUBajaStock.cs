using ProyectoDistribuidora.Compartida.Exception;
using ProyectoDistribuidora.Compartida.Interfaces.IRepositorio;
using ProyectoDistribuidora.Models.Legacy;

namespace ProyectoDistribuidora.CasoDeUso.Stock
{
    public class LCUBajaStock
    {
        private readonly LIStockRepositorio _stockLegacyRepositorio;

        public LCUBajaStock(LIStockRepositorio stockLegacyRepositorio)
        {
            _stockLegacyRepositorio = stockLegacyRepositorio ?? throw new ArgumentNullException(nameof(stockLegacyRepositorio));
        }

        public async Task<bool> Execute(string codigo)
        {
            if (string.IsNullOrWhiteSpace(codigo))
            {
                throw new StockException("El código del stock no puede estar vacío.");
            }

            // Llama al repositorio para eliminar el stock
            return await _stockLegacyRepositorio.Delete(codigo);
        }
    }
}

using ProyectoDistribuidora.Compartida.Exception;
using ProyectoDistribuidora.Compartida.Interfaces.IRepositorio;
using ProyectoDistribuidora.Models.Legacy;

namespace ProyectoDistribuidora.CasoDeUso.Stock
    {
        public class LCUAltaStock
        {
            private readonly LIStockRepositorio _stockLegacyRepositorio;

            public LCUAltaStock(LIStockRepositorio stockLegacyRepositorio)
            {
                _stockLegacyRepositorio = stockLegacyRepositorio ?? throw new ArgumentNullException(nameof(stockLegacyRepositorio));
            }

            public async Task<Models.Legacy.Stock> Execute(Models.Legacy.Stock stock)
            {
            if (stock == null)
            {
                throw new StockException("El stock no puede ser nulo.");
            }

            if (string.IsNullOrWhiteSpace(stock.Codigo))
            {
                throw new StockException("El stock debe tener un código válido.");
            }

            // Inserta el nuevo stock en la base Legacy
            return await _stockLegacyRepositorio.Add(stock);
            }
        }
    }


using ProyectoDistribuidora.Compartida.Exception;
using ProyectoDistribuidora.Compartida.Interfaces.IRepositorio;
using ProyectoDistribuidora.Models.GestionPedidos;

namespace ProyectoDistribuidora.CasoDeUso.Stock
{
    public class CUObtenerStocks
    {
        private readonly IStockRepositorio _stockRepositorio;

        public CUObtenerStocks(IStockRepositorio stockRepositorio)
        {
            _stockRepositorio = stockRepositorio ?? throw new ArgumentNullException(nameof(stockRepositorio));
        }

        public async Task<IEnumerable<StockGP>> Execute()
        {
            // Llama al repositorio para obtener todos los stocks
            var stocks = await _stockRepositorio.GetAll();

            // Validar que la lista no sea null (aunque normalmente EF Core retorna una lista vacía)
            if (stocks == null)
            {
                throw new StockException("No se pudo obtener la lista de stocks.");
            }

            return stocks; // Retorna la lista de stocks
        }
    }
}

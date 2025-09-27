using ProyectoDistribuidora.Compartida.Exception;
using ProyectoDistribuidora.Compartida.Interfaces.IRepositorio;
using ProyectoDistribuidora.Models.GestionPedidos;

namespace ProyectoDistribuidora.CasoDeUso.Stock
{
    public class CUObtenerStockById
    {
        private readonly IStockRepositorio _stockRepositorio;

        public CUObtenerStockById(IStockRepositorio stockRepositorio)
        {
            _stockRepositorio = stockRepositorio ?? throw new ArgumentNullException(nameof(stockRepositorio));
        }

        public async Task<StockGP> Execute(string codigo)
        {
            // Validar el código del stock
            if (string.IsNullOrWhiteSpace(codigo))
            {
                throw new StockException("El código de stock no puede ser nulo o vacío.");
            }

            // Llama al repositorio para obtener el stock por código
            var stock = await _stockRepositorio.GetById(codigo);
            if (stock == null)
            {
                throw new StockException($"El stock con código {codigo} no existe.");
            }

            return stock;
        }
    }
}

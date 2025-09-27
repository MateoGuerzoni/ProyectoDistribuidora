using ProyectoDistribuidora.Compartida.Exception;
using ProyectoDistribuidora.Compartida.Interfaces.IRepositorio;
using ProyectoDistribuidora.Models.GestionPedidos;

namespace ProyectoDistribuidora.CasoDeUso.Stock
{
    public class CUObtenerStocksPorDescripcion
    {
        private readonly IStockRepositorio _stockRepositorio;

        public CUObtenerStocksPorDescripcion(IStockRepositorio stockRepositorio)
        {
            _stockRepositorio = stockRepositorio ?? throw new ArgumentNullException(nameof(stockRepositorio));
        }

        public async Task<IEnumerable<StockGP>> Execute(string descripcion)
        {
            // Validar la descripción
            if (string.IsNullOrWhiteSpace(descripcion))
            {
                throw new StockException("La descripción no puede ser nula o vacía.");
            }

            // Llama al repositorio para obtener los stocks filtrados por descripción
            var stocks = await _stockRepositorio.GetByDescripcion(descripcion);
            if (stocks == null)
            {
                throw new StockException($"No se encontraron stocks con la descripción '{descripcion}'.");
            }

            return stocks;
        }
    }
}

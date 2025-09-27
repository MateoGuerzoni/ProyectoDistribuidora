using ProyectoDistribuidora.Compartida.Exception;
using ProyectoDistribuidora.Compartida.Interfaces.IRepositorio;

namespace ProyectoDistribuidora.CasoDeUso.Stock
{
    public class CUBajaStock
    {
        private readonly IStockRepositorio _stockRepositorio;

        // Constructor para inyectar el repositorio de stock
        public CUBajaStock(IStockRepositorio stockRepositorio)
        {
            _stockRepositorio = stockRepositorio;
        }

        // Método para ejecutar la lógica de baja de un stock
        public async Task Execute(string codigoStock)
        {
            // Validar el código del stock
            if (string.IsNullOrWhiteSpace(codigoStock))
            {
                throw new StockException("El código de stock no puede ser nulo o vacío.");
            }

            // Buscar el stock existente en la base de datos
            var stockExistente = await _stockRepositorio.GetById(codigoStock);
            if (stockExistente == null)
            {
                throw new StockException($"El stock con código {codigoStock} no existe.");
            }

            // Llama al repositorio para eliminar el stock
            await _stockRepositorio.Delete(stockExistente);
        }
    }

}

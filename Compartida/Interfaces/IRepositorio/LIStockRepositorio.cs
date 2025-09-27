using ProyectoDistribuidora.Models.Legacy;

namespace ProyectoDistribuidora.Compartida.Interfaces.IRepositorio
{
    public interface LIStockRepositorio
    {
        Task<bool> Delete(string codigo);
        Task<Stock> GetById(string codigo);
        Task<bool> Update(Stock stock);
        Task<Stock> Add(Stock stock);
    }

}

using ProyectoDistribuidora.Models.GestionPedidos;

namespace ProyectoDistribuidora.Compartida.Interfaces.IRepositorio
{
    public interface IStockRepositorio
    {
        /// <summary>
        /// Agrega un nuevo registro de stock a la base de datos.
        /// </summary>
        /// <param name="stock">El objeto StockGP que se desea agregar.</param>
        /// <returns>Una tarea que representa la operación asincrónica.</returns>
        Task Add(StockGP stock);

        /// <summary>
        /// Elimina un registro de stock de la base de datos.
        /// </summary>
        /// <param name="stock">El objeto StockGP que se desea eliminar.</param>
        /// <returns>Una tarea que representa la operación asincrónica.</returns>
        Task Delete(StockGP stock);

        /// <summary>
        /// Obtiene todos los registros de stock de la base de datos.
        /// </summary>
        /// <returns>Una lista de objetos StockGP.</returns>
        Task<IEnumerable<StockGP>> GetAll();
        Task<IEnumerable<StockGP>> GetByDescripcion(string descripcion);

        /// <summary>
        /// Obtiene un registro de stock de la base de datos por su código.
        /// </summary>
        /// <param name="codigo">El código único del stock a buscar.</param>
        /// <returns>El objeto StockGP correspondiente, o null si no se encuentra.</returns>
        Task<StockGP> GetById(string codigo);

        /// <summary>
        /// Actualiza un registro de stock existente en la base de datos.
        /// </summary>
        /// <param name="stock">El objeto StockGP actualizado.</param>
        /// <returns>Una tarea que representa la operación asincrónica.</returns>
        Task Update(StockGP stock);
    }

}

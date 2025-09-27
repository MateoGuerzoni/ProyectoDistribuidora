using Microsoft.EntityFrameworkCore;
using ProyectoDistribuidora.Compartida.Interfaces.IRepositorio;
using ProyectoDistribuidora.Data;
using ProyectoDistribuidora.Models.Legacy;
using ProyectoDistribuidora.Compartida.Exception;

namespace ProyectoDistribuidora.Repositorios.Legacy
{
    public class LStockRepositorio : LIStockRepositorio
    {
        private readonly LegacyContext _context;

        // Constructor para inyectar el contexto de la base de datos
        public LStockRepositorio(LegacyContext context)
        {
            _context = context;
        }

        // Método para obtener todos los registros de stock
        public async Task<IEnumerable<Stock>> GetAll()
        {
            try
            {
                // Obtiene todos los registros de stock de la base de datos
                var stocks = await _context.Stocks.ToListAsync();

                // Si deseas lanzar excepción cuando no hay registros:
                if (stocks == null || !stocks.Any())
                {
                   throw new StockException("No se encontraron registros de Stock.");
                }

                return stocks;
            }
            catch (DbUpdateException dbEx)
            {
                throw new StockException("Error de base de datos al obtener los registros de Stock.", dbEx);
            }
            catch (System.Exception ex)
            {
                throw new StockException("Error inesperado al obtener los registros de Stock.", ex);
            }
        }

        public async Task<bool> Delete(string codigo)
        {
            try
            {
                // Busca el stock por código
                var stock = await _context.Stocks.FirstOrDefaultAsync(s => s.Codigo == codigo);

                if (stock == null)
                {
                    // Retorna false si no se encontró el stock
                    return false;
                }

                // Elimina el stock del contexto
                _context.Stocks.Remove(stock);
                await _context.SaveChangesAsync();

                return true; // Retorna true si se elimina correctamente
            }
            catch (DbUpdateException dbEx)
            {
                throw new StockException($"Error al eliminar el Stock con código '{codigo}'.", dbEx);
            }
            catch (System.Exception ex)
            {
                throw new StockException($"Error inesperado al eliminar el Stock con código '{codigo}'.", ex);
            }
        }

        public async Task<bool> Update(Stock stock)
        {
            try
            {
                // Verifica si el stock existe
                var stockExistente = await _context.Stocks.FirstOrDefaultAsync(s => s.Codigo == stock.Codigo);
                if (stockExistente == null)
                {
                    // Retorna false si no se encontró el stock
                    return false;
                }

                // Actualiza los valores del stock
                stockExistente.Descripcion = stock.Descripcion;
                stockExistente.Precio = stock.Precio;

                // Guarda los cambios en la base de datos
                await _context.SaveChangesAsync();

                return true; // Retorna true si la actualización fue exitosa
            }
            catch (DbUpdateException dbEx)
            {
                throw new StockException($"Error al actualizar el Stock con código '{stock?.Codigo}'.", dbEx);
            }
            catch (System.Exception ex)
            {
                throw new StockException($"Error inesperado al actualizar el Stock con código '{stock?.Codigo}'.", ex);
            }
        }

        // Método para obtener un registro de stock por su código
        public async Task<Stock> GetById(string codigoStock)
        {
            try
            {
                // Obtiene un registro de stock por su código
                var stock = await _context.Stocks
                    .FirstOrDefaultAsync(s => s.Codigo == codigoStock);

                if (stock == null)
                {
                     throw new StockException($"No se encontró Stock con código '{codigoStock}'.");
                }

                return stock;
            }
            catch (DbUpdateException dbEx)
            {
                throw new StockException($"Error al obtener el Stock con código '{codigoStock}'.", dbEx);
            }
            catch (System.Exception ex)
            {
                throw new StockException($"Error inesperado al obtener el Stock con código '{codigoStock}'.", ex);
            }
        }

        public async Task<Stock> Add(Stock stock)
        {
            try
            {
                // Agrega el nuevo stock al contexto
                await _context.Stocks.AddAsync(stock);

                // Guarda los cambios en la base de datos
                await _context.SaveChangesAsync();

                return stock; // Retorna el stock creado
            }
            catch (DbUpdateException dbEx)
            {
                throw new StockException($"Error al agregar el Stock con código '{stock?.Codigo}'.", dbEx);
            }
            catch (System.Exception ex)
            {
                throw new StockException($"Error inesperado al agregar el Stock con código '{stock?.Codigo}'.", ex);
            }
        }
    }
}

using Microsoft.EntityFrameworkCore;
using ProyectoDistribuidora.Compartida.Interfaces.IRepositorio;
using ProyectoDistribuidora.Data;
using ProyectoDistribuidora.Models.GestionPedidos;
using ProyectoDistribuidora.Compartida.Exception; // Asegúrate de tener aquí tu StockException
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectoDistribuidora.Repositorios.GestionPedidos
{
    public class StockRepositorio : IStockRepositorio
    {
        private readonly ILogger<StockRepositorio> _logger;
        private readonly GestionPedidosContext _context;

        public StockRepositorio(GestionPedidosContext context, ILogger<StockRepositorio> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Agrega un nuevo registro de stock a la base de datos.
        /// </summary>
        public async Task Add(StockGP stock)
        {
            if (stock == null)
            {
                _logger.LogWarning("El stock proporcionado es nulo.");
                throw new StockException("El stock no puede ser nulo.");
            }

            try
            {
                _logger.LogInformation($"Agregando stock con Código: {stock.Codigo}, Descripción: {stock.Descripcion}");

                await _context.Stock.AddAsync(stock);
                await _context.SaveChangesAsync();

                _logger.LogInformation($"Stock con Código {stock.Codigo} agregado exitosamente.");
            }
            catch (DbUpdateException dbEx)
            {
                _logger.LogError(dbEx, $"Error al agregar el stock con Código '{stock.Codigo}' a la base de datos.");
                throw new StockException($"Error al agregar el stock con Código '{stock.Codigo}'.", dbEx);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error inesperado al agregar el stock con Código '{stock.Codigo}'.");
                throw new StockException($"Error inesperado al agregar el stock con Código '{stock.Codigo}'.", ex);
            }
        }

        /// <summary>
        /// Elimina un registro de stock de la base de datos.
        /// </summary>
        public async Task Delete(StockGP stock)
        {
            if (stock == null)
            {
                _logger.LogWarning("El stock proporcionado es nulo.");
                throw new StockException("El stock no puede ser nulo.");
            }

            try
            {
                _logger.LogInformation($"Eliminando stock con Código: {stock.Codigo}, Descripción: {stock.Descripcion}");

                _context.Stock.Remove(stock);
                await _context.SaveChangesAsync();

                _logger.LogInformation($"Stock con Código {stock.Codigo} eliminado exitosamente.");
            }
            catch (DbUpdateException dbEx)
            {
                _logger.LogError(dbEx, $"Error al eliminar el stock con Código '{stock.Codigo}' de la base de datos.");
                throw new StockException($"Error al eliminar el stock con Código '{stock.Codigo}'.", dbEx);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error inesperado al eliminar el stock con Código '{stock.Codigo}'.");
                throw new StockException($"Error inesperado al eliminar el stock con Código '{stock.Codigo}'.", ex);
            }
        }

        /// <summary>
        /// Obtiene todos los registros de stock de la base de datos.
        /// </summary>
        public async Task<IEnumerable<StockGP>> GetAll()
        {
            try
            {
                var stocks = await _context.Stock.ToListAsync();
                _logger.LogInformation($"Se obtuvieron {stocks.Count} registros de stock.");
                return stocks;
            }
            catch (DbUpdateException dbEx)
            {
                _logger.LogError(dbEx, "Error de base de datos al obtener todos los registros de Stock.");
                throw new StockException("Error de base de datos al obtener todos los registros de Stock.", dbEx);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al obtener todos los registros de Stock.");
                throw new StockException("Error inesperado al obtener todos los registros de Stock.", ex);
            }
        }

        /// <summary>
        /// Obtiene los registros de stock que contienen una descripción específica.
        /// </summary>
        public async Task<IEnumerable<StockGP>> GetByDescripcion(string descripcion)
        {
            if (string.IsNullOrWhiteSpace(descripcion))
            {
                _logger.LogWarning("La descripción proporcionada no es válida.");
                throw new StockException("La descripción del stock no puede ser nula o vacía.");
            }

            try
            {
                var stocks = await _context.Stock
                    .Where(s => s.Descripcion.Contains(descripcion))
                    .ToListAsync();

                if (stocks == null || !stocks.Any())
                {
                    _logger.LogInformation($"No se encontraron stocks con descripción que contenga '{descripcion}'.");
                }
                else
                {
                    _logger.LogInformation($"Se encontraron {stocks.Count} stocks con descripción que contenga '{descripcion}'.");
                }

                return stocks;
            }
            catch (DbUpdateException dbEx)
            {
                _logger.LogError(dbEx, $"Error al buscar Stocks por descripción '{descripcion}'.");
                throw new StockException($"Error al buscar Stocks por descripción '{descripcion}'.", dbEx);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error inesperado al buscar Stocks por descripción '{descripcion}'.");
                throw new StockException($"Error inesperado al buscar Stocks por descripción '{descripcion}'.", ex);
            }
        }

        /// <summary>
        /// Obtiene un registro de stock por su código.
        /// </summary>
        public async Task<StockGP> GetById(string codigoStock)
        {
            if (string.IsNullOrWhiteSpace(codigoStock))
            {
                _logger.LogWarning("El código proporcionado no es válido.");
                throw new StockException("El código del stock no puede ser nulo o vacío.");
            }

            try
            {
                var stock = await _context.Stock
                                          .FirstOrDefaultAsync(s => s.Codigo == codigoStock);

                if (stock == null)
                {
                    _logger.LogInformation($"No se encontró el stock con Código {codigoStock}.");
                    return null; // Retorna null si no se encontró el stock
                }

                _logger.LogInformation($"Stock con Código {codigoStock} obtenido exitosamente.");
                return stock;
            }
            catch (DbUpdateException dbEx)
            {
                _logger.LogError(dbEx, $"Error al obtener el Stock con código '{codigoStock}'.");
                throw new StockException($"Error al obtener el Stock con código '{codigoStock}'.", dbEx);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error inesperado al obtener el Stock con código '{codigoStock}'.");
                throw new StockException($"Error inesperado al obtener el Stock con código '{codigoStock}'.", ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de stock existente en la base de datos.
        /// </summary>
        public async Task Update(StockGP stock)
        {
            if (stock == null)
            {
                _logger.LogWarning("El stock proporcionado es nulo.");
                throw new StockException("El stock no puede ser nulo.");
            }

            try
            {
                _logger.LogInformation($"Actualizando stock con Código: {stock.Codigo}, Descripción: {stock.Descripcion}");

                _context.Stock.Update(stock);
                await _context.SaveChangesAsync();

                _logger.LogInformation($"Stock con Código {stock.Codigo} actualizado exitosamente.");
            }
            catch (DbUpdateException dbEx)
            {
                _logger.LogError(dbEx, $"Error al actualizar el Stock con código '{stock.Codigo}'.");
                throw new StockException($"Error al actualizar el Stock con código '{stock.Codigo}'.", dbEx);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error inesperado al actualizar el Stock con código '{stock.Codigo}'.");
                throw new StockException($"Error inesperado al actualizar el Stock con código '{stock.Codigo}'.", ex);
            }
        }
    }
}

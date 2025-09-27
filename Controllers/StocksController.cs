using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoDistribuidora.Compartida.DTO.Stock;
using ProyectoDistribuidora.Compartida.Exception;
using ProyectoDistribuidora.Data;
using ProyectoDistribuidora.Models.Legacy;
using ProyectoDistribuidora.ServicioMigracion;

namespace ProyectoDistribuidora.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class StocksController : ControllerBase
    {
        private readonly StockServicio _stockServicio;
        public StocksController(StockServicio stockServicio)
        {
            _stockServicio = stockServicio ?? throw new ArgumentNullException(nameof(stockServicio));
        }

        // GET: api/Stocks
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<StockDTO>>> GetStocks()
        {
            try
            {
                // Llama al servicio para obtener los stocks
                var stocks = await _stockServicio.ObtenerStocks();
                return Ok(stocks); // Retorna un 200 OK con la lista de stocks
            }
            catch (StockException ex)
            {
                // Maneja errores específicos del dominio
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                // Maneja cualquier error inesperado
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Error interno del servidor.", details = ex.Message });
            }
        }

        //----------------------devolver DTO
        // GET: api/Stocks/5
        [HttpGet("{codigo}")]
        [Authorize]
        public async Task<ActionResult<StockDTO>> GetStock(string codigo)
        {
            if (string.IsNullOrWhiteSpace(codigo))
            {
                return BadRequest(new { message = "El código del stock no puede estar vacío." });
            }

            try
            {
                // Llama al servicio para obtener el stock por código
                StockDTO stock = await _stockServicio.ObtenerStockById(codigo);

                if (stock == null)
                {
                    return NotFound(new { message = $"No se encontró el stock con el código {codigo}." });
                }

                return Ok(stock); // Retorna un 200 OK con el StockDTO
            }
            catch (StockException ex)
            {
                // Maneja errores específicos del dominio
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                // Maneja cualquier error inesperado
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Error interno del servidor.", details = ex.Message });
            }
        }

        //----------------------devolver DTO
        [HttpGet("buscar/{buscar}")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<StockDTO>>> GetStockXDesc(string buscar)
        {
            if (string.IsNullOrWhiteSpace(buscar))
            {
                return BadRequest(new { message = "El término de búsqueda no puede estar vacío." });
            }

            try
            {
                // Llama al servicio para obtener los stocks filtrados por descripción
                var stocks = await _stockServicio.ObtenerStocksPorDescripcion(buscar);

                if (stocks == null || !stocks.Any())
                {
                    return NotFound(new { message = $"No se encontraron stocks con la descripción que contiene '{buscar}'." });
                }

                return Ok(stocks); // Retorna un 200 OK con la lista de StockDTO
            }
            catch (StockException ex)
            {
                // Maneja errores específicos del dominio
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                // Maneja cualquier error inesperado
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Error interno del servidor.", details = ex.Message });
            }
        }
        // PUT: api/Stocks/5
        // EndPoint que no utilizamos por el momento, si se aplica habria que cambiar para que reciba y mande DTO
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutStock(string id, StockDTO stock)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return BadRequest(new { message = "El ID del stock no puede estar vacío." });
            }

            if (stock == null)
            {
                return BadRequest(new { message = "El objeto StockDTO no puede ser nulo." });
            }

            if (id != stock.Codigo)
            {
                return BadRequest(new { message = "El ID del stock no coincide con el código del objeto enviado." });
            }

            try
            {
                // Llama al servicio para actualizar el stock en la base Legacy
                var actualizado = await _stockServicio.LModificarStock(id, stock);

                if (!actualizado)
                {
                    return NotFound(new { message = $"No se encontró el stock con el código {id}." });
                }

                return Ok(); // Retorna un 200 si la actualización fue exitosa
            }
            catch (StockException ex)
            {
                // Maneja errores específicos del dominio
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                // Maneja cualquier error inesperado
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Error interno del servidor.", details = ex.Message });
            }
        }

        // POST: api/Stocks
        // EndPoint que no utilizamos por el momento, si se aplica habria que cambiar para que reciba y mande DTO
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<StockDTO>> PostStock(StockDTO stock)
        {
            if (stock == null)
            {
                return BadRequest(new { message = "El objeto StockDTO no puede ser nulo." });
            }

            try
            {
                // Llama al servicio para dar de alta el stock en la base Legacy
                var nuevoStock = await _stockServicio.LAltaStock(stock);

                if (nuevoStock == null)
                {
                    return Conflict(new { message = $"Ya existe un stock con el código {stock.Codigo}." });
                }

                // Retorna un 200 OK en lugar de un 201 Created
                return Ok(nuevoStock);
            }
            catch (StockException ex)
            {
                // Maneja errores específicos del dominio
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                // Maneja cualquier error inesperado
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Error interno del servidor.", details = ex.Message });
            }
        }


        // DELETE: api/Stocks/5
        // EndPoint que no utilizamos por el momento, si se aplica habria que cambiar para que reciba y mande DTO
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteStock(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return BadRequest(new { message = "El ID del stock no puede estar vacío." });
            }

            try
            {
                // Llama al servicio para eliminar el stock en la base Legacy
                var eliminado = await _stockServicio.LBajaStock(id);

                if (!eliminado)
                {
                    return NotFound(new { message = $"No se encontró el stock con el código {id}." });
                }

                return Ok(new { message = "Stock eliminado exitosamente." });
            }
            catch (StockException ex)
            {
                // Maneja errores específicos del dominio
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                // Maneja cualquier error inesperado
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Error interno del servidor.", details = ex.Message });
            }
        }

        private async Task<bool> StockExists(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return false;
            }

            try
            {
                // Llama al servicio para intentar obtener el stock
                var stock = await _stockServicio.ObtenerStockById(id);
                return stock != null;
            }
            catch (StockException)
            {
                // En caso de excepción específica, asume que el stock no existe o hay un error
                return false;
            }
            catch
            {
                // En caso de cualquier otra excepción, también asume que el stock no existe
                return false;
            }
        }
}
}
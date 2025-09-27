using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoDistribuidora.CasoDeUso.Cliente;
using ProyectoDistribuidora.Compartida.DTO.Cliente;
using ProyectoDistribuidora.Data;
using ProyectoDistribuidora.Models.GestionPedidos;
using ProyectoDistribuidora.Models.Legacy;
using ProyectoDistribuidora.ServicioMigracion;
using ProyectoDistribuidora.Compartida.Exception; // <-- Para usar ClienteException
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;

namespace ProyectoDistribuidora.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ClientesController : ControllerBase
    {
        private readonly ClienteServicio _clienteServicio;
        private readonly ILogger<ClientesController> _logger;

        public ClientesController(ClienteServicio clienteServicio, ILogger<ClientesController> logger)
        {
            _clienteServicio = clienteServicio ?? throw new ArgumentNullException(nameof(clienteServicio));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        // ============================================================
        // GET: api/v1/Clientes
        // ============================================================
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<ClienteDTO>>> GetClientes()
        {
            try
            {
                var clientes = await _clienteServicio.ObtenerClientes();
                return Ok(clientes);
            }
            catch (ClienteException ex)
            {
                _logger.LogError(ex, "Error de dominio (ClienteException) al obtener la lista de clientes.");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al obtener los clientes.");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        // ============================================================
        // GET: api/v1/Clientes/5
        // ============================================================
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<ClienteDTO>> GetCliente(long id)
        {
            try
            {
                var cliente = await _clienteServicio.ObtenerClienteById(id);
                if (cliente == null)
                {
                    return NotFound();
                }

                return Ok(cliente);
            }
            catch (ClienteException ex)
            {
                _logger.LogError(ex, $"Error de dominio (ClienteException) al obtener el cliente con id={id}.");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error inesperado al obtener el cliente con id={id}.");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        // ============================================================
        // PUT: api/v1/Clientes/{id}
        // ============================================================
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutCliente(long id, [FromBody] ClienteDTO cliente)
        {
            if (id != cliente.NroCliente)
            {
                return BadRequest("El ID de la URL no coincide con el del objeto.");
            }

            try
            {
                // Llamamos al servicio
                var clienteActualizado = await _clienteServicio.LModificarCliente(cliente);
                return Ok();
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogError(ex, $"No se encontró el cliente con id={id} al actualizar.");
                return NotFound(ex.Message);
            }
            catch (ClienteException ex)
            {
                _logger.LogError(ex, $"Error de dominio (ClienteException) al actualizar el cliente con id={id}.");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error inesperado al actualizar el cliente con id={id}.");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        // ============================================================
        // POST: api/v1/Clientes
        // ============================================================
        // EndPoint que no utilizamos por el momento, 
        // si se aplica habría que cambiar para que reciba y mande DTO
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Cliente>> PostCliente(Cliente cliente)
        {
            try
            {
                if (await ClienteExists(cliente.Nro_Cliente)) // Verificar existencia en el controlador
                {
                    return Conflict($"El cliente con NroCliente {cliente.Nro_Cliente} ya existe.");
                }

                // Llamamos al servicio que crea el cliente en la base Legacy
                Cliente nuevoCliente = await _clienteServicio.LAltaCliente(cliente);

                // Retorna un 201 Created con la ruta para obtener el cliente
                return Ok(new
                {
                    mensaje = "Cliente creado correctamente",
                    cliente = nuevoCliente
                });
            }
            catch (ClienteException ex)
            {
                _logger.LogError(ex, "Error de dominio (ClienteException) al crear un nuevo cliente.");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al crear un nuevo cliente.");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        // ============================================================
        // DELETE: api/v1/Clientes/{id}
        // ============================================================
        // EndPoint que no utilizamos por el momento, 
        // si se aplica habría que cambiar para que reciba y mande DTO
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteCliente(long id)
        {
            try
            {
                if (await ClienteExists(id))
                {
                    // Llamamos al servicio que elimina el cliente en la base Legacy
                    await _clienteServicio.LBajaCliente(id);
                    return Ok();
                }
                else
                {
                    // Si el cliente no existe, devolvemos un 404
                    return NotFound($"El cliente con ID {id} no existe.");
                }
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogError(ex, $"No se encontró el cliente con id={id} al eliminar.");
                return NotFound(ex.Message);
            }
            catch (ClienteException ex)
            {
                _logger.LogError(ex, $"Error de dominio (ClienteException) al eliminar el cliente con id={id}.");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error inesperado al eliminar el cliente con id={id}.");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        // ============================================================
        // Método privado para verificar existencia
        // ============================================================
        private async Task<bool> ClienteExists(long id)
        {
            // Llamamos al servicio que verifica si el cliente existe en la base Legacy
            return await _clienteServicio.ExisteCliente(id);
        }
    }
}

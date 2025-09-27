using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ProyectoDistribuidora.Compartida.Auxiliar;
using ProyectoDistribuidora.Compartida.DTO.Pedido;
using ProyectoDistribuidora.Compartida.Exception;
using ProyectoDistribuidora.Data;
using ProyectoDistribuidora.Models.GestionPedidos;
using ProyectoDistribuidora.Models.Legacy;
using ProyectoDistribuidora.Repositorios.GestionPedidos;
using ProyectoDistribuidora.ServicioMigracion;
using System.Collections.Generic;

namespace ProyectoDistribuidora.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class PedidosController : ControllerBase
    {
        private readonly ILogger<PedidosController> _logger;
        private readonly PedidoServicio _pedidoServicio;
        public PedidosController(PedidoServicio pedidoServicio, ILogger<PedidosController> logger)
        {
            _pedidoServicio = pedidoServicio ?? throw new ArgumentNullException(nameof(pedidoServicio));
            _logger = logger;
        }

        // ============================================================
        // GET: api/Pedidos
        // ============================================================
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<PedidoDTO>>> GetPedidos()
        {
            try
            {
                IEnumerable<PedidoDTO> pedidosDTO = await _pedidoServicio.ObtenerPedidos();
                return Ok(pedidosDTO);
            }
            catch (PedidoException ex)
            {
                _logger.LogError(ex, "Error de dominio (PedidoException) al obtener los pedidos.");
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al obtener los pedidos.");
                return StatusCode(500, "Error interno del servidor al obtener los pedidos.");
            }
        }

        // ============================================================
        // POST: api/Pedidos
        // ============================================================
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Pedido>> PostPedido(PedidosDTO nuevoPedidoDTO)
        {
            try
            {
                // Llama al servicio para realizar alta de pedido en la base de datos Legacy
                await _pedidoServicio.LAltaPedido(nuevoPedidoDTO);
                return Ok("Pedido creado exitosamente.");
            }
            catch (ArgumentException ex)
            {
                // Validaciones de parámetros
                _logger.LogError(ex, "Error de argumento al crear el pedido.");
                return BadRequest(ex.Message);
            }
            catch (PedidoException ex)
            {
                // Error de dominio
                _logger.LogError(ex, "Error de dominio (PedidoException) al crear un pedido.");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                // Error genérico
                _logger.LogError(ex, "Error inesperado al crear el pedido.");
                return StatusCode(500, $"Error al crear el pedido: {ex.Message}");
            }
        }

        // ============================================================
        // POST: api/Pedidos/{idPedido}/lineas
        // ============================================================
        [HttpPost("{idPedido}/lineas")]
        [Authorize]
        public async Task<IActionResult> InsertarLineaPedido(int idPedido, LineaPedido nuevaLinea)
        {
            try
            {
                // Llama al servicio para agregar linea de pedido al pedido
                await _pedidoServicio.LAgregarLineaPedido(idPedido, nuevaLinea);
                return Ok("Línea de pedido insertada exitosamente.");
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, $"ArgumentException al insertar línea de pedido. IdPedido={idPedido}");
                return BadRequest(ex.Message);
            }
            catch (PedidoException ex)
            {
                _logger.LogError(ex, $"Error de dominio (PedidoException) al insertar la línea de pedido. IdPedido={idPedido}");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error inesperado al insertar la línea de pedido. IdPedido={idPedido}");
                return StatusCode(500, $"Error al insertar la línea de pedido: {ex.Message}");
            }
        }

        // ============================================================
        // GET: api/Pedidos/{idPedido}/lineas
        // ============================================================
        [HttpGet("{idPedido}/lineas")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<LineaPedido>>> GetLineasDePedido(int idPedido)
        {
            try
            {
                // Llamar al servicio para obtener las líneas de pedido
                var lineasPedidoDTO = await _pedidoServicio.ObtenerLineasDePedido(idPedido);

                if (lineasPedidoDTO == null || !lineasPedidoDTO.Any())
                {
                    return NotFound($"No se encontraron líneas para el pedido con Id {idPedido}.");
                }

                return Ok(lineasPedidoDTO);
            }
            catch (PedidoException ex)
            {
                _logger.LogError(ex, $"Error de dominio (PedidoException) al obtener las líneas del pedido Id={idPedido}.");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error inesperado al obtener las líneas del pedido Id={idPedido}.");
                return StatusCode(500, $"Error al obtener las líneas del pedido: {ex.Message}");
            }
        }
        // ============================================================
        // GET: api/Pedidos/{idPedido}/lineas
        // ============================================================
        [HttpGet("{idPedido}/lineas/DescProducto")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<LineaPedidoConProductoDTO>>> GetLineasDePedidoConProducto(int idPedido)
        {
            try
            {
                // Llamar al servicio para obtener las líneas de pedido
                List<LineaPedidoConProductoDTO> LineaPedido = await _pedidoServicio.ObtenerLineasDePedidoConProducto(idPedido);

                if (LineaPedido == null || !LineaPedido.Any())
                {
                    return NotFound($"No se encontraron líneas para el pedido con Id {idPedido}.");
                }

                return Ok(LineaPedido);
            }
            catch (PedidoException ex)
            {
                _logger.LogError(ex, $"Error de dominio (PedidoException) al obtener las líneas del pedido Id={idPedido}.");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error inesperado al obtener las líneas del pedido Id={idPedido}.");
                return StatusCode(500, $"Error al obtener las líneas del pedido: {ex.Message}");
            }
        }
        // ============================================================
        // GET: api/Pedidos/Cliente/{idCliente}
        // ============================================================
        [HttpGet("Cliente/{idCliente}")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<PedidoGP>>> GetPedidosPorCliente(long idCliente)
        {
            try
            {
                var pedidos = await _pedidoServicio.ObtenerPedidosPorCliente(idCliente);
                if (pedidos == null || !pedidos.Any())
                {
                    return NotFound($"No se encontraron pedidos para el cliente con IdCliente {idCliente}.");
                }

                return Ok(pedidos);
            }
            catch (PedidoException ex)
            {
                _logger.LogError(ex, $"Error de dominio (PedidoException) al obtener pedidos por cliente {idCliente}.");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error inesperado al obtener pedidos para el cliente {idCliente}.");
                return StatusCode(500, $"Error interno al obtener los pedidos del cliente {idCliente}.");
            }
        }

        // ============================================================
        // GET: api/Pedidos/Pedido/{idPedido}
        // ============================================================
        [HttpGet("Pedido/{idPedido}")]
        [Authorize]
        public async Task<ActionResult<PedidoGP>> GetPedidosById(long idPedido)
        {
            try
            {
                var pedido = await _pedidoServicio.ObtenerPedidoById(idPedido);
                if (pedido == null)
                {
                    return NotFound($"No se encontraron pedidos para el pedido con idPedido {idPedido}.");
                }

                return Ok(pedido);
            }
            catch (PedidoException ex)
            {
                _logger.LogError(ex, $"Error de dominio (PedidoException) al obtener el pedido con idPedido={idPedido}.");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error inesperado al obtener el pedido con idPedido={idPedido}.");
                return StatusCode(500, $"Error interno al obtener el pedido con idPedido {idPedido}.");
            }
        }

        // ============================================================
        // GET: api/Pedidos/Vendedor/{idVendedor}
        // ============================================================
        [HttpGet("Vendedor/{idVendedor}")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<PedidoGP>>> GetPedidosPorVendedor(long idVendedor)
        {
            try
            {
                var pedidos = await _pedidoServicio.ObtenerPedidosPorVendedor(idVendedor);
                if (pedidos == null || !pedidos.Any())
                {
                    return NotFound($"No se encontraron pedidos asignados al vendedor con IdVendedor {idVendedor}.");
                }

                return Ok(pedidos);
            }
            catch (PedidoException ex)
            {
                _logger.LogError(ex, $"Error de dominio (PedidoException) al obtener pedidos para el vendedor con IdVendedor={idVendedor}.");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error inesperado al obtener pedidos del vendedor con IdVendedor={idVendedor}.");
                return StatusCode(500, $"Error interno al obtener pedidos del vendedor {idVendedor}.");
            }
        }

        // ============================================================
        // GET: api/Pedidos/Vendedor/{idVendedor}
        // ============================================================
        [HttpGet("Vendedores")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Vendedor>>> GetVendedores()
        {
            try
            {
                IEnumerable<Vendedor> vendedores = await _pedidoServicio.ObtenerVendedores();
                return Ok(vendedores);
            }
            catch (VendedorException ex)
            {
                _logger.LogError(ex, "Error de dominio (VendedorException) al obtener los vendedores.");
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al obtener los vendedores.");
                return StatusCode(500, "Error interno del servidor al obtener los vendedores.");
            }
        }

        // ============================================================
        // GET: api/Pedidos/Estado/{estado}
        // ============================================================
        [HttpGet("Estado/{estado}")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<PedidoGP>>> GetPedidosPorEstado(string estado)
        {
            try
            {
                var pedidos = await _pedidoServicio.ObtenerPedidosPorEstado(estado);
                if (pedidos == null || !pedidos.Any())
                {
                    return NotFound($"No se encontraron pedidos con estado: {estado}.");
                }

                return Ok(pedidos);
            }
            catch (PedidoException ex)
            {
                _logger.LogError(ex, $"Error de dominio (PedidoException) al obtener pedidos con estado='{estado}'.");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error inesperado al obtener pedidos con estado='{estado}'.");
                return StatusCode(500, $"Error interno al obtener los pedidos con estado '{estado}'.");
            }
        }

        // ============================================================
        // GET: api/Pedidos/FechaEntrega
        // ============================================================
        [HttpGet("FechaEntrega")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<PedidoGP>>> GetPedidosPorFechaEntrega(
          [FromQuery] DateTime fechaInicio,
          [FromQuery] DateTime fechaFin)
        {
            if (fechaInicio > fechaFin)
            {
                return BadRequest("La fecha de inicio no puede ser mayor que la fecha de fin.");
            }

            try
            {
                var pedidos = await _pedidoServicio.ObtenerPedidosPorFechaEntrega(fechaInicio, fechaFin);
                if (pedidos == null || !pedidos.Any())
                {
                    return NotFound($"No se encontraron pedidos con FechaEntrega entre {fechaInicio:yyyy-MM-dd} y {fechaFin:yyyy-MM-dd}.");
                }

                return Ok(pedidos);
            }
            catch (PedidoException ex)
            {
                _logger.LogError(ex, $"Error de dominio (PedidoException) al obtener pedidos por fecha de entrega. Rango={fechaInicio} - {fechaFin}");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error inesperado al obtener pedidos por fecha de entrega. Rango={fechaInicio} - {fechaFin}");
                return StatusCode(500, $"Error interno al obtener pedidos por fecha de entrega: {ex.Message}");
            }
        }

        // ============================================================
        // PUT: api/Pedidos/Estado/{idPedido}
        // ============================================================
        [HttpPut("Estado/{idPedido}")]
        [Authorize]
        public async Task<IActionResult> CambiarEstadoPedido(int idPedido, [FromBody] string nuevoEstado)
        {
            try
            {
                ResultadoCambioEstado resultado = await _pedidoServicio.CambiarEstadoPedido(idPedido, nuevoEstado);

                if (resultado.Advertencia)
                {
                    // Devolver 202 Accepted con advertencia
                    return Accepted(new
                    {
                        mensaje = resultado.Mensaje,
                        advertencia = true
                    });
                }

                // Devolver 200 OK en caso de éxito sin advertencias
                return Ok(new
                {
                    mensaje = resultado.Mensaje
                });
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, $"ArgumentException al cambiar el estado del pedido Id={idPedido}.");
                return BadRequest(ex.Message);
            }
            catch (PedidoException ex)
            {
                _logger.LogError(ex, $"Error de dominio (PedidoException) al cambiar el estado del pedido Id={idPedido}.");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error inesperado al cambiar el estado del pedido Id={idPedido}.");
                return StatusCode(500, $"Error al cambiar el estado del pedido: {ex.Message}");
            }
        }

        // ============================================================
        // PUT: api/Pedidos/{idPedido}
        // ============================================================
        [HttpPut("{idPedido}")]
        [Authorize]
        public async Task<IActionResult> ActualizarPedido(int idPedido, [FromBody] PedidosDTO pedidoActualizado)
        {
            if (pedidoActualizado.Pedido == null || pedidoActualizado.LineasPedido == null || idPedido != pedidoActualizado.Pedido.IdPedido)
            {
                return BadRequest("Los datos del pedido no son válidos o el IdPedido no coincide.");
            }

            try
            {
                await _pedidoServicio.ActualizarPedido(idPedido, pedidoActualizado);
                return Ok($"El pedido con IdPedido {idPedido} se ha actualizado correctamente.");
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogError(ex, $"No se encontró el pedido con IdPedido={idPedido} al actualizar.");
                return NotFound(ex.Message);
            }
            catch (PedidoException ex)
            {
                _logger.LogError(ex, $"Error de dominio (PedidoException) al actualizar el pedido Id={idPedido}.");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error inesperado al actualizar el pedido Id={idPedido}.");
                return StatusCode(500, $"Error al actualizar el pedido: {ex.Message}");
            }
        }

        // ============================================================
        // PUT: api/Pedidos/{idPedido}
        // ============================================================
        [HttpPut("sin-lineas/{idPedido}")]
        [Authorize]
        public async Task<ActionResult<string>> ActualizarPedidoSinLineas(int idPedido, [FromBody] PedidoDTO pedidoActualizado)
        {
            if (pedidoActualizado == null || idPedido != pedidoActualizado.IdPedido)
            {
                return BadRequest("Los datos del pedido no son válidos o el IdPedido no coincide.");
            }
            try
            {
                // Llamar al servicio para actualizar el pedido
                ResultadoCambioEstado resultado = await _pedidoServicio.ActualizarPedidoSinLineas(idPedido, pedidoActualizado);

                if (resultado.Advertencia)
                {
                    // Si hay advertencia de stock insuficiente, devolver 202 Accepted con advertencia
                    return Accepted(new
                    {
                        mensaje = resultado.Mensaje,
                        advertencia = true
                    });
                }

                // Si no hay advertencias, devolver 200 OK con el mensaje correspondiente
                return Ok(new
                {
                    mensaje = resultado.Mensaje
                });
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogError(ex, $"No se encontró el pedido con IdPedido={idPedido} al actualizar.");
                return NotFound(ex.Message);
            }
            catch (PedidoException ex)
            {
                _logger.LogError(ex, $"Error de dominio (PedidoException) al actualizar el pedido Id={idPedido}.");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error inesperado al actualizar el pedido Id={idPedido}.");
                return StatusCode(500, $"Error al actualizar el pedido: {ex.Message}");
            }
        }
    }
}
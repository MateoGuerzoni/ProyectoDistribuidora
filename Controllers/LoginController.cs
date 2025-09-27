using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ProyectoDistribuidora.Compartida.Exception;
using ProyectoDistribuidora.Data;
using ProyectoDistribuidora.Models.Legacy;
using ProyectoDistribuidora.ServicioMigracion;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using LoginRequest = ProyectoDistribuidora.Compartida.DTO.Usuario.LoginRequest; 
//Puse el using asi porque me dice definicion ambigua con un login request por defecto.

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ProyectoDistribuidora.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly LoginServicio _usuarioServicio;
        private readonly ILogger<LoginController> _logger;

        public LoginController(LoginServicio usuarioServicio, IConfiguration configuration, ILogger<LoginController> logger)
        {
            _configuration = configuration;
            _usuarioServicio = usuarioServicio;
            _logger = logger;
        }

        // GET: api/Clientes
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Usuario>>> GetUsuarios()
        {
            try
            {
                var usuarios = await _usuarioServicio.ObtenerUsuarios();
                return Ok(usuarios); // Retornar 200 OK con los datos
            }
            catch (UsuarioException ex)
            {
                // Maneja errores específicos del dominio
                _logger.LogWarning(ex, "Error específico al obtener usuarios.");
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                // Maneja cualquier error inesperado
                _logger.LogError(ex, "Error inesperado al obtener usuarios.");
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Error interno del servidor." });
            }
        }

        // GET: api/Clientes/5
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<Usuario>> GetUsuario(long id, string rol)
        {
            // Validaciones de Entrada
            if (id <= 0)
            {
                return BadRequest(new { message = "El Id del usuario debe ser mayor que 0." });
            }

            if (string.IsNullOrWhiteSpace(rol))
            {
                return BadRequest(new { message = "El Rol del usuario no puede estar vacío o nulo." });
            }

            try
            {
                var usuario = await _usuarioServicio.ObtenerUsuarioById(id, rol);
                if (usuario == null)
                {
                    return NotFound(new { message = $"No se encontró el usuario con Id {id} y Rol {rol}." });
                }

                return Ok(usuario); // Retornar 200 OK con el usuario encontrado
            }
            catch (UsuarioException ex)
            {
                // Maneja errores específicos del dominio
                _logger.LogWarning(ex, $"Error específico al obtener el usuario con Id {id} y Rol {rol}.");
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                // Maneja cualquier error inesperado
                _logger.LogError(ex, $"Error inesperado al obtener el usuario con Id {id} y Rol {rol}.");
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Error interno del servidor." });
            }
        }



        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest login)
        {
            if (login == null || string.IsNullOrWhiteSpace(login.Username) || string.IsNullOrWhiteSpace(login.Password))
            {
                return BadRequest("El nombre de usuario y la contraseña son obligatorios.");
            }
            try
            {
                // Llamar al caso de uso para autenticar al usuario
                var user = await _usuarioServicio.Login(login.Username, login.Password);

                // Generar el token JWT
                var token = GenerateJwtToken(user);

                // Retornar el token y los datos del usuario autenticado
                return Ok(new { token, user });
            }
            catch (UsuarioException ex)
            {
                return Unauthorized(ex.Message); // Credenciales inválidas
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Error interno del servidor."});
            }
        }


        private string GenerateJwtToken(Usuario user)
        {
            // Clave secreta desde la configuración
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);

            // Definir los claims (información dentro del token)
            var claims = new[]
            {
        new Claim(ClaimTypes.NameIdentifier, user.IdUsuario.ToString()), // Identificador único del usuario
        new Claim(ClaimTypes.Name, user.NombreUsuario),                 // Nombre de usuario
        new Claim(ClaimTypes.Role, user.Rol)                            // Rol del usuario
    };

            // Configuración del token de sesion
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),  // Claims que van dentro del token
                Expires = DateTime.UtcNow.AddDays(30), // Tiempo de expiración del token
                Issuer = _configuration["Jwt:Issuer"], // Quién emite el token
                Audience = _configuration["Jwt:Audience"], // Quién puede usar el token
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature) // Firma del token
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token); // Devuelve el token como string
        }
    }
}

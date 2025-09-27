using Microsoft.EntityFrameworkCore;
using ProyectoDistribuidora.Compartida.Exception;
using ProyectoDistribuidora.Compartida.Interfaces.IRepositorio;
using ProyectoDistribuidora.Data;
using ProyectoDistribuidora.Models.GestionPedidos;
using ProyectoDistribuidora.Models.Legacy;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProyectoDistribuidora.Repositorios.Legacy
{
    public class LLoginRepositorio : LIUsuarioRepositorio
    {
        private readonly LegacyContext _context;
        private readonly ILogger<LLoginRepositorio> _logger;

        // Constructor para inyectar el contexto de la base de datos y el logger
        public LLoginRepositorio(LegacyContext context, ILogger<LLoginRepositorio> logger)
        {
            _context = context;
            _logger = logger;
        }
        /// <summary>
        /// Obtiene todos los usuarios de la base de datos.
        /// </summary>
        public async Task<IEnumerable<Usuario>> GetAll()
        {
            try
            {
                var usuarios = await _context.Usuarios.ToListAsync();
                _logger.LogInformation($"Se obtuvieron {usuarios.Count} registros de usuarios.");
                return usuarios;
            }
            catch (DbUpdateException dbEx)
            {
                _logger.LogError(dbEx, "Error de base de datos al obtener todos los usuarios.");
                throw new UsuarioException("Error de base de datos al obtener todos los usuarios.", dbEx);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al obtener todos los usuarios.");
                throw new UsuarioException("Error inesperado al obtener todos los usuarios.", ex);
            }
        }

        /// <summary>
        /// Obtiene un usuario por su Id y Rol.
        /// </summary>
        public async Task<Usuario> GetById(long idUsuario, string rol)
        {
            if (idUsuario <= 0)
            {
                _logger.LogWarning("El Id proporcionado no es válido.");
                throw new UsuarioException("El Id del usuario debe ser mayor que 0.");
            }

            if (string.IsNullOrWhiteSpace(rol))
            {
                _logger.LogWarning("El Rol proporcionado no es válido.");
                throw new UsuarioException("El Rol del usuario no puede estar vacío o nulo.");
            }

            try
            {
                // Buscar el usuario en la base de datos por Id y Rol
                var usuario = await _context.Usuarios
                    .FirstOrDefaultAsync(u => u.IdUsuario == idUsuario && u.Rol == rol); // Busca el usuario que coincida con el Id y el Rol

                if (usuario == null)
                {
                    _logger.LogInformation($"No se encontró el usuario con Id {idUsuario} y Rol {rol}.");
                    return null; // O podrías lanzar una excepción si prefieres
                }

                _logger.LogInformation($"Usuario con Id {idUsuario} y Rol {rol} obtenido exitosamente.");
                return usuario; // Retorna el usuario encontrado
            }
            catch (DbUpdateException dbEx)
            {
                _logger.LogError(dbEx, $"Error al buscar el usuario con Id '{idUsuario}' y Rol '{rol}' en la base de datos.");
                throw new UsuarioException($"Error al buscar el usuario con Id '{idUsuario}' y Rol '{rol}'.", dbEx);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error inesperado al buscar el usuario con Id '{idUsuario}' y Rol '{rol}'.");
                throw new UsuarioException($"Error inesperado al buscar el usuario con Id '{idUsuario}' y Rol '{rol}'.", ex);
            }
        }

        /// <summary>
        /// Obtiene un usuario por su nombre de usuario y contraseña.
        /// </summary>
        public async Task<Usuario> GetByUsernameAndPassword(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                _logger.LogWarning("El nombre de usuario proporcionado está vacío o es nulo.");
                throw new UsuarioException("El nombre de usuario no puede estar vacío o ser nulo.");
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                _logger.LogWarning("La contraseña proporcionada está vacía o es nula.");
                throw new UsuarioException("La contraseña no puede estar vacía o ser nula.");
            }

            try
            {
                // Buscar el usuario en la base de datos por nombre de usuario y contraseña
                var usuario = await _context.Usuarios
                    .SingleOrDefaultAsync(u => u.NombreUsuario == username && u.Password == password);

                if (usuario == null)
                {
                    _logger.LogInformation($"No se encontró el usuario con NombreUsuario '{username}' y la contraseña proporcionada.");
                    throw new UsuarioException($"Nombre de usuario '{username}' y/o contraseña incorrectos.");
                }

                _logger.LogInformation($"Usuario con NombreUsuario '{username}' obtenido exitosamente.");
                return usuario; // Retorna el usuario encontrado
            }
            catch (DbUpdateException dbEx)
            {
                _logger.LogError(dbEx, $"Error al buscar el usuario con NombreUsuario '{username}' en la base de datos.");
                throw new UsuarioException($"Error al buscar el usuario con NombreUsuario '{username}'.", dbEx);
            }
            catch (Exception ex) when (!(ex is UsuarioException))
            {
                _logger.LogError(ex, $"Error inesperado al buscar el usuario con NombreUsuario '{username}'.");
                throw new UsuarioException($"Error inesperado al buscar el usuario con NombreUsuario '{username}'.", ex);
            }
        }
    }
}
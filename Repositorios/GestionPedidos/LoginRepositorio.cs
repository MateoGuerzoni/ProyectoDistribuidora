using Microsoft.EntityFrameworkCore;
using ProyectoDistribuidora.Compartida.Exception;
using ProyectoDistribuidora.Compartida.Interfaces.IRepositorio;
using ProyectoDistribuidora.Data;
using ProyectoDistribuidora.Models.GestionPedidos;

namespace ProyectoDistribuidora.Repositorios.GestionPedidos
{
    public class LoginRepositorio : IUsuarioRepositorio
    {
        private readonly ILogger<LoginRepositorio> _logger;
        private readonly GestionPedidosContext _context;

        public LoginRepositorio(GestionPedidosContext context, ILogger<LoginRepositorio> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task Add(UsuarioGP usuario)
        {
            if (usuario == null)
            {
                _logger.LogWarning("El usuario proporcionado es nulo.");
                throw new UsuarioException("El usuario no puede ser nulo.");
            }

            try
            {
                // Muestra el estado del usuario antes de guardar
                Console.WriteLine($"ContrasenaHash: {usuario.IdUsuario}, CorreoElectronico: {usuario.NombreUsuario}");

                // Agregar el nuevo usuario al contexto
                await _context.Usuarios.AddAsync(usuario);
                await _context.SaveChangesAsync(); // Guardar cambios en la base de datos

                _logger.LogInformation($"Usuario con Id {usuario.IdUsuario} agregado exitosamente.");
            }
            catch (DbUpdateException dbEx)
            {
                // Manejo específico para errores de actualización de base de datos
                _logger.LogError(dbEx, $"Error al agregar el usuario con Id '{usuario.IdUsuario}' a la base de datos.");
                throw new UsuarioException($"Error al agregar el usuario con Id '{usuario.IdUsuario}'.", dbEx);
            }
            catch (Exception ex)
            {
                // Manejo general de excepciones
                _logger.LogError(ex, $"Error inesperado al agregar el usuario con Id '{usuario.IdUsuario}'.");
                throw new UsuarioException($"Error inesperado al agregar el usuario con Id '{usuario.IdUsuario}'.", ex);
            }
        }

        public async Task Delete(UsuarioGP usuario)
        {
            if (usuario == null)
            {
                _logger.LogWarning("El usuario proporcionado es nulo.");
                throw new UsuarioException("El usuario no puede ser nulo.");
            }

            try
            {
                // Muestra el estado del usuario antes de eliminar
                Console.WriteLine($"Eliminando usuario con Id: {usuario.IdUsuario}, Rol: {usuario.Rol}, Nombre: {usuario.NombreUsuario}");

                // Eliminar el usuario del contexto
                _context.Usuarios.Remove(usuario);
                await _context.SaveChangesAsync(); // Guardar cambios en la base de datos

                _logger.LogInformation($"Usuario con Id {usuario.IdUsuario} eliminado exitosamente.");
            }
            catch (DbUpdateException dbEx)
            {
                _logger.LogError(dbEx, $"Error al eliminar el usuario con Id '{usuario.IdUsuario}' de la base de datos.");
                throw new UsuarioException($"Error al eliminar el usuario con Id '{usuario.IdUsuario}'.", dbEx);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error inesperado al eliminar el usuario con Id '{usuario.IdUsuario}'.");
                throw new UsuarioException($"Error inesperado al eliminar el usuario con Id '{usuario.IdUsuario}'.", ex);
            }
        }

        public async Task<IEnumerable<UsuarioGP>> GetAll()
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

        public async Task<UsuarioGP> GetById(long id, string rol)
        {
            if (id <= 0)
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
                    .FirstOrDefaultAsync(u => u.IdUsuario == id && u.Rol == rol); // Busca el usuario que coincida con el Id y el Rol

                if (usuario == null)
                {
                    _logger.LogInformation($"No se encontró el usuario con Id {id} y Rol {rol}.");
                    return null; // O podrías lanzar una excepción si prefieres
                }

                _logger.LogInformation($"Usuario con Id {id} y Rol {rol} obtenido exitosamente.");
                return usuario; // Retorna el usuario encontrado
            }
            catch (DbUpdateException dbEx)
            {
                _logger.LogError(dbEx, $"Error al buscar el usuario con Id '{id}' y Rol '{rol}' en la base de datos.");
                throw new UsuarioException($"Error al buscar el usuario con Id '{id}' y Rol '{rol}'.", dbEx);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error inesperado al buscar el usuario con Id '{id}' y Rol '{rol}'.");
                throw new UsuarioException($"Error inesperado al buscar el usuario con Id '{id}' y Rol '{rol}'.", ex);
            }
        }


        public async Task Update(UsuarioGP usuario)
        {
            if (usuario == null)
            {
                _logger.LogWarning("El usuario proporcionado es nulo.");
                throw new UsuarioException("El usuario no puede ser nulo.");
            }

            try
            {
                // Muestra el estado del usuario antes de actualizar
                Console.WriteLine($"Actualizando usuario con Id: {usuario.IdUsuario}, Nombre: {usuario.NombreUsuario}");

                // Actualizar el usuario en el contexto
                _context.Usuarios.Update(usuario);
                await _context.SaveChangesAsync(); // Guardar cambios en la base de datos

                _logger.LogInformation($"Usuario con Id {usuario.IdUsuario} actualizado exitosamente.");
            }
            catch (DbUpdateException dbEx)
            {
                _logger.LogError(dbEx, $"Error al actualizar el usuario con Id '{usuario.IdUsuario}' en la base de datos.");
                throw new UsuarioException($"Error al actualizar el usuario con Id '{usuario.IdUsuario}'.", dbEx);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error inesperado al actualizar el usuario con Id '{usuario.IdUsuario}'.");
                throw new UsuarioException($"Error inesperado al actualizar el usuario con Id '{usuario.IdUsuario}'.", ex);
            }
        }
    }
}

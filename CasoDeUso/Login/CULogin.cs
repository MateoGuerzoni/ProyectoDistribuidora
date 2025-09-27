using ProyectoDistribuidora.Compartida.Exception;
using ProyectoDistribuidora.Compartida.Interfaces.IRepositorio;
using ProyectoDistribuidora.Models.GestionPedidos;
using ProyectoDistribuidora.Models.Legacy;


public class CULogin
{
    private readonly LIUsuarioRepositorio _usuarioRepositorio;

    public CULogin(LIUsuarioRepositorio usuarioRepositorio)
    {
        _usuarioRepositorio = usuarioRepositorio ?? throw new ArgumentNullException(nameof(usuarioRepositorio));
    }

    public async Task<Usuario> Execute(string username, string password)
    {
        // Validar que los parámetros no sean nulos o vacíos
        if (string.IsNullOrWhiteSpace(username))
        {
            throw new UsuarioException("El nombre de usuario no puede estar vacío.");
        }

        if (string.IsNullOrWhiteSpace(password))
        {
            throw new UsuarioException("La contraseña no puede estar vacía.");
        }

        try
        {
            // Buscar el usuario con las credenciales proporcionadas
            var usuario = await _usuarioRepositorio.GetByUsernameAndPassword(username, password);

            if (usuario == null)
            {
                throw new UsuarioException("Credenciales inválidas.");
            }

            return usuario;
        }
        catch (UsuarioException)
        {
            // Re-lanzar excepciones de negocio sin modificarlas
            throw;
        }
        catch (Exception ex)
        {
            throw new UsuarioException("Error inesperado al intentar iniciar sesión.", ex);
        }
    }
}

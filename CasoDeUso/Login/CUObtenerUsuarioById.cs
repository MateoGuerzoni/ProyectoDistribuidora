using ProyectoDistribuidora.Compartida.Exception;
using ProyectoDistribuidora.Compartida.Interfaces.IRepositorio;
using ProyectoDistribuidora.Models.GestionPedidos;

public class CUObtenerUsuarioById
{
    private readonly IUsuarioRepositorio _usuarioRepositorio;

    public CUObtenerUsuarioById(IUsuarioRepositorio usuarioRepositorio)
    {
        _usuarioRepositorio = usuarioRepositorio ?? throw new ArgumentNullException(nameof(usuarioRepositorio));
    }

    public async Task<UsuarioGP> Execute(long idUsuario, string rol)
    {
        // Validar el Id del usuario
        if (idUsuario <= 0)
        {
            throw new UsuarioException("El Id del usuario debe ser mayor que 0.");
        }

        // Validar el Rol del usuario
        if (string.IsNullOrWhiteSpace(rol))
        {
            throw new UsuarioException("El Rol del usuario no puede ser nulo o vacío.");
        }

        var usuario = await _usuarioRepositorio.GetById(idUsuario, rol);

        if (usuario == null)
        {
            throw new UsuarioException($"No se encontró un usuario con el ID {idUsuario} y rol {rol}.");
        }

        return usuario;
    }

}

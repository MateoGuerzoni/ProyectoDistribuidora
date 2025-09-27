using ProyectoDistribuidora.Compartida.Exception;
using ProyectoDistribuidora.Compartida.Interfaces.IRepositorio;
using ProyectoDistribuidora.Models.GestionPedidos;

public class CUObtenerUsuarios
{
    private readonly IUsuarioRepositorio _usuarioRepositorio;

    public CUObtenerUsuarios(IUsuarioRepositorio usuarioRepositorio)
    {
        _usuarioRepositorio = usuarioRepositorio ?? throw new ArgumentNullException(nameof(usuarioRepositorio));
    }

    public async Task<IEnumerable<UsuarioGP>> Execute()
    {
        var usuarios = await _usuarioRepositorio.GetAll();
        if (usuarios == null)
        {
            throw new UsuarioException("No se pudo obtener la lista de usuarios.");
        }
        return usuarios;
    }
}

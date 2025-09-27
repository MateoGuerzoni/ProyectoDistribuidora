using ProyectoDistribuidora.Compartida.Interfaces.IRepositorio;
using ProyectoDistribuidora.Models.Legacy;
using ProyectoDistribuidora.Models.GestionPedidos;
using ProyectoDistribuidora.Compartida.Exception;

namespace ProyectoDistribuidora.CasoDeUso.Login
{
    public class LCUObtenerUsuarioById
    {
        private readonly LIUsuarioRepositorio _usuarioRepositorio;

        public LCUObtenerUsuarioById(LIUsuarioRepositorio usuarioRepositorio)
        {
            _usuarioRepositorio = usuarioRepositorio;
        }

        public async Task<Models.Legacy.Usuario> Execute(long id, string rol)
        {
            // Validar el Id del usuario
            if (id <= 0)
            {
                throw new UsuarioException("El Id del usuario debe ser mayor que 0.");
            }

            // Validar el Rol del usuario
            if (string.IsNullOrWhiteSpace(rol))
            {
                throw new UsuarioException("El Rol del usuario no puede ser nulo o vacío.");
            }

            // Llama al repositorio para obtener el usuario por Id y Rol
            var usuario = await _usuarioRepositorio.GetById(id, rol);
            if (usuario == null)
            {
                throw new UsuarioException($"No se encontró un usuario con el ID {id} y rol {rol}.");
            }

            return usuario;
        }

    }
}

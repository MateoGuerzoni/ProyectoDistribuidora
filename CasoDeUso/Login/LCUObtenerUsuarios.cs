using ProyectoDistribuidora.Compartida.Exception;
using ProyectoDistribuidora.Compartida.Interfaces.IRepositorio;
using ProyectoDistribuidora.Models.Legacy;

namespace ProyectoDistribuidora.CasoDeUso.Login
{
    public class LCUObtenerUsuarios
    {
        private readonly LIUsuarioRepositorio _usuarioRepositorio;

        public LCUObtenerUsuarios(LIUsuarioRepositorio usuarioRepositorio)
        {
            _usuarioRepositorio = usuarioRepositorio;
        }

        public async Task<IEnumerable<Models.Legacy.Usuario>> Execute()
        {
            var usuarios = await _usuarioRepositorio.GetAll();
            if (usuarios == null)
            {
                throw new UsuarioException("No se pudo obtener la lista de usuarios.");
            }
            return usuarios;
        }
    }
}

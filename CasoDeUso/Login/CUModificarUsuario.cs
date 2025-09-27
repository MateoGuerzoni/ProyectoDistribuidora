using ProyectoDistribuidora.Compartida.DTO.Usuario;
using ProyectoDistribuidora.Compartida.Exception;
using ProyectoDistribuidora.Compartida.Interfaces.IRepositorio;
using ProyectoDistribuidora.Models.GestionPedidos;

namespace ProyectoDistribuidora.CasoDeUso.Login
{
    public class CUModificarUsuario
    {
        private readonly IUsuarioRepositorio _usuarioRepositorio;

        public CUModificarUsuario(IUsuarioRepositorio usuarioRepositorio)
        {
            _usuarioRepositorio = usuarioRepositorio;
        }

        public async Task<UsuarioGP> Execute(UsuarioDTO usuarioDTO)
        {
            // Validar el DTO
            if (usuarioDTO == null)
            {
                throw new UsuarioException("El usuario no puede ser nulo.");
            }

            // Validar las propiedades requeridas del DTO
            if (usuarioDTO.IdUsuario <= 0)
            {
                throw new UsuarioException("El Id del usuario debe ser mayor que 0.");
            }

            if (string.IsNullOrWhiteSpace(usuarioDTO.Rol))
            {
                throw new UsuarioException("El Rol del usuario no puede ser nulo o vacío.");
            }

            if (string.IsNullOrWhiteSpace(usuarioDTO.NombreUsuario))
            {
                throw new UsuarioException("El Nombre de usuario no puede ser nulo o vacío.");
            }

            // Buscar el usuario existente en la base de datos
            var usuarioExistente = await _usuarioRepositorio.GetById(usuarioDTO.IdUsuario, usuarioDTO.Rol);
            if (usuarioExistente == null)
            {
                throw new UsuarioException($"El usuario con Id {usuarioDTO.IdUsuario} no existe.");
            }

            // Actualizar las propiedades del usuario
            usuarioExistente.NombreUsuario = usuarioDTO.NombreUsuario;
            // Agrega o actualiza otras propiedades según tu modelo

            // Guardar los cambios en la base de datos
            await _usuarioRepositorio.Update(usuarioExistente);

            return usuarioExistente; // Retorna la entidad modificada
        }

    }
}

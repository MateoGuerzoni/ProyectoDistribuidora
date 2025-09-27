using ProyectoDistribuidora.Compartida.DTO.Usuario;
using ProyectoDistribuidora.Compartida.Exception;
using ProyectoDistribuidora.Compartida.Interfaces.IRepositorio;
using ProyectoDistribuidora.Compartida.Maper;
using ProyectoDistribuidora.Models.GestionPedidos;

namespace ProyectoDistribuidora.CasoDeUso.Login
{
    public class CUBajaUsuario
    {
        private readonly IUsuarioRepositorio _usuarioRepositorio;

        public CUBajaUsuario(IUsuarioRepositorio usuarioRepositorio)
        {
            _usuarioRepositorio = usuarioRepositorio;
        }

        public async Task Execute(long id, string rol)
        {
            // Validar el Id del usuario
            if (id <= 0)
            {
                throw new UsuarioException("El Id del usuario no puede ser nulo o negativo.");
            }

            // Validar el Rol del usuario
            if (string.IsNullOrWhiteSpace(rol))
            {
                throw new UsuarioException("El Rol del usuario no puede ser nulo o vacío.");
            }

            // Buscar el usuario existente en la base de datos
            var usuarioExistente = await _usuarioRepositorio.GetById(id, rol);
            if (usuarioExistente == null)
            {
                throw new UsuarioException($"El usuario con ID {id} y Rol {rol} no existe.");
            }

            // Llama al repositorio para eliminar el usuario
            await _usuarioRepositorio.Delete(usuarioExistente);
        }

    }
}

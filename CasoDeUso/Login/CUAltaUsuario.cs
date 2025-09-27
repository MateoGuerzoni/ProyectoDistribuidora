using ProyectoDistribuidora.Compartida.DTO.Usuario;
using ProyectoDistribuidora.Compartida.Interfaces.IRepositorio;
using ProyectoDistribuidora.Compartida.Maper;
using ProyectoDistribuidora.Models.Legacy;
using ProyectoDistribuidora.Models.GestionPedidos;
using ProyectoDistribuidora.Compartida.Exception;

namespace ProyectoDistribuidora.CasoDeUso.Login
{
    public class CUAltaUsuario
    {
        private readonly IUsuarioRepositorio _usuarioRepositorio;

        public CUAltaUsuario(IUsuarioRepositorio usuarioRepositorio)
        {
            _usuarioRepositorio = usuarioRepositorio;
        }

        public async Task<UsuarioGP> Execute(UsuarioDTO nuevoUsuarioDTO)
        {
            // Validar el DTO
            if (nuevoUsuarioDTO == null)
            {
                throw new UsuarioException("El usuario no puede ser nulo.");
            }

            // Validar las propiedades requeridas del DTO
            if (string.IsNullOrWhiteSpace(nuevoUsuarioDTO.Rol))
            {
                throw new UsuarioException("El rol del usuario no puede ser nulo o vacío.");
            }

            if (string.IsNullOrWhiteSpace(nuevoUsuarioDTO.NombreUsuario))
            {
                throw new UsuarioException("El nombre de usuario no puede ser nulo o vacío.");
            }

            if (string.IsNullOrWhiteSpace(nuevoUsuarioDTO.Password))
            {
                throw new UsuarioException("La contraseña del usuario no puede ser nula o vacía.");
            }

            // Mapear el DTO a la entidad Usuario
            var nuevoUsuario = new UsuarioGP
            {
                IdUsuario = nuevoUsuarioDTO.IdUsuario,
                Rol = nuevoUsuarioDTO.Rol,
                NombreUsuario = nuevoUsuarioDTO.NombreUsuario,
                Password = nuevoUsuarioDTO.Password,
            };

            // Llama al repositorio para dar de alta el usuario
            await _usuarioRepositorio.Add(nuevoUsuario);

            return nuevoUsuario; // Retorna la entidad creada (o el DTO si prefieres)
        }
    }
}

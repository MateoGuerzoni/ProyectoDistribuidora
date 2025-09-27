using ProyectoDistribuidora.Compartida.DTO.Usuario;
using ProyectoDistribuidora.Models.Legacy;
using ProyectoDistribuidora.Models.GestionPedidos;

namespace ProyectoDistribuidora.Compartida.Maper
{
    public class UsuarioMapeo
    {
        public static UsuarioDTO MapLegacyToDTO(Usuario usuarioViejo)
        {
            if (usuarioViejo == null)
            {
                throw new ArgumentNullException(nameof(usuarioViejo), "El usuario no puede ser nulo");
            }
            return new UsuarioDTO
            {
                IdUsuario = usuarioViejo.IdUsuario,
                Rol = usuarioViejo.Rol,
                NombreUsuario = usuarioViejo.NombreUsuario,
                Password = usuarioViejo.Password
            };
        }
        public static UsuarioDTO MapGestionPedidosToDTO(UsuarioGP usuarioNuevo)
        {
            if (usuarioNuevo == null)
            {
                throw new ArgumentNullException(nameof(usuarioNuevo), "El usuario no puede ser nulo");
            }
            return new UsuarioDTO
            {
                IdUsuario = usuarioNuevo.IdUsuario,
                Rol = usuarioNuevo.Rol,
                NombreUsuario = usuarioNuevo.NombreUsuario,
                Password = usuarioNuevo.Password
            };
        }
    }
}

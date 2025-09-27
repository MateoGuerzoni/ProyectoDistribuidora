using ProyectoDistribuidora.Models;
using ProyectoDistribuidora.Models.GestionPedidos;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProyectoDistribuidora.Compartida.Maper;
using ProyectoDistribuidora.Compartida.DTO.Usuario;
using ProyectoDistribuidora.Repositorios;
using ProyectoDistribuidora.CasoDeUso.Login;
using ProyectoDistribuidora.Repositorios.GestionPedidos;
using ProyectoDistribuidora.Models.Legacy;
using ProyectoDistribuidora.Compartida.Exception;

namespace ProyectoDistribuidora.ServicioMigracion
{
    public class LoginServicio
    {
        private readonly CUAltaUsuario _CUAltaUsuario;
        private readonly LCUObtenerUsuarios _LCUObtenerUsuarios;
        private readonly LCUObtenerUsuarioById _LCUObtenerUsuarioById;
        private readonly CUModificarUsuario _CUModificarUsuario;
        private readonly CUBajaUsuario _CUBajaUsuario;
        private readonly CUObtenerUsuarioById _CUObtenerUsuarioById;
        private readonly CUObtenerUsuarios _CUObtenerUsuarios;
        private readonly CULogin _CULogin;
        public LoginServicio(LCUObtenerUsuarios obtenerUsuariosCasoDeUso, CUAltaUsuario altaUsuarioGestionPedidos,
                               LCUObtenerUsuarioById obtenerUsuarioPorIdLegacy, CUModificarUsuario modificarUsuario,
                               CUBajaUsuario bajaUsuario, CUObtenerUsuarios obtenerUsuarios, CUObtenerUsuarioById obtenerUsuarioPorId,
                               CULogin login)
        {
            _LCUObtenerUsuarios = obtenerUsuariosCasoDeUso;
            _CUAltaUsuario = altaUsuarioGestionPedidos;
            _LCUObtenerUsuarioById = obtenerUsuarioPorIdLegacy;
            _CUBajaUsuario = bajaUsuario;
            _CUModificarUsuario = modificarUsuario;
            _CUObtenerUsuarioById = obtenerUsuarioPorId;
            _CUObtenerUsuarios = obtenerUsuarios;
            _CULogin = login;
        }

        public async Task<IEnumerable<UsuarioDTO>> LObtenerUsuarios()
        {
            // Llama al caso de uso para obtener todos los usuarios
            var usuariosViejos = await _LCUObtenerUsuarios.Execute();
            return usuariosViejos.Select(usuario => UsuarioMapeo.MapLegacyToDTO(usuario)); // Mapea a UsuarioDTO
        }
        public async Task<IEnumerable<UsuarioDTO>> ObtenerUsuarios()
        {
            // Llama al caso de uso para obtener todos los usuarios
            var usuarios = await _CUObtenerUsuarios.Execute();
            return usuarios.Select(usuario => UsuarioMapeo.MapGestionPedidosToDTO(usuario)); // Mapea a UsuarioDTO
        }
        public async Task<UsuarioDTO> LObtenerUsuarioById(long idUsuario, string rol)
        {
            // Llama al caso de uso para obtener un usuario por ID y Rol
            var usuario = await _LCUObtenerUsuarioById.Execute(idUsuario, rol);

            // Mapea cada usuario de la entidad legacy a UsuarioDTO
            return UsuarioMapeo.MapLegacyToDTO(usuario);
        }
        public async Task<UsuarioDTO> ObtenerUsuarioById(long idUsuario, string rol)
        {
            // Llama al caso de uso para obtener un usuario por ID y Rol
            var usuario = await _CUObtenerUsuarioById.Execute(idUsuario, rol);

            // Mapea cada usuario de la entidad GP a UsuarioDTO
            return UsuarioMapeo.MapGestionPedidosToDTO(usuario);
        }
        public async Task<UsuarioDTO> AltaUsuario(long idUsuario, string rol)
        {
            // Llama a ObtenerUsuarioLegacyById pasándole el idUsuario y el rol
            var usuarioDTO = await LObtenerUsuarioById(idUsuario, rol);
            // Llama al caso de uso para dar de alta un nuevo usuario
            UsuarioGP usuarioCreado = await _CUAltaUsuario.Execute(usuarioDTO);
            return UsuarioMapeo.MapGestionPedidosToDTO(usuarioCreado); // Retorna el DTO del usuario creado
        }

        public async Task<UsuarioDTO> ModificarUsuario(int id, string rol)
        {
            var usuarioDTO = await LObtenerUsuarioById(id, rol);
            UsuarioGP usuario = await _CUModificarUsuario.Execute(usuarioDTO);
            return UsuarioMapeo.MapGestionPedidosToDTO(usuario); //Retorna el DTO del usuario modificado
        }

        public async Task BajaUsuario(long id, string rol) //capaz aplicar este formato a todos????
        {
            await _CUBajaUsuario.Execute(id, rol);
        }

        public async Task<Usuario> Login(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentException("El nombre de usuario y la contraseña no pueden estar vacíos.");
            }

            try
            {
                // Llama al caso de uso para autenticar al usuario
                return await _CULogin.Execute(username, password);
            }
            catch (UsuarioException)
            {
                throw; // Re-lanza la excepción para manejarla en el controlador
            }
            catch (Exception ex)
            {
                throw new Exception("Error en el servicio de autenticación.", ex);
            }
        }
    }
}

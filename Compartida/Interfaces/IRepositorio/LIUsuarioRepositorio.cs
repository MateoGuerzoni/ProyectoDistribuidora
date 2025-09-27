using ProyectoDistribuidora.Models;
using ProyectoDistribuidora.Models.GestionPedidos;
using ProyectoDistribuidora.Models.Legacy;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProyectoDistribuidora.Compartida.Interfaces.IRepositorio
{
    public interface LIUsuarioRepositorio
    {
        Task<IEnumerable<Usuario>> GetAll(); // Obtener todos los usuarios
        Task<Usuario> GetById(long idUsuario, string rol); // Obtener un usuario por ID
        Task<Usuario> GetByUsernameAndPassword(string username, string password);
    }
}
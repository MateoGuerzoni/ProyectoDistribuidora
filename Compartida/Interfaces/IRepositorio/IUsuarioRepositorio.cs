using ProyectoDistribuidora.Models.Legacy;
using ProyectoDistribuidora.Models.GestionPedidos;

namespace ProyectoDistribuidora.Compartida.Interfaces.IRepositorio
{
   public interface IUsuarioRepositorio
   {
        Task Add(UsuarioGP usuario);
        Task<IEnumerable<UsuarioGP>> GetAll(); // Obtener todos los usuarios
        Task<UsuarioGP> GetById(long id, string rol); // Obtener un usuario por ID
        Task Delete(UsuarioGP usuario);
        Task Update(UsuarioGP usuario);
    }
}

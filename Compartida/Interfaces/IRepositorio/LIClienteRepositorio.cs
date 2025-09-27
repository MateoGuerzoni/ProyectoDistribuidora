using ProyectoDistribuidora.Models.Legacy;

namespace ProyectoDistribuidora.Compartida.Interfaces.IRepositorio
{
    public interface LIClienteRepositorio
    {
        Task<IEnumerable<Cliente>> GetAll(); // Obtener todos los usuarios
        Task<Cliente> GetById(long NroCLiente); // Obtener un usuario por ID
        Task Update(Cliente cliente);
        Task Delete(long NroCLiente);
        Task AddCliente(Cliente cliente);
    }
}

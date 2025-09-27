using ProyectoDistribuidora.Models.GestionPedidos;

namespace ProyectoDistribuidora.Compartida.Interfaces.IRepositorio
{
    public interface IClienteRepositorio
    {
        Task Add(ClienteGP cliente); // Agregar un nuevo cliente
        Task<IEnumerable<ClienteGP>> GetAll(); // Obtener todos los clientes
        Task<ClienteGP> GetById(long NroCliente); // Obtener un cliente por ID
        Task Delete(ClienteGP cliente); // Eliminar un cliente
        Task Update(ClienteGP cliente); // Actualizar un cliente
    }
}

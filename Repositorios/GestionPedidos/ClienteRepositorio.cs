using Microsoft.EntityFrameworkCore;
using ProyectoDistribuidora.Compartida.Interfaces.IRepositorio;
using ProyectoDistribuidora.Data;
using ProyectoDistribuidora.Models.GestionPedidos;
using ProyectoDistribuidora.Compartida.Exception; // Asegúrate de que aquí esté tu ClienteException

namespace ProyectoDistribuidora.Repositorios.GestionPedidos
{
    public class ClienteRepositorio : IClienteRepositorio
    {
        private readonly ILogger<ClienteRepositorio> _logger;
        private readonly GestionPedidosContext _context;

        public ClienteRepositorio(GestionPedidosContext context, ILogger<ClienteRepositorio> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task Add(ClienteGP cliente)
        {
            if (cliente == null)
            {
                _logger.LogWarning("El cliente proporcionado es nulo.");
                throw new ClienteException("El cliente no puede ser nulo.");
            }

            try
            {
                Console.WriteLine($"Agregando cliente con NroCliente: {cliente.NroCliente}, Nombre: {cliente.NombreCliente}");

                await _context.Clientes.AddAsync(cliente);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException dbEx)
            {
                _logger.LogError(dbEx, "Error al agregar el cliente a la base de datos.");
                throw new ClienteException("Error al agregar el cliente a la base de datos.", dbEx);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al agregar el cliente.");
                throw new ClienteException("Error inesperado al agregar el cliente.", ex);
            }
        }

        public async Task Delete(ClienteGP cliente)
        {
            if (cliente == null)
            {
                _logger.LogWarning("El cliente proporcionado es nulo.");
                throw new ClienteException("El cliente no puede ser nulo.");
            }

            try
            {
                Console.WriteLine($"Eliminando cliente con NroCliente: {cliente.NroCliente}, Nombre: {cliente.NombreCliente}");

                _context.Clientes.Remove(cliente);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException dbEx)
            {
                _logger.LogError(dbEx, "Error al eliminar el cliente de la base de datos.");
                throw new ClienteException("Error al eliminar el cliente de la base de datos.", dbEx);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al eliminar el cliente.");
                throw new ClienteException("Error inesperado al eliminar el cliente.", ex);
            }
        }

        public async Task<IEnumerable<ClienteGP>> GetAll()
        {
            try
            {
                return await _context.Clientes.ToListAsync();
            }
            catch (DbUpdateException dbEx)
            {
                _logger.LogError(dbEx, "Error de base de datos al obtener todos los clientes.");
                throw new ClienteException("Error de base de datos al obtener la lista de clientes.", dbEx);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al obtener todos los clientes.");
                throw new ClienteException("Error inesperado al obtener la lista de clientes.", ex);
            }
        }

        public async Task<ClienteGP> GetById(long nroCliente)
        {
            if (nroCliente <= 0)
            {
                _logger.LogWarning("El NroCliente proporcionado no es válido.");
                throw new ClienteException("El número de cliente debe ser mayor que 0.");
            }

            try
            {
                var cliente = await _context.Clientes
                                            .FirstOrDefaultAsync(c => c.NroCliente == nroCliente);

                if (cliente == null)
                {
                    _logger.LogInformation($"No se encontró el cliente con NroCliente {nroCliente}.");
                    return null;
                }

                return cliente;
            }
            catch (DbUpdateException dbEx)
            {
                _logger.LogError(dbEx, $"Error SQL al buscar el cliente por NroCliente={nroCliente}.");
                throw new ClienteException($"Error SQL al buscar el cliente con NroCliente={nroCliente}.", dbEx);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, $"Error inesperado al buscar el cliente con NroCliente={nroCliente}.");
                throw new ClienteException($"Error inesperado al buscar el cliente con NroCliente={nroCliente}.", ex);
            }
        }

        public async Task Update(ClienteGP cliente)
        {
            if (cliente == null)
            {
                _logger.LogWarning("El cliente proporcionado es nulo.");
                throw new ClienteException("El cliente no puede ser nulo.");
            }

            try
            {
                Console.WriteLine($"Actualizando cliente con NroCliente: {cliente.NroCliente}, Nombre: {cliente.NombreCliente}");

                _context.Clientes.Update(cliente);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException dbEx)
            {
                _logger.LogError(dbEx, "Error al actualizar el cliente en la base de datos.");
                throw new ClienteException("Error al actualizar el cliente en la base de datos.", dbEx);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al actualizar el cliente.");
                throw new ClienteException("Error inesperado al actualizar el cliente.", ex);
            }
        }
    }
}

using Microsoft.EntityFrameworkCore;
using ProyectoDistribuidora.Compartida.Interfaces.IRepositorio;
using ProyectoDistribuidora.Data;
using ProyectoDistribuidora.Models.Legacy;
using ProyectoDistribuidora.Compartida.Exception; // Asegúrate de que aquí esté tu ClienteException

namespace ProyectoDistribuidora.Repositorios.Legacy
{
    public class LClienteRepositorio : LIClienteRepositorio
    {
        private readonly LegacyContext _context;

        public LClienteRepositorio(LegacyContext context)
        {
            _context = context;
        }

        public async Task AddCliente(Cliente cliente)
        {
            if (cliente == null)
            {
                throw new ClienteException("El cliente no puede ser nulo.");
            }

            try
            {
                // Agregar el cliente al contexto
                await _context.Clientes.AddAsync(cliente);

                // Guardar los cambios en la base de datos
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException dbEx)
            {
                // Ejemplo: Si quieres algo más descriptivo en tu ClienteException
                throw new ClienteException("Error de base de datos al agregar el cliente.", dbEx);
            }
            catch (Exception ex)
            {
                // Manejo genérico
                throw new ClienteException("Error inesperado al agregar el cliente.", ex);
            }
        }

        public async Task Delete(long NroCliente)
        {
            // Verifica que el ID sea válido
            if (NroCliente <= 0)
            {
                throw new ClienteException("El número de cliente debe ser mayor a cero.");
            }

            // Buscar el cliente en la base de datos
            var cliente = await _context.Clientes.FindAsync(NroCliente);

            if (cliente == null)
            {
                // Si no se encuentra el cliente, lanzar una excepción de dominio
                throw new ClienteException($"No se encontró el cliente con NroCliente {NroCliente}.");
            }

            // Eliminar el cliente del contexto
            _context.Clientes.Remove(cliente);

            // Guardar los cambios en la base de datos
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Cliente>> GetAll()
        {
            try
            {
                // Obtiene todos los clientes de la base de datos
                return await _context.Clientes.ToListAsync();
            }
            catch (DbUpdateException dbEx)
            {
                throw new ClienteException("Error de base de datos al obtener la lista de clientes.", dbEx);
            }
            catch (Exception ex)
            {
                throw new ClienteException("Error inesperado al obtener la lista de clientes.", ex);
            }
        }

        public async Task<Cliente?> GetById(long nroCliente)
        {
            try
            {
                // Obtiene un cliente por su número
                var cliente = await _context.Clientes
                                            .FirstOrDefaultAsync(c => c.Nro_Cliente == nroCliente);
                // Devuelve null si no existe
                return cliente;
            }
            catch (DbUpdateException dbEx)
            {
                throw new ClienteException(
                    $"Error de base de datos al obtener el cliente con NroCliente={nroCliente}.", dbEx
                );
            }
            catch (Exception ex)
            {
                throw new ClienteException(
                    $"Error inesperado al obtener el cliente con NroCliente={nroCliente}.", ex
                );
            }
        }

        public async Task Update(Cliente cliente)
        {
            if (cliente == null)
            {
                throw new ClienteException("El cliente no puede ser null.");
            }

            try
            {
                // Marca la entidad como modificada
                _context.Clientes.Update(cliente);

                // Guarda los cambios
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException dbEx)
            {
                // Manejo específico para errores de actualización
                throw new ClienteException(
                    "Ocurrió un error al intentar actualizar el cliente. Revisa los triggers o restricciones en la tabla Clientes.",
                    dbEx
                );
            }
            catch (Exception ex)
            {
                // Manejo genérico de errores
                throw new ClienteException("Ocurrió un error inesperado al intentar actualizar el cliente.", ex);
            }
        }
    }
}

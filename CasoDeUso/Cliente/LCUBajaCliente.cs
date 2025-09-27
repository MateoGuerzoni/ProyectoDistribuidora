using ProyectoDistribuidora.Compartida.Exception;
using ProyectoDistribuidora.Compartida.Interfaces.IRepositorio;

namespace ProyectoDistribuidora.CasoDeUso.Cliente
{
    public class LCUBajaCliente
    {
        private readonly LIClienteRepositorio _clienteRepositorio; // Repositorio para interactuar con la base de datos
        private readonly ILogger<LCUBajaCliente> _logger; // Logger para registrar eventos o errores

        public LCUBajaCliente(LIClienteRepositorio clienteRepositorio, ILogger<LCUBajaCliente> logger)
        {
            _clienteRepositorio = clienteRepositorio ?? throw new ArgumentNullException(nameof(clienteRepositorio));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task Execute(long id)
        {
            // Validar que el ID del cliente sea válido
            if (id <= 0)
            {
                throw new ClienteException("El ID del cliente debe ser mayor que cero.");
            }

            try
            {
                // Buscar el cliente antes de intentar eliminarlo
                var clienteExistente = await _clienteRepositorio.GetById(id);
                if (clienteExistente == null)
                {
                    throw new ClienteException($"No se encontró un cliente con el ID {id}.");
                }

                // Eliminar el cliente
                await _clienteRepositorio.Delete(id);
                _logger.LogInformation($"Cliente con ID {id} eliminado exitosamente.");
            }
            catch (ClienteException ex)
            {
                _logger.LogError(ex, $"Error de dominio al eliminar el cliente con ID {id}.");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error inesperado al intentar eliminar el cliente con ID {id}.");
                throw new ClienteException($"Error inesperado al eliminar el cliente con ID {id}.", ex);
            }
        }
    }
}

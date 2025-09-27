using ProyectoDistribuidora.Compartida.Exception;
using ProyectoDistribuidora.Compartida.Interfaces.IRepositorio;
using ProyectoDistribuidora.Models.Legacy;

namespace ProyectoDistribuidora.CasoDeUso.Cliente
{
    public class LCUAltaCliente
    {
        private readonly LIClienteRepositorio _clienteRepositorio;
        private readonly ILogger<LCUAltaCliente> _logger;

        public LCUAltaCliente(LIClienteRepositorio clienteRepositorio, ILogger<LCUAltaCliente> logger)
        {
            _clienteRepositorio = clienteRepositorio ?? throw new ArgumentNullException(nameof(clienteRepositorio));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Models.Legacy.Cliente> Execute(Models.Legacy.Cliente cliente)
        {
            // Validar que el cliente no sea nulo
            if (cliente == null)
            {
                throw new ClienteException("El cliente no puede ser nulo.");
            }

            // Validaciones adicionales
            if (cliente.Nro_Cliente <= 0)
            {
                throw new ClienteException("El número de cliente debe ser mayor que 0.");
            }

            if (string.IsNullOrWhiteSpace(cliente.Nombre_Cliente))
            {
                throw new ClienteException("El nombre del cliente no puede estar vacío.");
            }

            if (string.IsNullOrWhiteSpace(cliente.Dir_Cliente))
            {
                throw new ClienteException("La dirección del cliente no puede estar vacía.");
            }

            if (cliente.Puntos < 0)
            {
                throw new ClienteException("Los puntos del cliente no pueden ser negativos.");
            }

            try
            {
                // Insertar el nuevo cliente directamente en el repositorio
                await _clienteRepositorio.AddCliente(cliente);
                _logger.LogInformation($"Cliente con NroCliente {cliente.Nro_Cliente} agregado exitosamente.");

                return cliente; // Devuelve el cliente insertado
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al intentar agregar el cliente con NroCliente {cliente.Nro_Cliente}.");
                throw new ClienteException($"Error al agregar el cliente con NroCliente {cliente.Nro_Cliente}.", ex);
            }
        }
    }
}

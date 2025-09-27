using ProyectoDistribuidora.CasoDeUso.Cliente;
using ProyectoDistribuidora.Compartida.DTO.Cliente;
using ProyectoDistribuidora.Compartida.Maper;
using ProyectoDistribuidora.Models.GestionPedidos;
using ProyectoDistribuidora.Models.Legacy;

namespace ProyectoDistribuidora.ServicioMigracion
{
    public class ClienteServicio
    {
        private readonly CUAltaCliente _CUAltaCliente;
        private readonly LCUObtenerClientes _LCUObtenerClientes;
        private readonly LCUObtenerClienteById _LCUObtenerClienteById;
        private readonly CUModificarCliente _CUModificarCliente;
        private readonly CUBajaCliente _CUBajaCliente;
        private readonly CUObtenerClientes _CUObtenerClientes;
        private readonly CUObtenerClienteById _CUObtenerClienteById;
        private readonly LCUModificarCliente _LCUModificarCliente;
        private readonly LCUBajaCliente _LCUBajaCliente;
        private readonly LCUAltaCliente _LCUAltaCliente;
        // Constructor para inyectar los casos de uso
        public ClienteServicio(LCUObtenerClientes obtenerClientesLegacyCasoDeUso, CUAltaCliente altaClienteGestionPedidos,
                               LCUObtenerClienteById obtenerLegacyClienteById, CUModificarCliente modificarCliente,
                               CUBajaCliente bajaCliente, CUObtenerClientes obtenerClientesCasoDeUso,
                               CUObtenerClienteById obtenerClienteById, LCUModificarCliente modificarClienteLegacy,
                               LCUBajaCliente bajaClienteLegacy, LCUAltaCliente altaClienteLegacy)
        {
            _LCUObtenerClientes = obtenerClientesLegacyCasoDeUso;
            _CUAltaCliente = altaClienteGestionPedidos;
            _LCUObtenerClienteById = obtenerLegacyClienteById;
            _CUBajaCliente = bajaCliente;
            _CUModificarCliente = modificarCliente;
            _CUObtenerClientes = obtenerClientesCasoDeUso;
            _CUObtenerClienteById = obtenerClienteById;
            _LCUModificarCliente = modificarClienteLegacy;
            _LCUBajaCliente = bajaClienteLegacy;
            _LCUAltaCliente = altaClienteLegacy;
        }

        // Método para obtener todos los clientes de la clase legacy
        public async Task<IEnumerable<ClienteDTO>> LObtenerClientes()
        {
            // Llama al caso de uso para obtener todos los clientes
            var clientesViejos = await _LCUObtenerClientes.Execute();
            return clientesViejos.Select(cliente => ClienteMapeo.MapLegacyToDTO(cliente)); // Mapea a ClienteDTO
        }
        public async Task<IEnumerable<ClienteDTO>> ObtenerClientes()
        {
            // Llama al caso de uso para obtener todos los clientes
            var clientess = await _CUObtenerClientes.Execute();
            return clientess.Select(cliente => ClienteMapeo.MapGestionPedidosToDTO(cliente)); // Mapea a ClienteDTO
        }
        public async Task<ClienteDTO?> LObtenerClienteById(long nroCliente)
        {
            // Llama al caso de uso para obtener un cliente por ID
            var cliente = await _LCUObtenerClienteById.Execute(nroCliente);

            // Si el cliente es null, retorna null directamente
            if (cliente == null)
            {
                return null;
            }

            // Mapea el cliente a ClienteDTO
            return ClienteMapeo.MapLegacyToDTO(cliente);
        }
        // Método para obtener un cliente por ID
        public async Task<ClienteDTO> ObtenerClienteById(long nroCliente)
        {
            // Llama al caso de uso para obtener un cliente por ID
            var cliente = await _CUObtenerClienteById.Execute(nroCliente);

            // Mapea cada cliente de la entidad legacy a ClienteDTO
            return ClienteMapeo.MapGestionPedidosToDTO(cliente);
        }
        // Método para dar de alta un cliente en la clase GestiónPedidos
        public async Task<ClienteDTO> AltaCliente(long nroCliente)
        {
            // Llama a ObtenerClienteLegacyById pasándole el nroCliente
            var clienteDTO = await LObtenerClienteById(nroCliente);

            // Llama al caso de uso para dar de alta un nuevo cliente
            ClienteGP clienteCreado = await _CUAltaCliente.Execute(clienteDTO);
            return ClienteMapeo.MapGestionPedidosToDTO(clienteCreado); // Retorna el DTO del cliente creado
        }

        // Método para modificar un cliente en la clase GestiónPedidos
        public async Task<ClienteDTO> ModificarCliente(long nroCliente)
        {
            var clienteDTO = await LObtenerClienteById(nroCliente);
            ClienteGP clienteModificado = await _CUModificarCliente.Execute(clienteDTO);
            return ClienteMapeo.MapGestionPedidosToDTO(clienteModificado); // Retorna el DTO del cliente modificado
        }
        public async Task<ClienteDTO> LModificarCliente(ClienteDTO cliente)
        {
            if (cliente == null)
            {
                throw new ArgumentNullException(nameof(cliente), "El objeto ClienteDTO no puede ser nulo.");
            }
            Cliente clienteLegacy = ClienteMapeo.MapDTOToLegacy(cliente);
            // Llamamos al caso de uso que modifica en Legacy
            Cliente clienteModificado = await _LCUModificarCliente.Execute(clienteLegacy);

            // Retornamos el DTO del cliente modificado
            return ClienteMapeo.MapLegacyToDTO(clienteModificado);
        }
        // Método para dar de baja un cliente en la clase GestiónPedidos
        public async Task BajaCliente(long nroCliente)
        {
           await _CUBajaCliente.Execute(nroCliente); // No es necesario devolver nada en este caso
        }

        public async Task LBajaCliente(long id)
        {
            await _LCUBajaCliente.Execute(id);
        }

        public async Task<bool> ExisteCliente(long id)
        {
            var cliente = await LObtenerClienteById(id);
            return (cliente != null);
        }

        public async Task<Cliente> LAltaCliente(Cliente cliente)
        {
            // Validación básica
            if (cliente == null)
            {
                throw new ArgumentNullException(nameof(cliente), "El cliente no puede ser nulo.");
            }

            await _LCUAltaCliente.Execute(cliente);

            // Retorna la entidad que se acaba de insertar
            return cliente;
        }

    }
}

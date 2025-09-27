using ProyectoDistribuidora.Compartida.DTO.Cliente;
using ProyectoDistribuidora.Compartida.Exception;
using ProyectoDistribuidora.Compartida.Interfaces.IRepositorio;
using ProyectoDistribuidora.Models.Legacy; // Asumiendo que la entidad del Legacy está aquí

namespace ProyectoDistribuidora.CasoDeUso.Cliente
{
    public class LCUModificarCliente
    {
        private readonly LIClienteRepositorio _clienteRepositorio;

        // Constructor con la inyección del repositorio de Legacy
        public LCUModificarCliente(LIClienteRepositorio clienteRepositorio)
        {
            _clienteRepositorio = clienteRepositorio;
        }

        // Método para modificar un cliente en la base Legacy
        public async Task<Models.Legacy.Cliente> Execute(Models.Legacy.Cliente cliente)
        {
            // Validar que el cliente no sea nulo
            if (cliente == null)
            {
                throw new ClienteException("El cliente no puede ser nulo.");
            }

            // Validaciones adicionales sobre los campos requeridos
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
                throw new ClienteException("La dirección principal del cliente no puede estar vacía.");
            }

            if (cliente.Puntos < 0)
            {
                throw new ClienteException("Los puntos del cliente no pueden ser negativos.");
            }

            // Buscar el cliente existente en la base de datos Legacy
            var clienteExistente = await _clienteRepositorio.GetById(cliente.Nro_Cliente);
            if (clienteExistente == null)
            {
                throw new ClienteException($"No se encontró el cliente con NroCliente {cliente.Nro_Cliente} en la base Legacy.");
            }

            // Actualizar las propiedades del cliente según tu modelo Legacy
            clienteExistente.Nombre_Cliente = cliente.Nombre_Cliente;
            clienteExistente.Dir_Cliente = cliente.Dir_Cliente;
            clienteExistente.Dir2_Cliente = cliente.Dir2_Cliente;
            clienteExistente.Telef_Cliente = cliente.Telef_Cliente;
            clienteExistente.Cedula = cliente.Cedula;
            clienteExistente.RUT = cliente.RUT;
            clienteExistente.Puntos = cliente.Puntos;
            clienteExistente.Estado = cliente.Estado;

            // Guardar los cambios en la base de datos Legacy
            await _clienteRepositorio.Update(clienteExistente);

            // Retorna la entidad modificada en Legacy
            return clienteExistente;
        }
    }
}

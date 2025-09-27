using ProyectoDistribuidora.Compartida.DTO.Cliente;
using ProyectoDistribuidora.Compartida.Exception;
using ProyectoDistribuidora.Compartida.Interfaces.IRepositorio;
using ProyectoDistribuidora.Models.GestionPedidos;

namespace ProyectoDistribuidora.CasoDeUso.Cliente
{
    public class CUModificarCliente
    {
        private readonly IClienteRepositorio _clienteRepositorio;

        // Constructor con la inyección del repositorio
        public CUModificarCliente(IClienteRepositorio clienteRepositorio)
        {
            _clienteRepositorio = clienteRepositorio;
        }

        // Método para modificar un cliente
        public async Task<ClienteGP> Execute(ClienteDTO clienteDTO)
        {
            // Validar el DTO
            if (clienteDTO == null)
            {
                throw new ClienteException("El cliente no puede ser nulo.");
            }

            // Validar número de cliente
            if (clienteDTO.NroCliente <= 0)
            {
                throw new ClienteException("El número de cliente debe ser mayor que 0.");
            }

            // Buscar el cliente existente en la base de datos
            var clienteExistente = await _clienteRepositorio.GetById(clienteDTO.NroCliente);
            if (clienteExistente == null)
            {
                throw new ClienteException($"No se encontró el cliente con NroCliente {clienteDTO.NroCliente}.");
            }

            // Validaciones adicionales de negocio
            if (string.IsNullOrWhiteSpace(clienteDTO.NombreCliente))
            {
                throw new ClienteException("El nombre del cliente no puede estar vacío.");
            }

            if (string.IsNullOrWhiteSpace(clienteDTO.DirCliente))
            {
                throw new ClienteException("La dirección principal del cliente no puede estar vacía.");
            }

            if (clienteDTO.Puntos < 0)
            {
                throw new ClienteException("Los puntos del cliente no pueden ser negativos.");
            }

            // Actualizar las propiedades del cliente
            clienteExistente.NombreCliente = clienteDTO.NombreCliente;
            clienteExistente.DirCliente = clienteDTO.DirCliente;
            clienteExistente.Dir2Cliente = clienteDTO.Dir2Cliente;
            clienteExistente.TelefCliente = clienteDTO.TelefCliente;
            clienteExistente.Cedula = clienteDTO.Cedula;
            clienteExistente.RUT = clienteDTO.RUT;
            clienteExistente.Puntos = clienteDTO.Puntos;
            clienteExistente.Estado = clienteDTO.Estado;

            // Guardar los cambios en la base de datos
            await _clienteRepositorio.Update(clienteExistente);

            return clienteExistente; // Retorna la entidad modificada
        }
    }
}

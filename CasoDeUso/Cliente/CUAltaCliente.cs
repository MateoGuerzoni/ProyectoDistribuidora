using ProyectoDistribuidora.Compartida.DTO.Cliente;
using ProyectoDistribuidora.Compartida.Exception;
using ProyectoDistribuidora.Compartida.Interfaces.IRepositorio;
using ProyectoDistribuidora.Models.GestionPedidos;

namespace ProyectoDistribuidora.CasoDeUso.Cliente
{
    public class CUAltaCliente
    {
        private readonly IClienteRepositorio _clienteRepositorio;

        // Constructor para inyectar el repositorio de clientes
        public CUAltaCliente(IClienteRepositorio clienteRepositorio)
        {
            _clienteRepositorio = clienteRepositorio;
        }

        // Método para ejecutar la lógica de alta del cliente
        public async Task<ClienteGP> Execute(ClienteDTO nuevoClienteDTO)
        {
            // Validar que el DTO no sea nulo
            if (nuevoClienteDTO == null)
            {
                throw new ClienteException("El cliente no puede ser nulo.");
            }

            // Validaciones adicionales de los campos requeridos
            if (nuevoClienteDTO.NroCliente <= 0)
            {
                throw new ClienteException("El número de cliente debe ser mayor que 0.");
            }

            if (string.IsNullOrWhiteSpace(nuevoClienteDTO.NombreCliente))
            {
                throw new ClienteException("El nombre del cliente no puede estar vacío.");
            }

            if (string.IsNullOrWhiteSpace(nuevoClienteDTO.DirCliente))
            {
                throw new ClienteException("La dirección del cliente no puede estar vacía.");
            }

            if (nuevoClienteDTO.Puntos < 0)
            {
                throw new ClienteException("Los puntos del cliente no pueden ser negativos.");
            }

            // Mapear el DTO a la entidad ClienteGP
            var nuevoCliente = new ClienteGP
            {
                NroCliente = nuevoClienteDTO.NroCliente,
                NombreCliente = nuevoClienteDTO.NombreCliente,
                DirCliente = nuevoClienteDTO.DirCliente, 
                Dir2Cliente = nuevoClienteDTO.Dir2Cliente,
                TelefCliente = nuevoClienteDTO.TelefCliente,
                Cedula = nuevoClienteDTO.Cedula,
                RUT = nuevoClienteDTO.RUT,
                Puntos = nuevoClienteDTO.Puntos,
                Estado = nuevoClienteDTO.Estado
            };

            // Llamar al repositorio para dar de alta el cliente
            await _clienteRepositorio.Add(nuevoCliente);


            return nuevoCliente; // Retorna la entidad Cliente creada (o el DTO si prefieres)
        }
    }
}

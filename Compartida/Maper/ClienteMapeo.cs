using ProyectoDistribuidora.Compartida.DTO.Cliente;
using ProyectoDistribuidora.Models.GestionPedidos;
using ProyectoDistribuidora.Models.Legacy;

namespace ProyectoDistribuidora.Compartida.Maper
{
    public class ClienteMapeo
    {
        // Mapea la clase legacy Cliente a DTO
        public static ClienteDTO MapLegacyToDTO(Cliente clienteViejo)
        {
            if (clienteViejo == null)
            {
                throw new ArgumentNullException(nameof(clienteViejo), "El cliente no puede ser nulo");
            }

            return new ClienteDTO
            {
                NroCliente = clienteViejo.Nro_Cliente,
                NombreCliente = clienteViejo.Nombre_Cliente,
                DirCliente = clienteViejo.Dir_Cliente,
                Dir2Cliente = clienteViejo.Dir2_Cliente,
                TelefCliente = clienteViejo.Telef_Cliente,
                Cedula = clienteViejo.Cedula,
                RUT = clienteViejo.RUT,
                Puntos = clienteViejo.Puntos,
                Estado = clienteViejo.Estado
            };
        }
        public static Cliente MapDTOToLegacy(ClienteDTO clienteDTO)
        {
            if (clienteDTO == null)
            {
                throw new ArgumentNullException(nameof(clienteDTO), "El cliente no puede ser nulo");
            }

            return new Cliente
            {
                Nro_Cliente = clienteDTO.NroCliente,
                Nombre_Cliente = clienteDTO.NombreCliente,
                Dir_Cliente = clienteDTO.DirCliente,
                Dir2_Cliente = clienteDTO.Dir2Cliente,
                Telef_Cliente = clienteDTO.TelefCliente,
                Cedula = clienteDTO.Cedula,
                RUT = clienteDTO.RUT,
                Puntos = clienteDTO.Puntos,
                Estado = clienteDTO.Estado
            };
        }
        // Mapea la clase GP Cliente a DTO
        public static ClienteDTO MapGestionPedidosToDTO(ClienteGP clienteNuevo)
        {
            if (clienteNuevo == null)
            {
                throw new ArgumentNullException(nameof(clienteNuevo), "El cliente no puede ser nulo");
            }

            return new ClienteDTO
            {
                NroCliente = clienteNuevo.NroCliente,
                NombreCliente = clienteNuevo.NombreCliente,
                DirCliente = clienteNuevo.DirCliente,
                Dir2Cliente = clienteNuevo.Dir2Cliente,
                TelefCliente = clienteNuevo.TelefCliente,
                Cedula = clienteNuevo.Cedula,
                RUT = clienteNuevo.RUT,
                Puntos = clienteNuevo.Puntos,
                Estado = clienteNuevo.Estado
            };
        }
    }
}

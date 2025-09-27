using ProyectoDistribuidora.Compartida.Exception;
using ProyectoDistribuidora.Compartida.Interfaces.IRepositorio;
using ProyectoDistribuidora.Models.GestionPedidos;
using ProyectoDistribuidora.Models.Legacy;

namespace ProyectoDistribuidora.CasoDeUso.Cliente
{
    public class CUObtenerClientes
    {
        private readonly IClienteRepositorio _clienteRepositorio;

        // Constructor para inyectar el repositorio de clientes
        public CUObtenerClientes(IClienteRepositorio clienteRepositorio)
        {
            _clienteRepositorio = clienteRepositorio;
        }

        // Método para obtener todos los clientes
        public async Task<IEnumerable<ClienteGP>> Execute()
        {
            // Obtener todos los clientes desde el repositorio
            var clientes = await _clienteRepositorio.GetAll();

            // Validar si no hay clientes en la base de datos
            if (clientes == null || !clientes.Any())
            {
                throw new ClienteException("No se encontraron clientes en la base de datos.");
            }

            return clientes; // Retornar la lista de clientes obtenida
        }
    }
}
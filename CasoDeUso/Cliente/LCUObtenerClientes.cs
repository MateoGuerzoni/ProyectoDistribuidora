using ProyectoDistribuidora.Compartida.Exception;
using ProyectoDistribuidora.Compartida.Interfaces.IRepositorio;
using ProyectoDistribuidora.Models.Legacy;

namespace ProyectoDistribuidora.CasoDeUso.Cliente
{
    public class LCUObtenerClientes
    {
        private readonly LIClienteRepositorio _clienteRepositorio;

        // Constructor con la inyección del repositorio
        public LCUObtenerClientes(LIClienteRepositorio clienteRepositorio)
        {
            _clienteRepositorio = clienteRepositorio;
        }

        // Método para obtener todos los clientes
        public async Task<IEnumerable<Models.Legacy.Cliente>> Execute()
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

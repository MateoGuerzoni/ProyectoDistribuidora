using ProyectoDistribuidora.Compartida.Exception;
using ProyectoDistribuidora.Compartida.Interfaces.IRepositorio;
using ProyectoDistribuidora.Models.GestionPedidos;

namespace ProyectoDistribuidora.CasoDeUso.Cliente
{
    public class CUObtenerClienteById
    {
        private readonly IClienteRepositorio _clienteRepositorio;

        // Constructor para inyectar el repositorio de clientes
        public CUObtenerClienteById(IClienteRepositorio clienteRepositorio)
        {
            _clienteRepositorio = clienteRepositorio;
        }

        // Método para ejecutar la lógica de obtener un cliente por ID
        public async Task<ClienteGP> Execute(long idCliente)
        {
            // Validar que el ID del cliente sea mayor que 0
            if (idCliente <= 0)
            {
                throw new ClienteException("El ID del cliente debe ser mayor que 0.");
            }

            // Obtener el cliente desde el repositorio
            var cliente = await _clienteRepositorio.GetById(idCliente);

            // Si no se encuentra el cliente, lanzar una excepción
            if (cliente == null)
            {
                throw new ClienteException($"No se encontró un cliente con el ID {idCliente}.");
            }

            return cliente; // Retornar la entidad obtenida
        }
    }
}
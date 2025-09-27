using ProyectoDistribuidora.Compartida.Exception;
using ProyectoDistribuidora.Compartida.Interfaces.IRepositorio;

namespace ProyectoDistribuidora.CasoDeUso.Cliente
{
    public class CUBajaCliente
    {
        private readonly IClienteRepositorio _clienteRepositorio;

        public CUBajaCliente(IClienteRepositorio clienteRepositorio)
        {
            _clienteRepositorio = clienteRepositorio;
        }

        public async Task Execute(long nroCliente)
        {
            // Validar el Id del cliente
            if (nroCliente <= 0)
            {
                throw new ClienteException("El número de cliente debe ser mayor que 0.");
            }

            // Buscar el cliente existente en la base de datos
            var clienteExistente = await _clienteRepositorio.GetById(nroCliente);

            if (clienteExistente == null)
            {
                throw new ClienteException($"No se encontró el cliente con NroCliente {nroCliente}.");
            }

            // Llama al repositorio para eliminar el cliente
            await _clienteRepositorio.Delete(clienteExistente);
        }
    }
}

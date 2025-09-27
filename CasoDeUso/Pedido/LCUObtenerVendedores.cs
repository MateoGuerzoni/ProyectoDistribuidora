using ProyectoDistribuidora.Compartida.Exception;
using ProyectoDistribuidora.Compartida.Interfaces.IRepositorio;
using ProyectoDistribuidora.Models.Legacy;

namespace ProyectoDistribuidora.CasoDeUso.Pedido
{
    public class LCUObtenerVendedores
    {

        private readonly LIPedidoRepositorio _pedidosRepositorio;
        private readonly ILogger<LCUObtenerVendedores> _logger;

        public LCUObtenerVendedores(LIPedidoRepositorio pedidoRepositorio, ILogger<LCUObtenerVendedores> logger)
        {
            _pedidosRepositorio = pedidoRepositorio ?? throw new ArgumentNullException(nameof(pedidoRepositorio));
            _logger = logger;
        }

        public async Task<IEnumerable<Vendedor>> Execute()
        {
            try
            {
                IEnumerable<Vendedor> vendedores = await _pedidosRepositorio.GetVendedores();

                if (!vendedores.Any())
                {
                    throw VendedorException.NoEncontrado(0); // ID 0 indica búsqueda de todos los vendedores
                }

                return vendedores;
            }
            catch (VendedorException)
            {
                throw;  // Relanza la excepción específica sin modificarla
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado en la lógica de negocio al obtener los vendedores.");
                throw new VendedorException("Error inesperado al procesar los vendedores.", ex);
            }
        }
    }
}

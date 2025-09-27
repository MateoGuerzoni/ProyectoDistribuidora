using ProyectoDistribuidora.Compartida.Exception;
using ProyectoDistribuidora.Compartida.Interfaces.IRepositorio;
using ProyectoDistribuidora.Models.GestionPedidos;
using ProyectoDistribuidora.Repositorios.GestionPedidos;

public class CUObtenerPedidos
{
    private readonly IPedidoRepositorio _pedidosRepositorio;
    private readonly ILogger<CUObtenerPedidos> _logger;

    public CUObtenerPedidos(IPedidoRepositorio pedidosRepositorio, ILogger<CUObtenerPedidos> logger)
    {
        _pedidosRepositorio = pedidosRepositorio ?? throw new ArgumentNullException(nameof(pedidosRepositorio));
        _logger = logger;
    }

    public async Task<IEnumerable<PedidoGP>> Execute()
    {
        try
        {
            var pedidos = await _pedidosRepositorio.GetAll();

            if (!pedidos.Any())
            {
                throw PedidoException.NoEncontrado(0); // ID 0 indica búsqueda de todos los pedidos
            }

            return pedidos;
        }
        catch (PedidoException)
        {
            throw;  // Relanza la excepción específica sin modificarla
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error inesperado en la lógica de negocio al obtener los pedidos.");
            throw new PedidoException("Error inesperado al procesar los pedidos.", ex);
        }
    }
}

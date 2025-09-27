using ProyectoDistribuidora.Compartida.Exception;
using ProyectoDistribuidora.Compartida.Interfaces.IRepositorio;
using ProyectoDistribuidora.Models.Legacy;

public class LCUInsertarLineaPedido
{
    private readonly LIPedidoRepositorio _pedidoRepositorio;

    public LCUInsertarLineaPedido(LIPedidoRepositorio pedidoRepositorio)
    {
        _pedidoRepositorio = pedidoRepositorio ?? throw new ArgumentNullException(nameof(pedidoRepositorio));
    }

    public async Task Execute(int idPedido, LineaPedido nuevaLinea)
    {
        // Validar el ID del pedido
        if (idPedido <= 0)
        {
            throw new PedidoException("El ID del pedido debe ser mayor que 0.");
        }

        // Validar que la línea de pedido no sea nula
        if (nuevaLinea == null)
        {
            throw new PedidoException("La línea de pedido no puede ser nula.");
        }

        // Validar los datos de la línea de pedido
        if (string.IsNullOrWhiteSpace(nuevaLinea.Codigo))
        {
            throw new PedidoException("El código del producto no puede estar vacío.");
        }

        if (nuevaLinea.Cantidad <= 0)
        {
            throw new PedidoException("La cantidad de la línea de pedido debe ser mayor a cero.");
        }

        if (nuevaLinea.PrecioUnitario <= 0)
        {
            throw new PedidoException("El precio unitario debe ser mayor a cero.");
        }

        if (nuevaLinea.Subtotal != nuevaLinea.Cantidad * nuevaLinea.PrecioUnitario)
        {
            throw new PedidoException("El subtotal de la línea de pedido no es correcto.");
        }

        await _pedidoRepositorio.InsertarLineaPedido(idPedido, nuevaLinea);
    }
}

namespace ProyectoDistribuidora.Compartida.DTO
{
    public class LineaPedidoDTO
    {
        public int IdPedido { get; set; }
        public int IdLineaPedido { get; set; }
        public string Codigo { get; set; } = string.Empty;
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal? Subtotal { get; set; }
    }
}

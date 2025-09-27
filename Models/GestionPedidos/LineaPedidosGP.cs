namespace ProyectoDistribuidora.Models.GestionPedidos
{
    public class LineaPedidosGP
    {
        public int IdPedido { get; set; } // Clave primaria compuesta (parte 1)
        public int IdLineaPedido { get; set; } // Clave primaria compuesta (parte 2)
        public string Codigo { get; set; } // Código del producto (máximo 18 caracteres)
        public int Cantidad { get; set; } // Cantidad del producto
        public decimal PrecioUnitario { get; set; } // Precio por unidad del producto
        public decimal? Subtotal { get; set; } // Subtotal (opcional, nullable)
    }
}


namespace ProyectoDistribuidora.Models.GestionPedidos
{
    public class PedidoGP
    {
        public int IdPedido { get; set; }
        public long? IdVendedor { get; set; } // Campo nullable
        public long IdCliente { get; set; }
        public long IdAdministracion { get; set; }
        public long? IdSupervisor { get; set; } // Campo nullable
        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaEntrega { get; set; } // Campo nullable
        public DateTime? FechaEntregado { get; set; } // Campo nullable
        public string Estado { get; set; } // Máximo de 20 caracteres
        public int? IdContacto { get; set; } // Campo nullable
        public string? Direccion { get; set; }
        public string MetodoPago { get; set; } // Máximo de 50 caracteres
        public decimal Total { get; set; }
        public string Comentarios { get; set; } // Campo nullable
    }
}

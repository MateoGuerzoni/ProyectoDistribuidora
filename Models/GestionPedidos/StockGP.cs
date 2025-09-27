namespace ProyectoDistribuidora.Models.GestionPedidos
{
    public class StockGP
    {
        public string Codigo { get; set; } // Máximo de 50 caracteres
        public string Descripcion { get; set; } // Máximo de 255 caracteres
        public string Marca { get; set; } // Máximo de 100 caracteres
        public double StockActual { get; set; }
        public double StockMinimo { get; set; }
        public string Estante { get; set; } // Máximo de 50 caracteres
        public string Procedencia { get; set; } // Máximo de 100 caracteres
        public byte Clase { get; set; }
        public byte Moneda { get; set; }
        public byte MonLista { get; set; }
        public byte Tasa { get; set; }
        public double Stock2 { get; set; }
        public double Stock3 { get; set; }
        public double Stock4 { get; set; }
        public double Stock5 { get; set; }
        public double? Precio { get; set; } // Campo nullable
        public double? Costo { get; set; } // Campo nullable
        public string? Detalle { get; set; } // Campo nullable
        public string? CodBarra { get; set; } // Campo nullable
        public double? Porcentaje { get; set; } // Campo nullable
        public long? Similar { get; set; } // Campo nullable
        public DateTime? FCompra { get; set; } // Campo nullable
        public byte? Estado { get; set; } // Campo nullable
    }
}

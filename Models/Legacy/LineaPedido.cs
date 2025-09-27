using Microsoft.EntityFrameworkCore;

namespace ProyectoDistribuidora.Models.Legacy
{


    public class LineaPedido
    {
        public int IdPedido { get; set; } // Parte de la clave primaria compuesta
        public int IdLineaPedido { get; set; } // Parte de la clave primaria compuesta
        public string Codigo { get; set; } // Código del producto
        public int Cantidad { get; set; } // Cantidad de productos
        public decimal PrecioUnitario { get; set; } // Precio por unidad
        public decimal Subtotal { get; set; } // Calculado en la base de datos

    }
}
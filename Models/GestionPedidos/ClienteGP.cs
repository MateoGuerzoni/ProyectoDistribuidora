using System.ComponentModel.DataAnnotations;

namespace ProyectoDistribuidora.Models.GestionPedidos
{
    public class ClienteGP
    {
        public long NroCliente { get; set; }
        public string NombreCliente { get; set; } // Máximo de 255 caracteres
        public string DirCliente { get; set; } 
        public string Dir2Cliente { get; set; } 
        public string TelefCliente { get; set; } 
        public string? Cedula { get; set; }
        public string? RUT { get; set; }
        public double? Puntos { get; set; }
        public byte Estado { get; set; }
    }
}

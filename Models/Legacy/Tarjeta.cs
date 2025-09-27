using System;
using System.ComponentModel.DataAnnotations;

namespace ProyectoDistribuidora.Models.Legacy
{
    public class Tarjeta
    {
        [Key]
        public int ID { get; set; }
        public string? Nombre_Tarjeta { get; set; }
        public string? Coeficiente { get; set; }
    }
}

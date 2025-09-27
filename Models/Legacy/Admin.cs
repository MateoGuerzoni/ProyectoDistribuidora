using System;
using System.ComponentModel.DataAnnotations;
namespace ProyectoDistribuidora.Models.Legacy
{
    public class Admin
    {
        [Key]
        public int ID { get; set; } // Corresponde a la columna ID (int, NOT NULL)

        [MaxLength(20)]
        public string Usuario { get; set; } // Corresponde a Usuario (char(20), NULL)

        [MaxLength(10)]
        public string Clave { get; set; } // Corresponde a Clave (char(10), NULL)

        public byte? Nivel { get; set; } // Corresponde a Nivel (tinyint, NULL)

        public float? Dato { get; set; } // Corresponde a Dato (float, NULL)

        [MaxLength(50)]
        public string Descripcion { get; set; } // Corresponde a Descripcion (char(50), NULL)

        public byte? Respaldo { get; set; } // Corresponde a Respaldo (tinyint, NULL)

        public int? Dato2 { get; set; } // Corresponde a Dato2 (int, NULL)

        [MaxLength(250)]
        public string Texto { get; set; } // Corresponde a Texto (char(250), NULL)

        [MaxLength(20)]
        public string Texto2 { get; set; } // Corresponde a Texto2 (char(20), NULL)

        public byte? Nro_Funcionario { get; set; } // Corresponde a Nro_Funcionario (tinyint, NULL)
    }
}

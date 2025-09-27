using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ProyectoDistribuidora.Models.Legacy
{
    public class Usuario
    {
        public long IdUsuario { get; set; }

        [MaxLength(50)]
        public string Rol { get; set; }

        [Required]
        [MaxLength(50)]
        public string NombreUsuario { get; set; }

        [Required]
        [MaxLength(256)]
        public string Password { get; set; }
    }
}

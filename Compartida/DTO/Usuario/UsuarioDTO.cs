using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoDistribuidora.Compartida.DTO.Usuario
{
    public class UsuarioDTO
    {
        [Column(Order = 1)] // Define el orden de la clave compuesta
        public long IdUsuario { get; set; }

        [Column(Order = 2)] // Define el orden de la clave compuesta
        public string Rol { get; set; }

        public string NombreUsuario { get; set; }

        public string Password { get; set; }
    }
}

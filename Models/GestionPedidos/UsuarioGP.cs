using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoDistribuidora.Models.GestionPedidos
{
    public class UsuarioGP
    {
        public long IdUsuario { get; set; }
        public string Rol { get; set; }
        public string NombreUsuario { get; set; }
        public string Password { get; set; }
    }
}

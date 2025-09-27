using System;

namespace ProyectoDistribuidora.Compartida.Exception
{
    public class UsuarioException : System.Exception
    {
        public UsuarioException()
            : base("Error relacionado con la entidad Usuario.") { }

        public UsuarioException(string mensaje)
            : base(mensaje) { }

        public UsuarioException(string mensaje, System.Exception inner)
            : base(mensaje, inner) { }

        public static UsuarioException NoEncontrado(long userId) =>
            new UsuarioException($"No se encontró el Usuario con ID={userId}.");

        public static UsuarioException Duplicado(string username) =>
            new UsuarioException($"Ya existe un usuario con el nombre '{username}'.");
    }
}

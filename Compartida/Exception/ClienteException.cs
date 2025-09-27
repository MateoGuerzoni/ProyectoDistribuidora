using System;

namespace ProyectoDistribuidora.Compartida.Exception
{
    public class ClienteException : System.Exception
    {
        // Constructor genérico
        public ClienteException()
            : base("Error relacionado con la entidad Cliente.") { }

        // Constructor con mensaje personalizado
        public ClienteException(string mensaje)
            : base(mensaje) { }

        // Constructor con inner exception
        public ClienteException(string mensaje, System.Exception inner)
            : base(mensaje, inner) { }

        // Métodos estáticos de ayuda para distintos casos:
        public static ClienteException NoEncontrado(long clienteId) =>
            new ClienteException($"No se encontró el Cliente con ID={clienteId}.");

        public static ClienteException Duplicado(string nombre) =>
            new ClienteException($"Ya existe un Cliente con el nombre '{nombre}'.");

        public static ClienteException IdInvalido(long clienteId) =>
            new ClienteException($"El ID de Cliente '{clienteId}' no es válido.");
    }
}

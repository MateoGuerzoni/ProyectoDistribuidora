using System;

namespace ProyectoDistribuidora.Compartida.Exception
{
    public class LogException : System.Exception
    {
        public LogException()
            : base("Error relacionado con la entidad Log.") { }

        public LogException(string mensaje)
            : base(mensaje) { }

        public LogException(string mensaje, System.Exception inner)
            : base(mensaje, inner) { }

        public static LogException NoEncontrado(long logId) =>
            new LogException($"No se encontró el Log con ID={logId}.");

        public static LogException OperacionInvalida(long logId, string operacion) =>
            new LogException($"La operación '{operacion}' no es válida para el Log con ID={logId}.");
    }
}

using System;

namespace ProyectoDistribuidora.Compartida.Exception
{
    public class SupervisorDeCargaException : System.Exception
    {
        public SupervisorDeCargaException()
            : base("Error relacionado con la entidad SupervisorDeCarga.") { }

        public SupervisorDeCargaException(string mensaje)
            : base(mensaje) { }

        public SupervisorDeCargaException(string mensaje, System.Exception inner)
            : base(mensaje, inner) { }

        public static SupervisorDeCargaException NoEncontrado(long id) =>
            new SupervisorDeCargaException($"No se encontró el SupervisorDeCarga con ID={id}.");
    }
}

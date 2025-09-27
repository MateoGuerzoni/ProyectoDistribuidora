using System;

namespace ProyectoDistribuidora.Compartida.Exception
{
    public class TarjetaException : System.Exception
    {
        public TarjetaException()
            : base("Error relacionado con la entidad Tarjeta.") { }

        public TarjetaException(string mensaje)
            : base(mensaje) { }

        public TarjetaException(string mensaje, System.Exception inner)
            : base(mensaje, inner) { }

        public static TarjetaException NoEncontrada(long tarjetaId) =>
            new TarjetaException($"No se encontró la Tarjeta con ID={tarjetaId}.");

        public static TarjetaException DatosInvalidos(string detalle) =>
            new TarjetaException($"La Tarjeta es inválida. Detalle: {detalle}.");
    }
}

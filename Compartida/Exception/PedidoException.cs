using System;

namespace ProyectoDistribuidora.Compartida.Exception
{
    public class PedidoException : System.Exception
    {
        public PedidoException()
            : base("Error relacionado con la entidad Pedido.") { }

        public PedidoException(string mensaje)
            : base(mensaje) { }

        public PedidoException(string mensaje, System.Exception inner)
            : base(mensaje, inner) { }

        public static PedidoException NoEncontrado(long pedidoId) =>
            new PedidoException($"No se encontró el Pedido con ID={pedidoId}.");

        public static PedidoException Duplicado(long pedidoId) =>
            new PedidoException($"Ya existe un Pedido con ID={pedidoId}.");

        public static PedidoException OperacionInvalida(string detalle) =>
            new PedidoException($"La operación sobre el Pedido no es válida: {detalle}.");
    }
}

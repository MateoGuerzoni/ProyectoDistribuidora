using System;

namespace ProyectoDistribuidora.Compartida.Exception
{
    public class LineaPedidoException : System.Exception
    {
        public LineaPedidoException()
            : base("Error relacionado con la entidad Línea de Pedido.") { }

        public LineaPedidoException(string mensaje)
            : base(mensaje) { }

        public LineaPedidoException(string mensaje, System.Exception inner)
            : base(mensaje, inner) { }

        public static LineaPedidoException NoEncontrada(long idPedido, long idLinea) =>
            new LineaPedidoException($"No se encontró la línea {idLinea} del pedido {idPedido}.");

        public static LineaPedidoException DatosInvalidos(string detalle) =>
            new LineaPedidoException($"Los datos de la línea de pedido no son válidos. Detalle: {detalle}.");
    }
}

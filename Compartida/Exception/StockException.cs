using System;

namespace ProyectoDistribuidora.Compartida.Exception
{
    public class StockException : System.Exception
    {
        public StockException()
            : base("Error relacionado con la entidad Stock.") { }

        public StockException(string mensaje)
            : base(mensaje) { }

        public StockException(string mensaje, System.Exception inner)
            : base(mensaje, inner) { }

        public static StockException NoEncontrado(long stockId) =>
            new StockException($"No se encontró el Stock con ID={stockId}.");

        public static StockException SinSuficienteStock(long stockId, int cantidadSolicitada) =>
            new StockException($"El Stock con ID={stockId} es insuficiente para la cantidad solicitada ({cantidadSolicitada}).");
    }
}

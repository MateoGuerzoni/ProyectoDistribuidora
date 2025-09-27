using System;

namespace ProyectoDistribuidora.Compartida.Exception
{
    public class VendedorException : System.Exception
    {
        public VendedorException()
            : base("Error relacionado con la entidad Vendedor.") { }

        public VendedorException(string mensaje)
            : base(mensaje) { }

        public VendedorException(string mensaje, System.Exception inner)
            : base(mensaje, inner) { }

        public static VendedorException NoEncontrado(long nroVendedor) =>
            new VendedorException($"No se encontró el Vendedor con Nro={nroVendedor}.");

        public static VendedorException Duplicado(long nroVendedor) =>
            new VendedorException($"Ya existe un Vendedor con Nro={nroVendedor}.");

        public static VendedorException IdInvalido(long nroVendedor) =>
            new VendedorException($"El número de Vendedor '{nroVendedor}' no es válido.");
    }
}

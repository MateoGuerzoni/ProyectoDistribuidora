using Microsoft.Extensions.Hosting;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using Mono.TextTemplating;
using ProyectoDistribuidora.Compartida.DTO.Stock;
using ProyectoDistribuidora.Compartida.Exception;
using ProyectoDistribuidora.Compartida.Interfaces.IRepositorio;
using ProyectoDistribuidora.Models.GestionPedidos;
using System.Threading.Tasks;

namespace ProyectoDistribuidora.CasoDeUso.Stock
{
    public class CUModificarStock
    {
        private readonly IStockRepositorio _stockRepositorio;

        // Constructor con la inyección del repositorio
        public CUModificarStock(IStockRepositorio stockRepositorio)
        {
            _stockRepositorio = stockRepositorio;
        }

        // Método para modificar un registro de stock
        public async Task<StockGP> Execute(StockDTO stockDTO)
        {
            // Validar el DTO
            if (stockDTO == null)
            {
                throw new StockException("El stock no puede ser nulo.");
            }

            // Validar las propiedades requeridas del DTO
            if (string.IsNullOrWhiteSpace(stockDTO.Codigo))
            {
                throw new StockException("El código del stock no puede ser nulo o vacío.");
            }

            if (string.IsNullOrWhiteSpace(stockDTO.Descripcion))
            {
                throw new StockException("La descripción del stock no puede ser nula o vacía.");
            }

            // Buscar el stock existente en la base de datos
            var stockExistente = await _stockRepositorio.GetById(stockDTO.Codigo);
            if (stockExistente == null)
            {
                throw new StockException($"El stock con código {stockDTO.Codigo} no existe.");
            }

            stockExistente.Descripcion = stockDTO.Descripcion;
            stockExistente.Marca = stockDTO.Marca;
            stockExistente.StockActual = stockDTO.Stock_Actual;
            stockExistente.StockMinimo = stockDTO.Stock_Minimo;
            stockExistente.Estante = stockDTO.Estante;
            stockExistente.Procedencia = stockDTO.Procedencia;
            stockExistente.Clase = stockDTO.Clase;
            stockExistente.Moneda = stockDTO.Moneda;
            stockExistente.MonLista = stockDTO.MonLista;
            stockExistente.Tasa = stockDTO.Tasa;
            stockExistente.Stock2 = stockDTO.Stock_2;
            stockExistente.Stock3 = stockDTO.Stock_3;
            stockExistente.Stock4 = stockDTO.Stock_4;
            stockExistente.Stock5 = stockDTO.Stock_5;
            stockExistente.Precio = stockDTO.Precio;
            stockExistente.Costo = stockDTO.Costo;
            stockExistente.Detalle = stockDTO.Detalle;
            stockExistente.CodBarra = stockDTO.CodBarra;
            stockExistente.Porcentaje = stockDTO.Porcentaje;
            stockExistente.Similar = stockDTO.Similar;
            stockExistente.FCompra = stockDTO.F_Compra;
            stockExistente.Estado = stockDTO.Estado;

            // Guardar los cambios en la base de datos
            await _stockRepositorio.Update(stockExistente);

            return stockExistente; // Retorna la entidad modificada
        }
    }

}

using ProyectoDistribuidora.Compartida.DTO.Stock;
using ProyectoDistribuidora.Compartida.Exception;
using ProyectoDistribuidora.Compartida.Interfaces.IRepositorio;
using ProyectoDistribuidora.Models.GestionPedidos;

namespace ProyectoDistribuidora.CasoDeUso.Stock
{
    public class CUAltaStock
    {
        private readonly IStockRepositorio _stockRepositorio;

        // Constructor para inyectar el repositorio de stock
        public CUAltaStock(IStockRepositorio stockRepositorio)
        {
            _stockRepositorio = stockRepositorio;
        }

        // Método para ejecutar la lógica de alta del stock
        public async Task<StockGP> Execute(StockDTO nuevoStockDTO)
        {
            // Validar el DTO
            if (nuevoStockDTO == null)
            {
                throw new StockException("El stock no puede ser nulo.");
            }

            // Validar las propiedades requeridas del DTO
            if (string.IsNullOrWhiteSpace(nuevoStockDTO.Codigo))
            {
                throw new StockException("El código del stock no puede ser nulo o vacío.");
            }

            if (string.IsNullOrWhiteSpace(nuevoStockDTO.Descripcion))
            {
                throw new StockException("La descripción del stock no puede ser nula o vacía.");
            }

            // Mapear el DTO a la entidad StockGP
            var nuevoStock = new StockGP
            {
                Codigo = nuevoStockDTO.Codigo,
                Descripcion = nuevoStockDTO.Descripcion,
                Marca = nuevoStockDTO.Marca,
                StockActual = nuevoStockDTO.Stock_Actual,
                StockMinimo = nuevoStockDTO.Stock_Minimo,
                Estante = nuevoStockDTO.Estante,
                Procedencia = nuevoStockDTO.Procedencia,
                Clase = nuevoStockDTO.Clase,
                Moneda = nuevoStockDTO.Moneda,
                MonLista = nuevoStockDTO.MonLista,
                Tasa = nuevoStockDTO.Tasa,
                Stock2 = nuevoStockDTO.Stock_2,
                Stock3 = nuevoStockDTO.Stock_3,
                Stock4 = nuevoStockDTO.Stock_4,
                Stock5 = nuevoStockDTO.Stock_5,
                Precio = nuevoStockDTO.Precio,
                Costo = nuevoStockDTO.Costo,
                Detalle = nuevoStockDTO.Detalle,
                CodBarra = nuevoStockDTO.CodBarra,
                Porcentaje = nuevoStockDTO.Porcentaje,
                Similar = nuevoStockDTO.Similar,
                FCompra = nuevoStockDTO.F_Compra,
                Estado = nuevoStockDTO.Estado
            };

            // Llamar al repositorio para dar de alta el stock
            await _stockRepositorio.Add(nuevoStock);

            return nuevoStock; // Retorna la entidad Stock creada
        }
    }

}

using ProyectoDistribuidora.Compartida.DTO.Stock;
using ProyectoDistribuidora.Models.GestionPedidos;
using ProyectoDistribuidora.Models.Legacy;

namespace ProyectoDistribuidora.Compartida.Maper
{
    public class StockMapeo
    {
        // Mapea la clase legacy Stock a DTO
        public static StockDTO MapLegacyToDTO(Stock stockViejo)
        {
            if (stockViejo == null)
            {
                throw new ArgumentNullException(nameof(stockViejo), "El stock no puede ser nulo");
            }

            return new StockDTO
            {
                Codigo = stockViejo.Codigo,
                Descripcion = stockViejo.Descripcion,
                Marca = stockViejo.Marca,
                Stock_Actual = stockViejo.Stock_Actual,
                Stock_Minimo = stockViejo.Stock_Minimo,
                Estante = stockViejo.Estante,
                Procedencia = stockViejo.Procedencia,
                Clase = stockViejo.Clase,
                Moneda = stockViejo.Moneda,
                MonLista = stockViejo.MonLista,
                Tasa = stockViejo.Tasa,
                Stock_2 = stockViejo.Stock_2,
                Stock_3 = stockViejo.Stock_3,
                Stock_4 = stockViejo.Stock_4,
                Stock_5 = stockViejo.Stock_5,
                Precio = stockViejo.Precio,
                Costo = stockViejo.Costo,
                Detalle = stockViejo.Detalle,
                CodBarra = stockViejo.CodBarra,
                Porcentaje = stockViejo.Porcentaje,
                Similar = stockViejo.Similar,
                F_Compra = stockViejo.F_Compra,
                Estado = stockViejo.Estado
            };
        }
        public static Stock MapDTOToLegacy(StockDTO stockDTO)
        {
            if (stockDTO == null)
            {
                throw new ArgumentNullException(nameof(stockDTO), "El stock no puede ser nulo");
            }

            return new Stock
            {
                Codigo = stockDTO.Codigo,
                Descripcion = stockDTO.Descripcion,
                Marca = stockDTO.Marca,
                Stock_Actual = stockDTO.Stock_Actual,
                Stock_Minimo = stockDTO.Stock_Minimo,
                Estante = stockDTO.Estante,
                Procedencia = stockDTO.Procedencia,
                Clase = stockDTO.Clase,
                Moneda = stockDTO.Moneda,
                MonLista = stockDTO.MonLista,
                Tasa = stockDTO.Tasa,
                Stock_2 = stockDTO.Stock_2,
                Stock_3 = stockDTO.Stock_3,
                Stock_4 = stockDTO.Stock_4,
                Stock_5 = stockDTO.Stock_5,
                Precio = stockDTO.Precio,
                Costo = stockDTO.Costo,
                Detalle = stockDTO.Detalle,
                CodBarra = stockDTO.CodBarra,
                Porcentaje = stockDTO.Porcentaje,
                Similar = stockDTO.Similar,
                F_Compra = stockDTO.F_Compra,
                Estado = stockDTO.Estado
            };
        }

        // Mapea la clase GP Stock a DTO
        public static StockDTO MapGestionPedidosToDTO(StockGP stockNuevo)
        {
            if (stockNuevo == null)
            {
                throw new ArgumentNullException(nameof(stockNuevo), "El stock no puede ser nulo");
            }

            return new StockDTO
            {
                Codigo = stockNuevo.Codigo,
                Descripcion = stockNuevo.Descripcion,
                Marca = stockNuevo.Marca,
                Stock_Actual = stockNuevo.StockActual,
                Stock_Minimo = stockNuevo.StockMinimo,
                Estante = stockNuevo.Estante,
                Procedencia = stockNuevo.Procedencia,
                Clase = stockNuevo.Clase,
                Moneda = stockNuevo.Moneda,
                MonLista = stockNuevo.MonLista,
                Tasa = stockNuevo.Tasa,
                Stock_2 = stockNuevo.Stock2,
                Stock_3 = stockNuevo.Stock3,
                Stock_4 = stockNuevo.Stock4,
                Stock_5 = stockNuevo.Stock5,
                Precio = stockNuevo.Precio,
                Costo = stockNuevo.Costo,
                Detalle = stockNuevo.Detalle,
                CodBarra = stockNuevo.CodBarra,
                Porcentaje = stockNuevo.Porcentaje,
                Similar = stockNuevo.Similar,
                F_Compra = stockNuevo.FCompra,
                Estado = stockNuevo.Estado
            };
        }
    }

}

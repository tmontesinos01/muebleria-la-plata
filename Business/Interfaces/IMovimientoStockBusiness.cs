using Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface IMovimientoStockBusiness
    {
        Task<string> RegistrarMovimiento(MovimientoStock movimiento);
        Task<IEnumerable<MovimientoStock>> ObtenerMovimientosPorProducto(string idProducto);
    }
}

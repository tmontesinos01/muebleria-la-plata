using Data.Interfaces;
using Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface IVentaDetalleBusiness : IRepository<VentaDetalle>
    {
        Task<IEnumerable<VentaDetalle>> ObtenerDetallesPorVenta(string ventaId);
    }
}

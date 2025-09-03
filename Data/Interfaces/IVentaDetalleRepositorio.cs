using Entities;
using Data.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface IVentaDetalleRepositorio : IRepositorio<VentaDetalle>
    {
        Task<IEnumerable<VentaDetalle>> ObtenerDetallesPorVentaId(string ventaId);
    }
}

using Data.Interfaces;
using Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface IFacturaBusiness : IRepository<Factura>
    {
        Task<IEnumerable<Factura>> GetFacturasPorVenta(string ventaId);
        Task<IEnumerable<Factura>> GetFacturasPorEstado(string estado);
        Task<bool> ExisteFacturaParaVenta(string ventaId);
    }
}

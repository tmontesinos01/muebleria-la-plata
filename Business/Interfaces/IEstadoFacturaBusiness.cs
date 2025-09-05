using Data.Interfaces;
using Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface IEstadoFacturaBusiness : IRepository<EstadoFactura>
    {
        Task<EstadoFactura> GetByCodigo(string codigo);
        Task<IEnumerable<EstadoFactura>> GetEstadosQuePermitenAnulacion();
        Task<IEnumerable<EstadoFactura>> GetEstadosFinales();
    }
}

using Data.Interfaces;
using Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Business.Services
{
    public class VentaDetalleBusiness
    {
        private readonly IVentaDetalleRepositorio _ventaDetalleRepo;

        public VentaDetalleBusiness(IVentaDetalleRepositorio ventaDetalleRepo)
        {
            _ventaDetalleRepo = ventaDetalleRepo;
        }

        public async Task<IEnumerable<VentaDetalle>> ObtenerDetallesPorVenta(string ventaId)
        {
            return await _ventaDetalleRepo.ObtenerDetallesPorVentaId(ventaId);
        }
    }
}

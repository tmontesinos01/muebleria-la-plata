using Data.Interfaces;
using Entities;
using System.Threading.Tasks;

namespace Business.Services
{
    public class VentaBusiness
    {
        private readonly IVentaRepositorio _ventaRepo;
        private readonly IVentaDetalleRepositorio _ventaDetalleRepo;
        private readonly MovimientoStockBusiness _movimientoStockBusiness;

        public VentaBusiness(
            IVentaRepositorio ventaRepo, 
            IVentaDetalleRepositorio ventaDetalleRepo, 
            MovimientoStockBusiness movimientoStockBusiness)
        {
            _ventaRepo = ventaRepo;
            _ventaDetalleRepo = ventaDetalleRepo;
            _movimientoStockBusiness = movimientoStockBusiness;
        }

        public async Task<string> CrearVenta(Venta venta)
        {
            // TODO: This operation should be transactional to ensure data integrity.
            // 1. Create the main Venta
            var createdVenta = await _ventaRepo.Add(venta);

            // 2. Save Details and update Stock
            foreach (var detalle in venta.Detalles)
            {
                detalle.IdVenta = createdVenta.Id;
                await _ventaDetalleRepo.Add(detalle);

                var movimiento = new MovimientoStock
                {
                    IdProducto = detalle.IdProducto,
                    Fecha = venta.FechaVenta,
                    Cantidad = -detalle.Cantidad, // Sale decreases stock
                    TipoMovimiento = "Venta"
                };
                // The movement registration must be part of the transaction.
                await _movimientoStockBusiness.RegistrarMovimiento(movimiento);
            }

            return createdVenta.Id;
        }

        public async Task<Venta> ObtenerVentaPorId(string id)
        {
            return await _ventaRepo.Get(id);
        }
    }
}

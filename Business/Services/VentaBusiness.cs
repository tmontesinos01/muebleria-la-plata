using Business.Interfaces;
using Data.Interfaces;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Business.Services
{
    public class VentaBusiness : IVentaBusiness
    {
        private readonly IRepository<Venta> _ventaRepo;
        private readonly IRepository<VentaDetalle> _ventaDetalleRepo;
        private readonly IMovimientoStockBusiness _movimientoStockBusiness;

        public VentaBusiness(
            IRepository<Venta> ventaRepo, 
            IRepository<VentaDetalle> ventaDetalleRepo, 
            IMovimientoStockBusiness movimientoStockBusiness)
        {
            _ventaRepo = ventaRepo;
            _ventaDetalleRepo = ventaDetalleRepo;
            _movimientoStockBusiness = movimientoStockBusiness;
        }

        public async Task<string> Add(Venta venta)
        {
            if (venta == null) throw new ArgumentNullException(nameof(venta));

            // TODO: This operation should be transactional.
            venta.Activo = true;
            venta.FechaCreacion = DateTime.UtcNow;

            // 1. Create the main Venta and get its ID
            var ventaId = await _ventaRepo.Add(venta);
            venta.Id = ventaId;

            // 2. Save Details and update Stock
            if (venta.Detalles != null)
            {
                foreach (var detalle in venta.Detalles)
                {
                    detalle.IdVenta = ventaId;
                    detalle.Activo = true;
                    detalle.FechaCreacion = DateTime.UtcNow;
                    await _ventaDetalleRepo.Add(detalle);

                    var movimiento = new MovimientoStock
                    {
                        IdProducto = detalle.IdProducto,
                        Fecha = venta.FechaVenta,
                        Cantidad = -detalle.Cantidad, // Sale decreases stock
                        TipoMovimiento = "Venta",
                        Activo = true,
                        FechaCreacion = DateTime.UtcNow
                    };
                    await _movimientoStockBusiness.RegistrarMovimiento(movimiento);
                }
            }
            return ventaId;
        }

        public int CrearVenta(Venta datos)
        {
            throw new NotImplementedException();
        }

        public async Task Delete(string id)
        {
            var venta = await _ventaRepo.Get(id);
            if (venta != null)
            {
                venta.Activo = false;
                venta.FechaLog = DateTime.UtcNow;
                await _ventaRepo.Update(venta);
            }
        }

        public async Task<Venta?> Get(string id)
        {
            var venta = await _ventaRepo.Get(id);
            if (venta == null || !venta.Activo) return null;
            return venta;
        }

        public async Task<IEnumerable<Venta>> GetAll()
        {
            var ventas = await _ventaRepo.GetAll();
            return ventas.Where(v => v.Activo);
        }

        public Venta ObtenerVentaPorId(string id)
        {
            throw new NotImplementedException();
        }

        public async Task Update(Venta venta)
        {
            venta.FechaLog = DateTime.UtcNow;
            await _ventaRepo.Update(venta);
        }
    }
}

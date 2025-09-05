using Business.Interfaces;
using Data.Interfaces;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Business.Services
{
    public class FacturaBusiness : IFacturaBusiness
    {
        private readonly IRepository<Factura> _facturaRepo;

        public FacturaBusiness(IRepository<Factura> facturaRepo)
        {
            _facturaRepo = facturaRepo;
        }

        public async Task<string> Add(Factura factura)
        {
            factura.Activo = true;
            factura.FechaCreacion = DateTime.UtcNow;
            return await _facturaRepo.Add(factura);
        }

        public async Task Delete(string id)
        {
            var factura = await _facturaRepo.Get(id);
            if (factura != null)
            {
                factura.Activo = false;
                factura.FechaLog = DateTime.UtcNow;
                await _facturaRepo.Update(factura);
            }
        }

        public async Task<Factura?> Get(string id)
        {
            var factura = await _facturaRepo.Get(id);
            if (factura == null || !factura.Activo) return null;
            return factura;
        }

        public async Task<IEnumerable<Factura>> GetAll()
        {
            var facturas = await _facturaRepo.GetAll();
            return facturas.Where(f => f.Activo);
        }

        public async Task Update(Factura factura)
        {
            factura.FechaLog = DateTime.UtcNow;
            await _facturaRepo.Update(factura);
        }

        public async Task<IEnumerable<Factura>> GetFacturasPorVenta(string ventaId)
        {
            var facturas = await GetAll();
            return facturas.Where(f => f.IdVenta == ventaId);
        }

        public async Task<IEnumerable<Factura>> GetFacturasPorEstado(string estado)
        {
            var facturas = await GetAll();
            return facturas.Where(f => f.Estado == estado);
        }

        public async Task<bool> ExisteFacturaParaVenta(string ventaId)
        {
            var facturas = await GetFacturasPorVenta(ventaId);
            return facturas.Any();
        }
    }
}

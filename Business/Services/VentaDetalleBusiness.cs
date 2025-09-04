using Business.Interfaces;
using Data.Interfaces;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Business.Services
{
    public class VentaDetalleBusiness : IVentaDetalleBusiness
    {
        private readonly IRepository<VentaDetalle> _repository;

        public VentaDetalleBusiness(IRepository<VentaDetalle> repository)
        {
            _repository = repository;
        }

        public async Task<string> Add(VentaDetalle entity)
        {
            entity.Activo = true;
            entity.FechaCreacion = DateTime.UtcNow;
            return await _repository.Add(entity);
        }

        public async Task Delete(string id)
        {
            var entity = await _repository.Get(id);
            if (entity != null)
            {
                entity.Activo = false;
                entity.FechaLog = DateTime.UtcNow;
                await _repository.Update(entity);
            }
        }

        public async Task<VentaDetalle?> Get(string id)
        {
            return await _repository.Get(id);
        }

        public async Task<IEnumerable<VentaDetalle>> GetAll()
        {
            return await _repository.GetAll();
        }

        public async Task<IEnumerable<VentaDetalle>> ObtenerDetallesPorVenta(string ventaId)
        {
            var all = await _repository.GetAll();
            return all.Where(d => d.IdVenta == ventaId && d.Activo);
        }

        public async Task Update(VentaDetalle entity)
        {
            entity.FechaLog = DateTime.UtcNow;
            await _repository.Update(entity);
        }
    }
}

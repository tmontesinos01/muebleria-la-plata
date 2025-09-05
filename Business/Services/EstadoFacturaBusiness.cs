using Business.Interfaces;
using Data.Interfaces;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Business.Services
{
    public class EstadoFacturaBusiness : IEstadoFacturaBusiness
    {
        private readonly IRepository<EstadoFactura> _repository;

        public EstadoFacturaBusiness(IRepository<EstadoFactura> repository)
        {
            _repository = repository;
        }

        public async Task<string> Add(EstadoFactura entity)
        {
            entity.Activo = true;
            entity.FechaCreacion = DateTime.UtcNow;
            entity.FechaLog = DateTime.UtcNow;
            entity.UserLog = "Sistema";
            return await _repository.Add(entity);
        }

        public async Task Delete(string id)
        {
            var entity = await _repository.Get(id);
            if (entity != null)
            {
                entity.Activo = false;
                entity.FechaLog = DateTime.UtcNow;
                entity.UserLog = "Sistema";
                await _repository.Update(entity);
            }
        }

        public async Task<EstadoFactura?> Get(string id)
        {
            var item = await _repository.Get(id);
            if (item != null && !item.Activo) return null;
            return item;
        }

        public async Task<IEnumerable<EstadoFactura>> GetAll()
        {
            var items = await _repository.GetAll();
            return items.Where(e => e.Activo);
        }

        public async Task Update(EstadoFactura entity)
        {
            entity.FechaLog = DateTime.UtcNow;
            entity.UserLog = "Sistema";
            await _repository.Update(entity);
        }

        public async Task<EstadoFactura> GetByCodigo(string codigo)
        {
            var items = await GetAll();
            return items.FirstOrDefault(e => e.Codigo == codigo);
        }

        public async Task<IEnumerable<EstadoFactura>> GetEstadosQuePermitenAnulacion()
        {
            var items = await GetAll();
            return items.Where(e => e.PermiteAnulacion);
        }

        public async Task<IEnumerable<EstadoFactura>> GetEstadosFinales()
        {
            var items = await GetAll();
            return items.Where(e => e.EsEstadoFinal);
        }
    }
}

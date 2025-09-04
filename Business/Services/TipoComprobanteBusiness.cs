using Business.Interfaces;
using Data.Interfaces;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Business.Services
{
    public class TipoComprobanteBusiness : ITipoComprobanteBusiness
    {
        private readonly IRepository<TipoComprobante> _repository;

        public TipoComprobanteBusiness(IRepository<TipoComprobante> repository)
        {
            _repository = repository;
        }

        public async Task<string> Add(TipoComprobante entity)
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

        public async Task<TipoComprobante?> Get(string id)
        {
            var item = await _repository.Get(id);
            if (item != null && !item.Activo) return null;
            return item;
        }

        public async Task<IEnumerable<TipoComprobante>> GetAll()
        {
            var items = await _repository.GetAll();
            return items.Where(t => t.Activo);
        }

        public async Task Update(TipoComprobante entity)
        {
            entity.FechaLog = DateTime.UtcNow;
            await _repository.Update(entity);
        }
    }
}

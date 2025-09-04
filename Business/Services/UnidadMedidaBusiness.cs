using Business.Interfaces;
using Data.Interfaces;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Business.Services
{
    public class UnidadMedidaBusiness : IUnidadMedidaBusiness
    { 
        private readonly IRepository<UnidadMedida> _repository;

        public UnidadMedidaBusiness(IRepository<UnidadMedida> repository)
        {
            _repository = repository;
        }

        public async Task<string> Add(UnidadMedida entity)
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

        public async Task<UnidadMedida?> Get(string id)
        {
            var item = await _repository.Get(id);
            if (item != null && !item.Activo) return null;
            return item;
        }

        public async Task<IEnumerable<UnidadMedida>> GetAll()
        {
            var items = await _repository.GetAll();
            return items.Where(u => u.Activo);
        }

        public async Task Update(UnidadMedida entity)
        {
            entity.FechaLog = DateTime.UtcNow;
            await _repository.Update(entity);
        }
    }
}

using Business.Interfaces;
using Data.Interfaces;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Business.Services
{
    public class CategoriaBusiness : ICategoriaBusiness
    {
        private readonly IRepository<Categoria> _repository;

        public CategoriaBusiness(IRepository<Categoria> repository)
        {
            _repository = repository;
        }

        public async Task<string> Add(Categoria entity)
        {
            entity.Activo = true;
            entity.FechaCreacion = DateTime.UtcNow;
            // Correctly return only the ID string
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

        public async Task<Categoria?> Get(string id)
        {
            var item = await _repository.Get(id);
            if (item != null && !item.Activo) return null;
            return item;
        }

        public async Task<IEnumerable<Categoria>> GetAll()
        {
            var items = await _repository.GetAll();
            return items.Where(c => c.Activo);
        }

        public async Task Update(Categoria entity)
        {
            entity.FechaLog = DateTime.UtcNow;
            await _repository.Update(entity);
        }
    }
}

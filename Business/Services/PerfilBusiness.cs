using Business.Interfaces;
using Data.Interfaces;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Business.Services
{
    public class PerfilBusiness : IPerfilBusiness
    {
        private readonly IRepository<Perfil> _repository;

        public PerfilBusiness(IRepository<Perfil> repository)
        {
            _repository = repository;
        }

        public async Task<string> Add(Perfil entity)
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

        public async Task<Perfil?> Get(string id)
        {
            var perfil = await _repository.Get(id);
            if (perfil == null || !perfil.Activo) return null;
            return perfil;
        }

        public async Task<IEnumerable<Perfil>> GetAll()
        {
            var items = await _repository.GetAll();
            return items.Where(p => p.Activo);
        }

        public async Task Update(Perfil entity)
        {
            entity.FechaLog = DateTime.UtcNow;
            await _repository.Update(entity);
        }
    }
}

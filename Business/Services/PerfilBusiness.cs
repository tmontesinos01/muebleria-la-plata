using Data.Interfaces;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Business.Services
{
    public class PerfilBusiness
    {
        private readonly IRepositorio<Perfil> _perfilRepo;

        public PerfilBusiness(IRepositorio<Perfil> perfilRepo)
        {
            _perfilRepo = perfilRepo;
        }

        public async Task<List<Perfil>> GetAll()
        {
            var items = await _perfilRepo.GetAll();
            return items.Where(p => p.Activo).ToList();
        }

        public async Task<Perfil?> GetById(string id)
        {
            var perfil = await _perfilRepo.Get(id);
            if (perfil == null || !perfil.Activo) return null;
            return perfil;
        }

        public async Task<Perfil> Create(Perfil perfil)
        {
            perfil.Activo = true;
            perfil.FechaCreacion = DateTime.UtcNow;
            return await _perfilRepo.Add(perfil);
        }

        public async Task Update(string id, Perfil perfil)
        {
            perfil.FechaLog = DateTime.UtcNow;
            await _perfilRepo.Update(id, perfil);
        }

        public async Task Delete(string id)
        {
            await _perfilRepo.Delete(id);
        }
    }
}

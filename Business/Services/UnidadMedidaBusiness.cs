using Data.Interfaces;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Business.Services
{
    public class UnidadMedidaBusiness
    {
        private readonly IRepositorio<UnidadMedida> _unidadMedidaRepo;

        public UnidadMedidaBusiness(IRepositorio<UnidadMedida> unidadMedidaRepo)
        {
            _unidadMedidaRepo = unidadMedidaRepo;
        }

        public async Task<List<UnidadMedida>> GetAll()
        {
            var items = await _unidadMedidaRepo.GetAll();
            return items.Where(u => u.Activo).ToList();
        }

        public async Task<UnidadMedida?> GetById(string id)
        {
            var unidadMedida = await _unidadMedidaRepo.Get(id);
            if (unidadMedida == null || !unidadMedida.Activo) return null;
            return unidadMedida;
        }

        public async Task<UnidadMedida> Create(UnidadMedida unidadMedida)
        {
            unidadMedida.Activo = true;
            unidadMedida.FechaCreacion = DateTime.UtcNow;
            return await _unidadMedidaRepo.Add(unidadMedida);
        }

        public async Task Update(string id, UnidadMedida unidadMedida)
        {
            unidadMedida.FechaLog = DateTime.UtcNow;
            await _unidadMedidaRepo.Update(id, unidadMedida);
        }

        public async Task Delete(string id)
        {
            await _unidadMedidaRepo.Delete(id);
        }
    }
}

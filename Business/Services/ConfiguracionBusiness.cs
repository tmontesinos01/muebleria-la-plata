using Data.Interfaces;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Business.Services
{
    public class ConfiguracionBusiness
    {
        private readonly IRepositorio<Configuracion> _configuracionRepo;

        public ConfiguracionBusiness(IRepositorio<Configuracion> configuracionRepo)
        {
            _configuracionRepo = configuracionRepo;
        }

        public async Task<List<Configuracion>> GetAll()
        {
            var items = await _configuracionRepo.GetAll();
            return items.Where(c => c.Activo).ToList();
        }

        public async Task<Configuracion?> GetById(string id)
        {
            var configuracion = await _configuracionRepo.Get(id);
            if (configuracion == null || !configuracion.Activo) return null;
            return configuracion;
        }

        public async Task<Configuracion> Create(Configuracion configuracion)
        {
            configuracion.Activo = true;
            configuracion.FechaCreacion = DateTime.UtcNow;
            return await _configuracionRepo.Add(configuracion);
        }

        public async Task Update(string id, Configuracion configuracion)
        {
            configuracion.FechaLog = DateTime.UtcNow;
            await _configuracionRepo.Update(id, configuracion);
        }

        public async Task Delete(string id)
        {
            await _configuracionRepo.Delete(id);
        }
    }
}

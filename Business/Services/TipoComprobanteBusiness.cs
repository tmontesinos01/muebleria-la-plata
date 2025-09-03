using Data.Interfaces;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Business.Services
{
    public class TipoComprobanteBusiness
    {
        private readonly IRepositorio<TipoComprobante> _tipoComprobanteRepo;

        public TipoComprobanteBusiness(IRepositorio<TipoComprobante> tipoComprobanteRepo)
        {
            _tipoComprobanteRepo = tipoComprobanteRepo;
        }

        public async Task<List<TipoComprobante>> GetAll()
        {
            var items = await _tipoComprobanteRepo.GetAll();
            return items.Where(t => t.Activo).ToList();
        }

        public async Task<TipoComprobante?> GetById(string id)
        {
            var tipoComprobante = await _tipoComprobanteRepo.Get(id);
            if (tipoComprobante == null || !tipoComprobante.Activo) return null;
            return tipoComprobante;
        }

        public async Task<TipoComprobante> Create(TipoComprobante tipoComprobante)
        {
            tipoComprobante.Activo = true;
            tipoComprobante.FechaCreacion = DateTime.UtcNow;
            return await _tipoComprobanteRepo.Add(tipoComprobante);
        }

        public async Task Update(string id, TipoComprobante tipoComprobante)
        {
            tipoComprobante.FechaLog = DateTime.UtcNow;
            await _tipoComprobanteRepo.Update(id, tipoComprobante);
        }

        public async Task Delete(string id)
        {
            await _tipoComprobanteRepo.Delete(id);
        }
    }
}

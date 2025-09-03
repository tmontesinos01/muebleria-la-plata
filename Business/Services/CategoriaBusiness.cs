using Data.Interfaces;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Business.Services
{
    public class CategoriaBusiness
    {
        private readonly IRepositorio<Categoria> _categoriaRepo;

        public CategoriaBusiness(IRepositorio<Categoria> categoriaRepo)
        {
            _categoriaRepo = categoriaRepo;
        }

        public async Task<List<Categoria>> GetAll()
        {
            var items = await _categoriaRepo.GetAll();
            return items.Where(c => c.Activo).ToList();
        }

        public async Task<Categoria?> GetById(string id)
        {
            var categoria = await _categoriaRepo.Get(id);
            if (categoria == null || !categoria.Activo) return null;
            return categoria;
        }

        public async Task<Categoria> Create(Categoria categoria)
        {
            categoria.Activo = true;
            categoria.FechaCreacion = DateTime.UtcNow;
            return await _categoriaRepo.Add(categoria);
        }

        public async Task Update(string id, Categoria categoria)
        {
            categoria.FechaLog = DateTime.UtcNow;
            await _categoriaRepo.Update(id, categoria);
        }

        public async Task Delete(string id)
        {
            await _categoriaRepo.Delete(id);
        }
    }
}

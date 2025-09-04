using Business.Interfaces;
using Data.Interfaces;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Business.Services
{
    public class ProductoBusiness : IProductoBusiness
    {
        private readonly IRepository<Producto> _productoRepo;

        public ProductoBusiness(IRepository<Producto> productoRepo)
        {
            _productoRepo = productoRepo;
        }

        public async Task<IEnumerable<Producto>> GetAll()
        {
            var items = await _productoRepo.GetAll();
            return items.Where(p => p.Activo).ToList();
        }

        public async Task<Producto?> Get(string id)
        {
            var producto = await _productoRepo.Get(id);
            if (producto == null || !producto.Activo) return null;
            return producto;
        }

        public async Task<string> Add(Producto producto)
        {
            producto.Activo = true;
            producto.FechaCreacion = DateTime.UtcNow;
            return await _productoRepo.Add(producto);
        }

        public async Task Update(Producto producto)
        {
            producto.FechaLog = DateTime.UtcNow;
            await _productoRepo.Update(producto);
        }

        public async Task Delete(string id)
        {
            var producto = await _productoRepo.Get(id);
            if (producto != null)
            {
                producto.Activo = false;
                producto.FechaLog = DateTime.UtcNow;
                await _productoRepo.Update(producto);
            }
        }
    }
}

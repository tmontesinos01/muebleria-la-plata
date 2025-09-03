using Business.Interfaces;
using Data.Interfaces;
using Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Business.Services
{
    public class ProductoBusiness : IProductoBusiness
    {
        private readonly IProductoRepositorio _productoRepositorio;

        public ProductoBusiness(IProductoRepositorio productoRepositorio)
        {
            _productoRepositorio = productoRepositorio;
        }

        public Task<IEnumerable<Producto>> GetAll()
        {
            return _productoRepositorio.GetAll();
        }

        public Task<Producto> Get(string id)
        {
            return _productoRepositorio.Get(id);
        }

        public async Task<string> Add(Producto producto)
        {
            var newProducto = await _productoRepositorio.Add(producto);
            return newProducto.Id;
        }

        public Task Update(Producto producto)
        {
            return _productoRepositorio.Update(producto.Id, producto);
        }

        public Task Delete(string id)
        {
            return _productoRepositorio.Delete(id);
        }
    }
}

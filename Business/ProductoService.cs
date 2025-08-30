using Data.Interfaces;
using Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Business
{
    public class ProductoService
    {
        private readonly IProductoRepositorio _productoRepositorio;

        public ProductoService(IProductoRepositorio productoRepositorio)
        {
            _productoRepositorio = productoRepositorio;
        }

        public Task<IEnumerable<Producto>> GetAllProductos()
        {
            return _productoRepositorio.GetAll();
        }

        public Task<Producto> GetProductoById(string id)
        {
            return _productoRepositorio.Get(id);
        }

        public Task<string> CreateProducto(Producto producto)
        {
            return _productoRepositorio.Add(producto);
        }

        public Task UpdateProducto(Producto producto)
        {
            return _productoRepositorio.Update(producto);
        }

        public Task DeleteProducto(string id)
        {
            return _productoRepositorio.Delete(id);
        }
    }
}

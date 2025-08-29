using Data.Interfaces;
using Entities;

namespace Business
{
    public class ProductoServicio
    {
        private readonly IProductoRepositorio _productoRepositorio;

        public ProductoServicio(IProductoRepositorio productoRepositorio)
        {
            _productoRepositorio = productoRepositorio;
        }

        public Producto GetProducto(int id)
        {
            return _productoRepositorio.GetProducto(id);
        }
    }
}
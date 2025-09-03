using Data.Interfaces;
using Entities;

namespace Data.Repositorios
{
    public class ProductoRepositorio : RepositorioBase<Producto>, IProductoRepositorio
    {
        public ProductoRepositorio() : base("productos")
        {
        }
    }
}

using Entities;

namespace Data.Interfaces
{
    public interface IProductoRepositorio
    {
        Producto GetProducto(int id);
    }
}
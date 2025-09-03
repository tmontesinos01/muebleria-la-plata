using Entities;
using Data.Interfaces;

namespace Data.Interfaces
{
    public interface IProductoRepositorio : IRepositorio<Producto>
    {
        // We can add specific methods for products here in the future if needed
    }
}
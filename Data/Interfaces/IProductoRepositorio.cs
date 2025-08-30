using Entities;

namespace Data.Interfaces
{
    public interface IProductoRepositorio : IRepository<Producto>
    {
        // We can add specific methods for products here in the future if needed
    }
}
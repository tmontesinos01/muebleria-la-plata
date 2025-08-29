using Data.Interfaces;
using Entities;

namespace Data.Repositorios
{
    public class ProductoRepositorio : IProductoRepositorio
    {
        public Producto GetProducto(int id)
        {
            // Aquí iría la lógica para obtener un producto de la base de datos
            return new Producto { Id = id, Nombre = "Silla", Precio = 50 };
        }
    }
}
using Data.Interfaces;
using Entities;
using Google.Cloud.Firestore;

namespace Data.Repositorios
{
    public class ProductoRepositorio : RepositorioBase<Producto>, IProductoRepositorio
    {
        public ProductoRepositorio(FirestoreDb firestoreDb) : base("productos", firestoreDb)
        {
        }
    }
}

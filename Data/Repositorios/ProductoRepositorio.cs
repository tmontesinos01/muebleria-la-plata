using Data.Interfaces;
using Entities;
using Google.Cloud.Firestore;

namespace Data.Repositorios
{
    public class ProductoRepositorio : FirebaseRepository<Producto>, IProductoRepositorio
    {
        public ProductoRepositorio(FirestoreDb firestoreDb) : base(firestoreDb)
        {
        }
    }
}

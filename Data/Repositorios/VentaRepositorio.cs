using Data.Interfaces;
using Entities;
using Google.Cloud.Firestore;

namespace Data.Repositorios
{
    public class VentaRepositorio : RepositorioBase<Venta>, IVentaRepositorio
    {
        public VentaRepositorio(FirestoreDb firestoreDb) : base("ventas", firestoreDb)
        {
        }
    }
}

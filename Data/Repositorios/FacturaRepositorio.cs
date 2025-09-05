using Data.Interfaces;
using Entities;
using Google.Cloud.Firestore;

namespace Data.Repositorios
{
    public class FacturaRepositorio : RepositorioBase<Factura>
    {
        public FacturaRepositorio(FirestoreDb firestoreDb) : base("facturas", firestoreDb)
        {
        }
    }
}
using Data.Interfaces;
using Entities;
using Google.Cloud.Firestore;

namespace Data.Repositorios
{
    public class MovimientoStockRepositorio : RepositorioBase<MovimientoStock>, IMovimientoStockRepositorio
    {
        public MovimientoStockRepositorio(FirestoreDb firestoreDb) : base("movimientosStock", firestoreDb)
        {
        }
    }
}

using Data.Interfaces;
using Entities;
using Google.Cloud.Firestore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Data.Repositorios
{
    public class VentaDetalleRepositorio : RepositorioBase<VentaDetalle>, IVentaDetalleRepositorio
    {
        public VentaDetalleRepositorio(FirestoreDb firestoreDb) : base("ventasDetalle", firestoreDb)
        {
        }

        public async Task<IEnumerable<VentaDetalle>> ObtenerDetallesPorVentaId(string ventaId)
        {
            Query query = _collection.WhereEqualTo("VentaId", ventaId);
            QuerySnapshot snapshot = await query.GetSnapshotAsync();
            return snapshot.Documents.Select(doc => doc.ConvertTo<VentaDetalle>());
        }
    }
}

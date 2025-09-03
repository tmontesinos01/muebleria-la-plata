using Google.Cloud.Firestore;

namespace Entities
{
    [FirestoreData]
    public class VentaDetalle : EntidadMaestra
    {
        [FirestoreProperty]
        public string IdVenta { get; set; }

        [FirestoreProperty]
        public string IdProducto { get; set; }

        [FirestoreProperty]
        public int Cantidad { get; set; }

        [FirestoreProperty]
        public decimal PrecioUnitario { get; set; }

        [FirestoreProperty]
        public decimal Subtotal { get; set; }
    }
}

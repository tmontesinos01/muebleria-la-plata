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

        [FirestoreProperty]
        public decimal AlicuotaIVA { get; set; } = 21; // Alícuota de IVA (por defecto 21%)

        [FirestoreProperty]
        public decimal IVA { get; set; } // Monto de IVA calculado

        [FirestoreProperty]
        public decimal Total { get; set; } // Total con IVA incluido

        // Método para calcular automáticamente IVA y Total
        public void CalcularIVA()
        {
            IVA = Subtotal * (AlicuotaIVA / 100);
            Total = Subtotal + IVA;
        }
    }
}

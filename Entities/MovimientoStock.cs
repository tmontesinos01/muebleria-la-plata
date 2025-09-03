using Google.Cloud.Firestore;
using System;

namespace Entities
{
    [FirestoreData]
    public class MovimientoStock : EntidadMaestra
    {
        [FirestoreProperty]
        public string IdProducto { get; set; }

        // Se puede usar para referenciar la Venta, Compra o el documento que originó el movimiento
        [FirestoreProperty]
        public string IdDocumentoOrigen { get; set; }

        [FirestoreProperty]
        public DateTime Fecha { get; set; }

        [FirestoreProperty]
        public decimal Cantidad { get; set; } // Positivo para entradas, negativo para salidas

        [FirestoreProperty]
        public string TipoMovimiento { get; set; } // Ej: "Venta", "Devolución", "Ajuste Inventario"
    }
}

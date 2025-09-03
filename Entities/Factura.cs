using Google.Cloud.Firestore;
using System;

namespace Entities
{
    [FirestoreData]
    public class Factura : EntidadMaestra
    {
        // --- Relaciones ---
        [FirestoreProperty]
        public string IdVenta { get; set; }

        [FirestoreProperty]
        public string IdTipoComprobante { get; set; }

        [FirestoreProperty]
        public string IdCliente { get; set; }

        [FirestoreProperty]
        public string IdUsuario { get; set; }

        // --- Datos del Comprobante ---
        [FirestoreProperty]
        public DateTime FechaEmision { get; set; }

        [FirestoreProperty]
        public string NumeroComprobante { get; set; }

        [FirestoreProperty]
        public string CAE { get; set; }

        [FirestoreProperty]
        public DateTime VencimientoCAE { get; set; }

        [FirestoreProperty]
        public string UrlPdf { get; set; }

        // --- Totales ---
        [FirestoreProperty]
        public decimal Subtotal { get; set; }

        [FirestoreProperty]
        public decimal TotalIva { get; set; }

        [FirestoreProperty]
        public decimal Total { get; set; }

        // --- Estado ---
        [FirestoreProperty]
        public bool Anulado { get; set; } = false;
    }
}

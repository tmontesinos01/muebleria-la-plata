using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;

namespace Entities
{
    [FirestoreData]
    public class Venta : EntidadMaestra
    {
        [FirestoreProperty]
        public string IdCliente { get; set; }

        [FirestoreProperty]
        public string IdUsuario { get; set; }

        [FirestoreProperty]
        public DateTime FechaVenta { get; set; }

        [FirestoreProperty]
        public decimal Total { get; set; }

        [FirestoreProperty]
        public List<VentaDetalle> Detalles { get; set; }
    }
}

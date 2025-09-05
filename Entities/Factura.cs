using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;

namespace Entities
{
    [FirestoreData]
    public class Factura : EntidadMaestra
    {
        [FirestoreProperty]
        public string IdVenta { get; set; }

        [FirestoreProperty]
        public string NumeroFactura { get; set; }

        [FirestoreProperty]
        public string CAE { get; set; }

        [FirestoreProperty]
        public DateTime FechaVencimientoCAE { get; set; }

        [FirestoreProperty]
        public decimal Total { get; set; }

        [FirestoreProperty]
        public string UrlPDF { get; set; }

        [FirestoreProperty]
        public string TipoComprobante { get; set; }

        [FirestoreProperty]
        public string PuntoVenta { get; set; }

        [FirestoreProperty]
        public DateTime FechaEmision { get; set; }

        [FirestoreProperty]
        public string Estado { get; set; }

        [FirestoreProperty]
        public string ClienteDocumentoTipo { get; set; }

        [FirestoreProperty]
        public string ClienteDocumentoNro { get; set; }

        [FirestoreProperty]
        public string ClienteRazonSocial { get; set; }

        [FirestoreProperty]
        public string ClienteDireccion { get; set; }

        [FirestoreProperty]
        public string ClienteLocalidad { get; set; }

        [FirestoreProperty]
        public string ClienteProvincia { get; set; }

        [FirestoreProperty]
        public string ClienteCodigoPostal { get; set; }

        [FirestoreProperty]
        public string ClienteCondicionIVA { get; set; }

        [FirestoreProperty]
        public string Observaciones { get; set; }

        [FirestoreProperty]
        public List<ItemFactura> Items { get; set; }

        // Campos extendidos para anulación y notas de crédito/débito
        [FirestoreProperty]
        public string IdFacturaOriginal { get; set; }

        [FirestoreProperty]
        public DateTime? FechaAnulacion { get; set; }

        [FirestoreProperty]
        public string MotivoAnulacion { get; set; }

        [FirestoreProperty]
        public string UsuarioAnulacion { get; set; }

        [FirestoreProperty]
        public string ObservacionesAnulacion { get; set; }
    }

    [FirestoreData]
    public class ItemFactura
    {
        [FirestoreProperty]
        public string IdProducto { get; set; }

        [FirestoreProperty]
        public string Descripcion { get; set; }

        [FirestoreProperty]
        public int Cantidad { get; set; }

        [FirestoreProperty]
        public decimal PrecioUnitario { get; set; }

        [FirestoreProperty]
        public decimal AlicuotaIVA { get; set; }

        [FirestoreProperty]
        public decimal Subtotal { get; set; }

        [FirestoreProperty]
        public decimal IVA { get; set; }

        [FirestoreProperty]
        public decimal Total { get; set; }
    }
}
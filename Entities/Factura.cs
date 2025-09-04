using System;

namespace Entities
{
    public class Factura : EntidadMaestra
    {
        public string VentaId { get; set; }
        public decimal Total { get; set; }
        public DateTime FechaEmision { get; set; }
        public string NumeroComprobante { get; set; }
        // Add other relevant properties for an invoice as needed
    }
}

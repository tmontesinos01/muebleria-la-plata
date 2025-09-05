using System;

namespace Entities.DTOs.Facturacion
{
    public class FacturaEmitidaDTO
    {
        public string numero_factura { get; set; }
        public string cae { get; set; }
        public DateTime fecha_vencimiento_cae { get; set; }
        public decimal total { get; set; }
        public string url_pdf { get; set; }
        public string tipo_comprobante { get; set; }
        public string punto_venta { get; set; }
    }
}

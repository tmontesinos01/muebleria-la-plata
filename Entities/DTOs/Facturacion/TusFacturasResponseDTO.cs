using System;
using System.Collections.Generic;

namespace Entities.DTOs.Facturacion
{
    public class TusFacturasResponseDTO
    {
        public string numero_factura { get; set; }
        public string cae { get; set; }
        public DateTime fecha_vencimiento_cae { get; set; }
        public decimal total { get; set; }
        public string url_pdf { get; set; }
        public string error { get; set; }
        public List<string> errores { get; set; }
    }
}

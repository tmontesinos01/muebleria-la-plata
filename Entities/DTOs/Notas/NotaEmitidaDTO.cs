using System;

namespace Entities.DTOs.Notas
{
    public class NotaEmitidaDTO
    {
        public string id_nota { get; set; }
        public string numero_nota { get; set; }
        public string cae { get; set; }
        public DateTime fecha_vencimiento_cae { get; set; }
        public decimal total { get; set; }
        public string url_pdf { get; set; }
        public string tipo_nota { get; set; }
        public string punto_venta { get; set; }
        public string id_factura_original { get; set; }
        public string motivo { get; set; }
        public DateTime fecha_emision { get; set; }
        public string estado { get; set; }
    }
}

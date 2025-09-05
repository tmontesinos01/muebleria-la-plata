using System;

namespace Entities.DTOs.Facturacion
{
    public class FacturaAnuladaDTO
    {
        public string id_factura { get; set; }
        public string numero_factura { get; set; }
        public DateTime fecha_anulacion { get; set; }
        public string motivo_anulacion { get; set; }
        public string usuario_anulacion { get; set; }
        public string estado { get; set; }
    }
}

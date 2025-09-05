using System.Collections.Generic;

namespace Entities.DTOs.Facturacion
{
    public class EmitirFacturaResponseDTO
    {
        public bool exito { get; set; }
        public string mensaje { get; set; }
        public FacturaEmitidaDTO factura { get; set; }
        public List<string> errores { get; set; }
    }
}

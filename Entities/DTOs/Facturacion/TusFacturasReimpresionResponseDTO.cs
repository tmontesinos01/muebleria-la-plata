using System.Collections.Generic;

namespace Entities.DTOs.Facturacion
{
    public class TusFacturasReimpresionResponseDTO
    {
        public bool exito { get; set; }
        public string mensaje { get; set; }
        public string url_pdf { get; set; }
        public string pdf_base64 { get; set; }
        public string error { get; set; }
        public List<string> errores { get; set; }
    }
}

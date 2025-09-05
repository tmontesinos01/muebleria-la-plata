using System.Collections.Generic;

namespace Entities.DTOs.Facturacion
{
    public class ReimprimirFacturaResponseDTO
    {
        public bool exito { get; set; }
        public string mensaje { get; set; }
        public string url_pdf { get; set; }
        public byte[] pdf_content { get; set; }
        public string nombre_archivo { get; set; }
        public List<string> errores { get; set; }
    }
}

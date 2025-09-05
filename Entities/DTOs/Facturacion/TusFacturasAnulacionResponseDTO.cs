using System.Collections.Generic;

namespace Entities.DTOs.Facturacion
{
    public class TusFacturasAnulacionResponseDTO
    {
        public bool exito { get; set; }
        public string mensaje { get; set; }
        public string error { get; set; }
        public List<string> errores { get; set; }
    }
}

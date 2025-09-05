using System.Collections.Generic;

namespace Entities.DTOs.Notas
{
    public class ConsultarNotasResponseDTO
    {
        public bool exito { get; set; }
        public string mensaje { get; set; }
        public List<NotaEmitidaDTO> notas { get; set; }
        public List<string> errores { get; set; }
    }
}

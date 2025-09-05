using System.Collections.Generic;

namespace Entities.DTOs.Notas
{
    public class EmitirNotaResponseDTO
    {
        public bool exito { get; set; }
        public string mensaje { get; set; }
        public NotaEmitidaDTO nota { get; set; }
        public List<string> errores { get; set; }
    }
}

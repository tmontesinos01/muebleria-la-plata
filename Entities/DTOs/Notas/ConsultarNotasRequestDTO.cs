using System;

namespace Entities.DTOs.Notas
{
    public class ConsultarNotasRequestDTO
    {
        public string id_factura_original { get; set; }
        public string tipo_nota { get; set; } // Opcional: filtrar por tipo
        public DateTime? fecha_desde { get; set; }
        public DateTime? fecha_hasta { get; set; }
    }
}

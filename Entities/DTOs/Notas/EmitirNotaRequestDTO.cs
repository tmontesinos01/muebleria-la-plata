using System.Collections.Generic;

namespace Entities.DTOs.Notas
{
    public class EmitirNotaRequestDTO
    {
        public string tipo_nota { get; set; } // "nota_credito", "nota_debito"
        public string id_factura_original { get; set; }
        public string motivo { get; set; }
        public List<ItemNotaDTO> items { get; set; }
        public string observaciones { get; set; }
        public string usuario_emision { get; set; }
    }
}

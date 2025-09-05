using System.Collections.Generic;

namespace Entities.DTOs.Facturacion
{
    public class EmitirFacturaRequestDTO
    {
        public ClienteAFIPDTO cliente { get; set; }
        public string tipo_comprobante { get; set; } // "factura_a", "factura_b", etc.
        public List<ItemFacturaDTO> items { get; set; }
        public string observaciones { get; set; }
    }
}

namespace Entities.DTOs
{
    public class EmitirFacturaRequestDTO
    {
        public ClienteAFIPDTO cliente { get; set; }
        public string tipo_comprobante { get; set; } // "factura_a", "factura_b", etc.
        public List<ItemFacturaDTO> items { get; set; }
        public string observaciones { get; set; }
    }

    public class ItemFacturaDTO
    {
        public string descripcion { get; set; }
        public int cantidad { get; set; }
        public decimal precio_unitario { get; set; }
        public decimal alicuota_iva { get; set; } = 21; // Solo 21% por ahora
    }

    public class EmitirFacturaResponseDTO
    {
        public bool exito { get; set; }
        public string mensaje { get; set; }
        public FacturaEmitidaDTO factura { get; set; }
        public List<string> errores { get; set; }
    }

    public class FacturaEmitidaDTO
    {
        public string numero_factura { get; set; }
        public string cae { get; set; }
        public DateTime fecha_vencimiento_cae { get; set; }
        public decimal total { get; set; }
        public string url_pdf { get; set; }
        public string tipo_comprobante { get; set; }
        public string punto_venta { get; set; }
    }

    // DTO para la respuesta de TusFacturasAPP
    public class TusFacturasResponseDTO
    {
        public string numero_factura { get; set; }
        public string cae { get; set; }
        public DateTime fecha_vencimiento_cae { get; set; }
        public decimal total { get; set; }
        public string url_pdf { get; set; }
        public string error { get; set; }
        public List<string> errores { get; set; }
    }
}

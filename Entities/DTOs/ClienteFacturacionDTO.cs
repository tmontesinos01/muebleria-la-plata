namespace Entities.DTOs
{
    public class ClienteFacturacionDTO
    {
        public ClienteAFIPDTO cliente { get; set; }
        public bool es_valido { get; set; }
        public List<string> errores { get; set; }
    }

    public class ClienteAFIPDTO
    {
        public string documento_tipo { get; set; } // "CUIT", "CUIL", "DNI"
        public string documento_nro { get; set; }
        public string razon_social { get; set; }
        public string direccion { get; set; }
        public string localidad { get; set; }
        public string provincia { get; set; }
        public string codigopostal { get; set; }
        public string condicion_iva { get; set; } // Código AFIP para condición IVA
    }
}

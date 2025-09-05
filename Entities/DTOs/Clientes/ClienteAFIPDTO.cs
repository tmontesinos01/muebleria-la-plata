namespace Entities.DTOs.Clientes
{
    public class ClienteAFIPDTO
    {
        public string documento_tipo { get; set; }
        public string documento_nro { get; set; }
        public string razon_social { get; set; }
        public string direccion { get; set; }
        public string localidad { get; set; }
        public string provincia { get; set; }
        public string codigopostal { get; set; }
        public string condicion_iva { get; set; }
    }
}

namespace Entities.DTOs
{
    public class ValidarClienteRequestDTO
    {
        public string documento_nro { get; set; }
        public string documento_tipo { get; set; } // "CUIT", "CUIL", "DNI"
    }
}

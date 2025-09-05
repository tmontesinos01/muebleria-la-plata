using System.Collections.Generic;

namespace Entities.DTOs.Clientes
{
    public class ValidarClienteResponseDTO
    {
        public string error { get; set; }
        public string razon_social { get; set; }
        public string condicion_impositiva { get; set; }
        public string direccion { get; set; }
        public string localidad { get; set; }
        public string codigopostal { get; set; }
        public string estado { get; set; }
        public string provincia { get; set; }
        public string actividad { get; set; }
        public List<string> errores { get; set; }
        public string descripcion { get; set; }
        public string id { get; set; }
        public string nomenclador { get; set; }
        public string periodo { get; set; }
    }
}

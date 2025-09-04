namespace Entities.DTOs
{
    public class UserInfoDTO
    {
        public int id { get; set; }
        public string nombre { get; set; }
        public string correo { get; set; }
        public int idPerfil { get; set; }
        public bool activo { get; set; }
        public DateTime? ultimoAcceso { get; set; }
    }
}

namespace Entities.DTOs.Auth
{
    public class AuthResponseDTO
    {
        public bool success { get; set; }
        public string message { get; set; }
        public AuthDataDTO data { get; set; }
    }
}

namespace Entities.DTOs.Auth
{
    public class AuthDataDTO
    {
        public string token { get; set; }
        public string refreshToken { get; set; }
        public int expiresIn { get; set; }
        public UserInfoDTO user { get; set; }
    }
}

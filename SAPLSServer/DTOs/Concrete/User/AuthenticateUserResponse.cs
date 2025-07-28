namespace SAPLSServer.DTOs.Concrete
{
    public class AuthenticateUserResponse
    {
        public string AccessToken { get; set; } = null!;
        public string RefreshToken { get; set; } = null!;
        public string TokenType { get; set; } = "Bearer";
        public int ExpiresIn { get; set; }
        public DateTime ExpiresAt { get; set; }

    }
}

namespace SAPLSServer.DTOs.Concrete.User.ClientProfile
{
    public class AuthenticateClientProfileDto : AuthenticateUserDto
    {
        public string? CitizenId { get; set; }
    }
}

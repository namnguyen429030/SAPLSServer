namespace SAPLSServer.DTOs.Concrete
{
    public class AuthenticateClientProfileRequest : AuthenticateUserRequest
    {
        public string? CitizenId { get; set; }
    }
}

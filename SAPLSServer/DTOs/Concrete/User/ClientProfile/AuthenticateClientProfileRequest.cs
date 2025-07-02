namespace SAPLSServer.DTOs.Concrete.User
{
    public class AuthenticateClientProfileRequest : AuthenticateUserRequest
    {
        public string? CitizenId { get; set; }
    }
}

namespace SAPLSServer.DTOs.Concrete.User
{
    public class AdminProfileDetailsDto : UserDetailsDto
    {
        public string AdminId { get; set; } = null!;
        public string Role { get; set; } = null!;
    }
}

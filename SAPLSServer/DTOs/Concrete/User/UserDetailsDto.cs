using SAPLSServer.DTOs.Base;

namespace SAPLSServer.DTOs.Concrete.User
{
    public abstract class UserDetailsDto : UserSummaryDto
    {
        public string? ProfileImageUrl { get; set; }
        public string Phone { get; set; } = null!;
        public DateTime UpdatedAt { get; set; }
    }
}

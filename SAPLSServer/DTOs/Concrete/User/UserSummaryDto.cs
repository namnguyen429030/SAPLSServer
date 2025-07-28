using SAPLSServer.DTOs.Base;

namespace SAPLSServer.DTOs.Concrete
{
    public abstract class UserSummaryDto : GetResult
    {
        public string Email { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public string Status { get; set; } = null!;
    }
}

namespace SAPLSServer.DTOs.Concrete.User
{
    public class AdminProfileSummaryDto : UserSummaryDto
    {
        public string AdminId { get; set; } = null!;
        public string Role { get; set; } = null!;
    }
}

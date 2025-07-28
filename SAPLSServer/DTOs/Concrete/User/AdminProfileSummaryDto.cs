using SAPLSServer.Models;

namespace SAPLSServer.DTOs.Concrete
{
    public class AdminProfileSummaryDto : UserSummaryDto
    {
        public string AdminId { get; set; }
        public string Role { get; set; }

        public AdminProfileSummaryDto(AdminProfile model)
        {
            Id = model.UserId;
            Email = model.User.Email;
            FullName = model.User.FullName;
            CreatedAt = model.User.CreatedAt;
            Status = model.User.Status;
            AdminId = model.AdminId;
            Role = model.Role;
        }
    }
}

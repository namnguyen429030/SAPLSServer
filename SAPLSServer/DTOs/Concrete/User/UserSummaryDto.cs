using SAPLSServer.DTOs.Base;
using SAPLSServer.Models;

namespace SAPLSServer.DTOs.Concrete.UserDto
{
    public abstract class UserSummaryDto : GetResult
    {
        public string Email { get; set; }
        public string FullName { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Status { get; set; }
        protected UserSummaryDto(User user)
        {
            Id = user.Id;
            Email = user.Email;
            FullName = user.FullName;
            CreatedAt = user.CreatedAt;
            Status = user.Status.ToString();
        }
    }
}

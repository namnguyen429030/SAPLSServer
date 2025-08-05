using SAPLSServer.DTOs.Base;
using SAPLSServer.Models;

namespace SAPLSServer.DTOs.Concrete.UserDto
{
    public abstract class UserDetailsDto : UserSummaryDto
    {
        public string? ProfileImageUrl { get; set; }
        public string Phone { get; set; }
        public DateTime UpdatedAt { get; set; }
        protected UserDetailsDto(User user) : base(user)
        {
            ProfileImageUrl = user.ProfileImageUrl;
            Phone = user.Phone;
            UpdatedAt = user.UpdatedAt;
        }
    }
}

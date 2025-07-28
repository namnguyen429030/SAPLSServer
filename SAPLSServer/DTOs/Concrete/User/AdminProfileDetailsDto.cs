using SAPLSServer.Models;

namespace SAPLSServer.DTOs.Concrete
{
    public class AdminProfileDetailsDto : UserDetailsDto
    {
        public string AdminId { get; set; }
        public string Role { get; set; }

        public AdminProfileDetailsDto(AdminProfile model)
        {
            Id = model.UserId;
            Email = model.User.Email;
            FullName = model.User.FullName;
            CreatedAt = model.User.CreatedAt;
            Status = model.User.Status;
            ProfileImageUrl = model.User.ProfileImageUrl;
            Phone = model.User.Phone;
            UpdatedAt = model.User.UpdatedAt;

            AdminId = model.AdminId;
            Role = model.Role;
        }
    }
}

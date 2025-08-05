using SAPLSServer.Models;

namespace SAPLSServer.DTOs.Concrete.UserDto
{
    public class AdminProfileDetailsDto : UserDetailsDto
    {
        public string AdminId { get; set; }
        public string AdminRole { get; set; }

        public AdminProfileDetailsDto(AdminProfile model) : base(model.User)
        {
            AdminId = model.AdminId;
            AdminRole = model.Role;
        }
    }
}

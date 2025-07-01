using SAPLSServer.DTOs.Base;
using System.ComponentModel.DataAnnotations;

namespace SAPLSServer.DTOs.Concrete.User.AdminProfile
{
    public class UpdateAdminProfileDto : UpdateUserDto
    {
        [Required(ErrorMessage = "Admin ID is required.")]
        public string AdminId { get; set; } = null!;
        [Required(ErrorMessage = "Role is required.")]
        public string Role { get; set; } = null!;
    }
}

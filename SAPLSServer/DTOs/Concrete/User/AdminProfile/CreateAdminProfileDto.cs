using System.ComponentModel.DataAnnotations;

namespace SAPLSServer.DTOs.Concrete.User
{
    public class CreateAdminProfileDto : CreateUserDto
    {
        [Required(ErrorMessage = "Admin ID is required.")]
        public string AdminId { get; set; } = null!;
        [Required(ErrorMessage = "Role is required.")]
        public string Role { get; set; } = null!;
    }
}

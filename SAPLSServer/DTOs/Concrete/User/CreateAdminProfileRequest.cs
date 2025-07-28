using System.ComponentModel.DataAnnotations;

namespace SAPLSServer.DTOs.Concrete
{
    public class CreateAdminProfileRequest : CreateUserRequest
    {
        [Required(ErrorMessage = "Admin ID is required.")]
        public string AdminId { get; set; } = null!;
    }
}

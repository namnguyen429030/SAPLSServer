using System.ComponentModel.DataAnnotations;

namespace SAPLSServer.DTOs.Concrete
{
    public class UpdateAdminProfileRequest : UpdateUserInformationRequest
    {
        [Required(ErrorMessage = "Admin ID is required.")]
        public string AdminId { get; set; } = null!;
        [Required(ErrorMessage = "Role is required.")]
        public string Role { get; set; } = null!;
    }
}

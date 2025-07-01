using SAPLSServer.DTOs.Base;
using System.ComponentModel.DataAnnotations;

namespace SAPLSServer.DTOs.Concrete.User.StaffProfile
{
    public class UpdateStaffProfileDto : UpdateDto
    {
        [Required(ErrorMessage = "Staff ID is required.")]
        public string StaffId { get; set; } = null!;
    }
}

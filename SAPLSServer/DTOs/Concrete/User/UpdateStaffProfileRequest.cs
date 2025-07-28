using SAPLSServer.DTOs.Base;
using System.ComponentModel.DataAnnotations;

namespace SAPLSServer.DTOs.Concrete
{
    public class UpdateStaffProfileRequest : UpdateRequest
    {
        [Required(ErrorMessage = "Staff ID is required.")]
        public string StaffId { get; set; } = null!;
    }
}

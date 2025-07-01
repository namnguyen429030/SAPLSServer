using Microsoft.Identity.Client;
using System.ComponentModel.DataAnnotations;

namespace SAPLSServer.DTOs.Concrete.User
{
    public class CreateStaffProfileDto : CreateUserDto
    {
        [Required(ErrorMessage = "Staff ID is required.")]
        public string StaffId { get; set; } = null!;
        [Required(ErrorMessage = "Parking Lot ID is required.")]
        public string ParkingLotId { get; set; } = null!;
    }
}

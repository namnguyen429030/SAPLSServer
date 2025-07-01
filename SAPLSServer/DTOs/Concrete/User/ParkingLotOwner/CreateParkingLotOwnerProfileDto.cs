using System.ComponentModel.DataAnnotations;

namespace SAPLSServer.DTOs.Concrete.User
{
    public class CreateParkingLotOwnerProfileDto : CreateUserDto
    {
        [Required(ErrorMessage = "Parking Lot Owner ID is required.")]
        public string ParkingLotOwnerId { get; set; } = null!;
    }
}

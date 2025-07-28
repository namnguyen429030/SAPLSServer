using System.ComponentModel.DataAnnotations;

namespace SAPLSServer.DTOs.Concrete
{
    public class CreateParkingLotOwnerProfileRequest : CreateUserRequest
    {
        [Required(ErrorMessage = "Parking Lot Owner ID is required.")]
        public string ParkingLotOwnerId { get; set; } = null!;
    }
}

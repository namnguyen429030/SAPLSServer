using SAPLSServer.DTOs.Base;
using System.ComponentModel.DataAnnotations;

namespace SAPLSServer.DTOs.Concrete.User
{
    public class UpdateParkingLotOwnerProfileRequest : UpdateUserRequest
    {
        [Required(ErrorMessage = "Parking Lot Owner ID is required.")]
        public string ParkingLotOwnerId { get; set; } = null!;
    }
}

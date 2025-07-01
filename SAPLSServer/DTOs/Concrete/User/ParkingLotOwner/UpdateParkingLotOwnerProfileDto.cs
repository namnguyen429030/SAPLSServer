using SAPLSServer.DTOs.Base;
using System.ComponentModel.DataAnnotations;

namespace SAPLSServer.DTOs.Concrete.User.ParkingLotOwner
{
    public class UpdateParkingLotOwnerProfileDto : UpdateUserDto
    {
        [Required(ErrorMessage = "Parking Lot Owner ID is required.")]
        public string ParkingLotOwnerId { get; set; } = null!;
    }
}

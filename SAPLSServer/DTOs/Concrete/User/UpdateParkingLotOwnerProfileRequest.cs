using SAPLSServer.DTOs.Base;
using System.ComponentModel.DataAnnotations;

namespace SAPLSServer.DTOs.Concrete
{
    public class UpdateParkingLotOwnerProfileRequest : UpdateUserInformationRequest
    {
        [Required(ErrorMessage = "Parking Lot Owner ID is required.")]
        public string ParkingLotOwnerId { get; set; } = null!;
    }
}

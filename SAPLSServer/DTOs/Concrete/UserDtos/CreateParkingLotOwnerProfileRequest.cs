using SAPLSServer.Constants;
using System.ComponentModel.DataAnnotations;

namespace SAPLSServer.DTOs.Concrete.UserDtos
{
    public class CreateParkingLotOwnerProfileRequest : CreateUserRequest
    {
        [Required(ErrorMessage = MessageKeys.PARKING_LOT_OWNER_ID_REQUIRED)]
        public string ParkingLotOwnerId { get; set; } = null!;
    }
}

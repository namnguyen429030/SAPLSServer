using SAPLSServer.Constants;
using SAPLSServer.DTOs.Concrete.ParkingLotDto;
using System.ComponentModel.DataAnnotations;

namespace SAPLSServer.DTOs.Concrete.ParkingSessionDto
{
    public class GetParkingSessionListByParkingLotIdRequest : GetOwnedParkingSessionListRequest
    {
        [Required(ErrorMessage = MessageKeys.PARKING_LOT_ID_REQUIRED)]
        public string? ParkingLotId { get; set; }
    }
}

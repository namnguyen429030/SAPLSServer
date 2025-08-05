using SAPLSServer.Models;

namespace SAPLSServer.DTOs.Concrete.UserDto
{
    public class ParkingLotOwnerProfileSummaryDto : UserSummaryDto
    {
        public string ParkingLotOwnerId { get; set; }

        public ParkingLotOwnerProfileSummaryDto(ParkingLotOwnerProfile parkingLotOwner) : base(parkingLotOwner.User)
        {
            ParkingLotOwnerId = parkingLotOwner.ParkingLotOwnerId;
        }
    }
}

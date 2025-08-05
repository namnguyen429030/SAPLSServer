using SAPLSServer.Models;

namespace SAPLSServer.DTOs.Concrete.UserDto
{
    public class ParkingLotOwnerProfileDetailsDto : UserDetailsDto
    {
        public string ParkingLotOwnerId { get; set; }
        public ParkingLotOwnerProfileDetailsDto(ParkingLotOwnerProfile parkingLotOwner) 
            : base(parkingLotOwner.User)
        {
            ParkingLotOwnerId = parkingLotOwner.ParkingLotOwnerId;
        }
    }
}

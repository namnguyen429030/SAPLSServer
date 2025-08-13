using SAPLSServer.DTOs.Concrete.UserDtos;
using SAPLSServer.Models;

namespace SAPLSServer.DTOs.Concrete.ParkingLotDtos
{
    public class ParkingLotDetailsDto : ParkingLotSummaryDto
    {
        public string? Description { get; set; }
        public int TotalParkingSlot { get; set; }
        public ParkingLotOwnerProfileDetailsDto ParkingLotOwner { get; set; }
        public ParkingLotDetailsDto(ParkingLot parkingLot) : base(parkingLot)
        {
            Description = parkingLot.Description;
            TotalParkingSlot = parkingLot.TotalParkingSlot;
            ParkingLotOwner = new ParkingLotOwnerProfileDetailsDto(parkingLot.ParkingLotOwner);
        }
    }
}

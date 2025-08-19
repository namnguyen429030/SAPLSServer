using SAPLSServer.DTOs.Base;
using SAPLSServer.Models;

namespace SAPLSServer.DTOs.Concrete.ParkingLotDto {
    public class ParkingLotSummaryDto : GetResult {
        public string Name {
            get; set;
        }
        public string Address {
            get; set;
        }
        public string? Description {
            get; set;
        }
        public int TotalParkingSlot {
            get; set;
        }
        public ParkingLotSummaryDto(ParkingLot parkingLot) {
            Id = parkingLot.Id;
            Name = parkingLot.Name;
            Address = parkingLot.Address;
            Description = parkingLot.Description;
            TotalParkingSlot = parkingLot.TotalParkingSlot;
        }
    }
}

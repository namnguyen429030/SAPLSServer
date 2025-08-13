using SAPLSServer.DTOs.Base;
using SAPLSServer.Models;

namespace SAPLSServer.DTOs.Concrete.ParkingLotDtos
{
    public class ParkingLotSummaryDto : GetResult
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public ParkingLotSummaryDto(ParkingLot parkingLot)
        {
            Id = parkingLot.Id;
            Name = parkingLot.Name;
            Address = parkingLot.Address;
        }
    }
}

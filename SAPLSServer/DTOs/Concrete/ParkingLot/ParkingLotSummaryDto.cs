using SAPLSServer.DTOs.Base;

namespace SAPLSServer.DTOs.Concrete.ParkingLot
{
    public class ParkingLotSummaryDto : GetDto
    {
        public string Name { get; set; } = null!;
        public string Address { get; set; } = null!;
    }
}

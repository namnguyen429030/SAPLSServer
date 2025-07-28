using SAPLSServer.DTOs.Concrete;

namespace SAPLSServer.DTOs.Concrete
{
    public class ParkingSessionDetailsForClientDto : ParkingSessionDetailsForParkingLotDto
    {
        public ParkingLotSummaryDto ParkingLot { get; set; } = null!;
    }
}

using SAPLSServer.DTOs.Concrete.ParkingLot;

namespace SAPLSServer.DTOs.Concrete.ParkingSession
{
    public class ParkingSessionDetailsForClientDto : ParkingSessionDetailsForParkingLotDto
    {
        public ParkingLotSummaryDto ParkingLot { get; set; } = null!;
    }
}

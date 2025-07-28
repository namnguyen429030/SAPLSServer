using SAPLSServer.Models;

namespace SAPLSServer.DTOs.Concrete
{
    public class ParkingSessionSummaryForClientDto : ParkingSessionSummaryForParkingLotDto
    {
        public string ParkingLotName { get; set; } = null!;
        public ParkingSessionSummaryForClientDto(ParkingSession parkingSession) : base(parkingSession)
        {
            ParkingLotName = parkingSession.ParkingLot?.Name ?? string.Empty;
        }
    }
}

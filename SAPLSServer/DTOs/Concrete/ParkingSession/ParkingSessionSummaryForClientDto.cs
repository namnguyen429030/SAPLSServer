namespace SAPLSServer.DTOs.Concrete.ParkingSession
{
    public class ParkingSessionSummaryForClientDto : ParkingSessionSummaryForParkingLotDto
    {
        public string ParkingLotName { get; set; } = null!;
    }
}

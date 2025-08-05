using SAPLSServer.Models;

namespace SAPLSServer.DTOs.Concrete.ParkingSessionDto
{
    public class ParkingSessionSummaryForClientDto : ParkingSessionSummaryForParkingLotDto
    {
        public string ParkingLotName { get; set; }
        public ParkingSessionSummaryForClientDto(ParkingSession parkingSession) : base(parkingSession)
        {
            ParkingLotName = parkingSession.ParkingLot.Name;
        }
    }
}

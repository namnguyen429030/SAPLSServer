using SAPLSServer.DTOs.Concrete.ParkingLotDto;
using SAPLSServer.Models;

namespace SAPLSServer.DTOs.Concrete.ParkingSessionDto
{
    public class ParkingSessionDetailsForClientDto : ParkingSessionDetailsForParkingLotDto
    {
        public ParkingLotSummaryDto ParkingLot { get; set; }
        public ParkingSessionDetailsForClientDto(ParkingSession session) : base(session)
        {
            ParkingLot = new ParkingLotSummaryDto(session.ParkingLot);
        }
    }
}

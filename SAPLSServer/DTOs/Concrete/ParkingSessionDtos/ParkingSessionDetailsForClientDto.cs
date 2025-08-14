using SAPLSServer.DTOs.Concrete.ParkingLotDtos;
using SAPLSServer.Models;

namespace SAPLSServer.DTOs.Concrete.ParkingSessionDtos
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

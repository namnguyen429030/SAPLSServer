using SAPLSServer.DTOs.Base;

namespace SAPLSServer.DTOs.Concrete.ParkingSession
{
    public class ParkingSessionSummaryForParkingLotDto : GetDto
    {
        public string VehicleNumber { get; set; } = null!;
        public DateTime EntryDateTime { get; set; }
        public DateTime? ExitDateTime { get; set; }
        public decimal Cost { get; set; }
        public string PaymentStatus { get; set; } = null!;
    }
}

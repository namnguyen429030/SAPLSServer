using SAPLSServer.DTOs.Base;
using SAPLSServer.DTOs.Concrete.ParkingLot;
using SAPLSServer.DTOs.Concrete.Vehicle;

namespace SAPLSServer.DTOs.Concrete.ParkingSession
{
    public class ParkingSessionDetailsForParkingLotDto : GetDto
    {
        public VehicleSummaryDto Vehicle { get; set; } = null!;
        public DateTime EntryDateTime { get; set; }

        public DateTime? ExitDateTime { get; set; }

        public DateTime? CheckOutDateTime { get; set; }

        public string EntryFrontCaptureUrl { get; set; } = null!;

        public string EntryBackCaptureUrl { get; set; } = null!;

        public string? ExitFrontCaptureUrl { get; set; }

        public string? ExitBackCaptureUrl { get; set; }
        public string PaymentMethod { get; set; } = null!;
        public decimal Cost { get; set; }
    }
}

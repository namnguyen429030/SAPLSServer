using SAPLSServer.DTOs.Base;

namespace SAPLSServer.DTOs.Concrete
{
    public class ParkingSessionDetailsForParkingLotDto : GetResult
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

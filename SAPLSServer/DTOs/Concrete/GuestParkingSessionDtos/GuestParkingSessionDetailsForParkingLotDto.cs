using SAPLSServer.Models;

namespace SAPLSServer.DTOs.Concrete.GuestParkingSessionDtos
{
    public class GuestParkingSessionDetailsForParkingLotDto : GuestParkingSessionSummaryForParkingLotDto
    {
        public string EntryFrontCaptureUrl { get; set; }
        public string EntryBackCaptureUrl { get; set; }
        public string? ExitFrontCaptureUrl { get; set; }
        public string? ExitBackCaptureUrl { get; set; }

        public GuestParkingSessionDetailsForParkingLotDto(GuestParkingSession session) : base(session)
        {
            EntryFrontCaptureUrl = session.EntryFrontCaptureUrl;
            EntryBackCaptureUrl = session.EntryBackCaptureUrl;
            ExitFrontCaptureUrl = session.ExitFrontCaptureUrl;
            ExitBackCaptureUrl = session.ExitBackCaptureUrl;
        }
    }
}
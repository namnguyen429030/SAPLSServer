using SAPLSServer.Models;

namespace SAPLSServer.DTOs.Concrete.GuestParkingSessionDtos
{
    public class GuestParkingSessionDetailsForParkingLotDto
    {
        public string Id { get; set; }
        public string VehicleLicensePlate { get; set; }
        public DateTime EntryDateTime { get; set; }
        public DateTime? ExitDateTime { get; set; }
        public string EntryFrontCaptureUrl { get; set; }
        public string EntryBackCaptureUrl { get; set; }
        public string? ExitFrontCaptureUrl { get; set; }
        public string? ExitBackCaptureUrl { get; set; }
        public decimal Cost { get; set; }
        public string Status { get; set; }

        public GuestParkingSessionDetailsForParkingLotDto(GuestParkingSession session)
        {
            Id = session.Id;
            VehicleLicensePlate = session.VehicleLicensePlate;
            EntryDateTime = session.EntryDateTime;
            ExitDateTime = session.ExitDateTime;
            EntryFrontCaptureUrl = session.EntryFrontCaptureUrl;
            EntryBackCaptureUrl = session.EntryBackCaptureUrl;
            ExitFrontCaptureUrl = session.ExitFrontCaptureUrl;
            ExitBackCaptureUrl = session.ExitBackCaptureUrl;
            Cost = session.Cost;
            Status = session.Status;
        }
    }
}
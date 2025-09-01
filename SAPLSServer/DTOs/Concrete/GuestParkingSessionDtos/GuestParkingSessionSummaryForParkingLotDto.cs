using SAPLSServer.Models;

namespace SAPLSServer.DTOs.Concrete.GuestParkingSessionDtos
{
    public class GuestParkingSessionSummaryForParkingLotDto
    {
        public string Id { get; set; }
        public string VehicleLicensePlate { get; set; }
        public string VehicleType { get; set; }
        public DateTime EntryDateTime { get; set; }
        public DateTime? ExitDateTime { get; set; }
        public decimal Cost { get; set; }
        public string Status { get; set; }

        public GuestParkingSessionSummaryForParkingLotDto(GuestParkingSession session)
        {
            Id = session.Id;
            VehicleLicensePlate = session.VehicleLicensePlate;
            VehicleType = session.VehicleType;
            EntryDateTime = session.EntryDateTime;
            ExitDateTime = session.ExitDateTime;
            Cost = session.Cost;
            Status = session.Status;
        }
    }
}
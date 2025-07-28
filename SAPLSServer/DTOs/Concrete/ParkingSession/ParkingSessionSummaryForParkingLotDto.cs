using SAPLSServer.DTOs.Base;
using SAPLSServer.Models;

namespace SAPLSServer.DTOs.Concrete
{
    public class ParkingSessionSummaryForParkingLotDto : GetResult
    {
        public string VehicleNumber { get; set; }
        public DateTime EntryDateTime { get; set; }
        public DateTime? ExitDateTime { get; set; }
        public decimal Cost { get; set; }
        public string PaymentStatus { get; set; }
        public ParkingSessionSummaryForParkingLotDto(ParkingSession parkingSession)
        {
            Id = parkingSession.Id;
            VehicleNumber = parkingSession.Vehicle.LicensePlate;
            EntryDateTime = parkingSession.EntryDateTime;
            ExitDateTime = parkingSession.ExitDateTime;
            Cost = parkingSession.Cost;
            //TODO: Add Payment's Payment status and status
        }
    }
}

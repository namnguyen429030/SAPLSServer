using SAPLSServer.DTOs.Base;
using System.ComponentModel.DataAnnotations;

namespace SAPLSServer.DTOs.Concrete.ParkingSession
{
    public class CreateParkingSessionDto : CreateDto
    {
        [Required(ErrorMessage = "Vehicle ID is required.")]
        public string VehicleId { get; set; } = null!;
        [Required(ErrorMessage = "Parking Lot ID is required.")]
        public string ParkingLotId { get; set; } = null!;
        [Required(ErrorMessage = "Entry Date and Time is required.")]
        public DateTime EntryDateTime { get; set; }
        [Required(ErrorMessage = "Entry Front Capture is required.")]
        public string EntryFrontCaptureUrl { get; set; } = null!;
        [Required(ErrorMessage = "Entry Back Capture is required.")]
        public string EntryBackCaptureUrl { get; set; } = null!;
    }
}

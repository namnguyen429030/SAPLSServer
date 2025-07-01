using SAPLSServer.DTOs.Base;
using System.ComponentModel.DataAnnotations;

namespace SAPLSServer.DTOs.Concrete.ParkingSession
{
    public class UpdateParkingSessionExitDto : UpdateDto
    {
        [Required(ErrorMessage = "Exit date and time is required.")]
        public DateTime ExitDateTime { get; set; }
        [Required(ErrorMessage = "Exit front capture is required.")]
        public string ExitFrontCaptureUrl { get; set; } = null!;
        [Required(ErrorMessage = "Exit back capture is required.")]
        public string ExitBackCaptureUrl { get; set; } = null!;
    }
}

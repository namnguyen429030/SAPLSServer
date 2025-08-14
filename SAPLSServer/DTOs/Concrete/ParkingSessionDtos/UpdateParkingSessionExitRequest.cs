using SAPLSServer.DTOs.Base;
using System.ComponentModel.DataAnnotations;
using SAPLSServer.Constants;

namespace SAPLSServer.DTOs.Concrete.ParkingSessionDtos
{
    public class UpdateParkingSessionExitRequest : UpdateRequest
    {
        [Required(ErrorMessage = MessageKeys.ENTRY_FRONT_CAPTURE_REQUIRED)]
        [Url(ErrorMessage = MessageKeys.EXIT_FRONT_CAPTURE_URL_INVALID_FORMAT)]
        public string ExitFrontCaptureUrl { get; set; } = null!;
        [Required(ErrorMessage = MessageKeys.ENTRY_BACK_CAPTURE_REQUIRED)]
        [Url(ErrorMessage = MessageKeys.EXIT_BACK_CAPTURE_URL_INVALID_FORMAT)]
        public string ExitBackCaptureUrl { get; set; } = null!;
    }
}

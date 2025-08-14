using SAPLSServer.DTOs.Base;
using System.ComponentModel.DataAnnotations;
using SAPLSServer.Constants;

namespace SAPLSServer.DTOs.Concrete.ParkingSessionDtos
{
    public class CreateParkingSessionRequest
    {
        [Required(ErrorMessage = MessageKeys.VEHICLE_ID_REQUIRED)]
        public string VehicleId { get; set; } = null!;
        [Required(ErrorMessage = MessageKeys.PARKING_LOT_ID_REQUIRED)]
        public string ParkingLotId { get; set; } = null!;
        [Required(ErrorMessage = MessageKeys.ENTRY_FRONT_CAPTURE_REQUIRED)]
        [DataType(DataType.Upload, ErrorMessage = MessageKeys.INVALID_FRONT_CAPTURE_IMAGE_UPLOADED)]
        public IFormFile? EntryFrontCaptureUrl { get; set; }
        [Required(ErrorMessage = MessageKeys.ENTRY_BACK_CAPTURE_REQUIRED)]
        [DataType(DataType.Upload, ErrorMessage = MessageKeys.INVALID_BACK_CAPTURE_IMAGE_UPLOADED)]
        public IFormFile? EntryBackCaptureUrl { get; set; }
    }
}

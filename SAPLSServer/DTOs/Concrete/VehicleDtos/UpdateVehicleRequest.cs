using SAPLSServer.DTOs.Base;
using System.ComponentModel.DataAnnotations;
using SAPLSServer.Constants;

namespace SAPLSServer.DTOs.Concrete.VehicleDtos
{
    public class UpdateVehicleRequest : UpdateRequest
    {
        [Required(ErrorMessage = MessageKeys.FRONT_VEHICLE_REGISTRATION_CERT_IMAGE_REQUIRED)]
        public IFormFile FrontVehicleRegistrationCertImage { get; set; } = null!;
        [Required(ErrorMessage = MessageKeys.BACK_VEHICLE_REGISTRATION_CERT_IMAGE_REQUIRED)]
        public IFormFile BackVehicleRegistrationCertImage { get; set; } = null!;
        [Required(ErrorMessage = MessageKeys.LICENSE_PLATE_REQUIRED)]
        public string LicensePlate { get; set; } = null!;
        [Required(ErrorMessage = MessageKeys.BRAND_REQUIRED)]
        public string Brand { get; set; } = null!;
        [Required(ErrorMessage = MessageKeys.MODEL_REQUIRED)]
        public string Model { get; set; } = null!;
        [Required(ErrorMessage = MessageKeys.ENGINE_NUMBER_REQUIRED)]
        public string EngineNumber { get; set; } = null!;
        [Required(ErrorMessage = MessageKeys.CHASSIS_NUMBER_REQUIRED)]
        public string ChassisNumber { get; set; } = null!;
        [Required(ErrorMessage = MessageKeys.COLOR_REQUIRED)]
        public string Color { get; set; } = null!;
        [Required(ErrorMessage = MessageKeys.OWNER_FULL_NAME_REQUIRED)]
        public string OwnerVehicleFullName { get; set; } = null!;
    }
}

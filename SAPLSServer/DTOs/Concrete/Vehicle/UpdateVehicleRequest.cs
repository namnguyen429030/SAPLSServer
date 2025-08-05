using SAPLSServer.DTOs.Base;
using System.ComponentModel.DataAnnotations;
using SAPLSServer.Constants;

namespace SAPLSServer.DTOs.Concrete.VehicleDto
{
    public class UpdateVehicleRequest : UpdateRequest
    {
        [Required(ErrorMessage = MessageKeys.LICENSE_PLATE_REQUIRED)]
        [StringLength(20, MinimumLength = 1, ErrorMessage = MessageKeys.LICENSE_PLATE_LENGTH)]
        public string LicensePlate { get; set; } = null!;
        [Required(ErrorMessage = MessageKeys.BRAND_REQUIRED)]
        [StringLength(100, MinimumLength = 1, ErrorMessage = MessageKeys.BRAND_LENGTH)]
        public string Brand { get; set; } = null!;
        [Required(ErrorMessage = MessageKeys.MODEL_REQUIRED)]
        [StringLength(100, MinimumLength = 1, ErrorMessage = MessageKeys.MODEL_LENGTH)]
        public string Model { get; set; } = null!;
        [Required(ErrorMessage = MessageKeys.ENGINE_NUMBER_REQUIRED)]
        [StringLength(50, MinimumLength = 1, ErrorMessage = MessageKeys.ENGINE_NUMBER_LENGTH)]
        public string EngineNumber { get; set; } = null!;
        [Required(ErrorMessage = MessageKeys.CHASSIS_NUMBER_REQUIRED)]
        [StringLength(50, MinimumLength = 1, ErrorMessage = MessageKeys.CHASSIS_NUMBER_LENGTH)]
        public string ChassisNumber { get; set; } = null!;
        [Required(ErrorMessage = MessageKeys.COLOR_REQUIRED)]
        [StringLength(50, MinimumLength = 1, ErrorMessage = MessageKeys.COLOR_LENGTH)]
        public string Color { get; set; } = null!;
        [Required(ErrorMessage = MessageKeys.OWNER_FULL_NAME_REQUIRED)]
        [StringLength(100, MinimumLength = 1, ErrorMessage = MessageKeys.OWNER_FULL_NAME_LENGTH)]
        public string OwnerVehicleFullName { get; set; } = null!;
    }
}

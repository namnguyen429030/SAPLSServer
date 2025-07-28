using SAPLSServer.DTOs.Base;
using System.ComponentModel.DataAnnotations;

namespace SAPLSServer.DTOs.Concrete
{
    public class CreateVehicleRequest : CreateRequest
    {
        [Required(ErrorMessage = "License plate number is required.")]
        public string LicensePlate { get; set; } = null!;
        [Required(ErrorMessage = "Brand is required.")]
        public string Brand { get; set; } = null!;
        [Required(ErrorMessage = "Model is required.")]
        public string Model { get; set; } = null!;
        [Required(ErrorMessage = "Engine number is required.")]
        public string EngineNumber { get; set; } = null!;
        [Required(ErrorMessage = "Chassis number is required.")]
        public string ChassisNumber { get; set; } = null!;
        [Required(ErrorMessage = "Color is required.")]
        public string Color { get; set; } = null!;
        [Required(ErrorMessage = "Full name of vehicle's owner is required.")]
        public string OwnerVehicleFullName { get; set; } = null!;
    }
}

using SAPLSServer.DTOs.Base;
using System.ComponentModel.DataAnnotations;

namespace SAPLSServer.DTOs.Concrete.SharedVehicle
{
    public class CreateSharedVehicleRequest : CreateRequest
    {
        [Required(ErrorMessage = "Vehicle ID is required.")]
        public string VehicleId { get; set; } = null!;
        public int? AccessDuration { get; set; }
        public string? Note { get; set; }
        [Required(ErrorMessage = "Shared Person ID is required.")]
        public string SharedPersonId { get; set; } = null!;
    }
}

using SAPLSServer.DTOs.Base;
using System.ComponentModel.DataAnnotations;

namespace SAPLSServer.DTOs.Concrete.Vehicle
{
    public class UpdateVehicleStatusRequest : UpdateRequest
    {
        [Required(ErrorMessage = "Vehicle status is required.")]
        public string Status { get; set; } = null!;
    }
}

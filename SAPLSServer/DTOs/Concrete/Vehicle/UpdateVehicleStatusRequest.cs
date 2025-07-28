using SAPLSServer.DTOs.Base;
using System.ComponentModel.DataAnnotations;

namespace SAPLSServer.DTOs.Concrete
{
    public class UpdateVehicleStatusRequest : UpdateRequest
    {
        [Required(ErrorMessage = "Vehicle status is required.")]
        public string Status { get; set; } = null!;
    }
}

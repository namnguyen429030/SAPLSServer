using SAPLSServer.DTOs.Base;
using System.ComponentModel.DataAnnotations;

namespace SAPLSServer.DTOs.Concrete.Vehicle
{
    public class UpdateVehicleStatusDto : UpdateDto
    {
        [Required(ErrorMessage = "Vehicle status is required.")]
        public string Status { get; set; } = null!;
    }
}

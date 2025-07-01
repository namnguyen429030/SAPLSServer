using SAPLSServer.DTOs.Base;
using System.ComponentModel.DataAnnotations;

namespace SAPLSServer.DTOs.Concrete.Vehicle
{
    public class UpdateVehicleSharingStatusDto : UpdateDto
    {
        [Required(ErrorMessage = "Sharing status is required.")]
        public string SharingStatus { get; set; } = null!;
    }
}

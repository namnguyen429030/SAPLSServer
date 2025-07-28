using SAPLSServer.DTOs.Base;
using System.ComponentModel.DataAnnotations;

namespace SAPLSServer.DTOs.Concrete
{
    public class UpdateVehicleSharingStatusRequest : UpdateRequest
    {
        [Required(ErrorMessage = "Sharing status is required.")]
        public string SharingStatus { get; set; } = null!;
    }
}

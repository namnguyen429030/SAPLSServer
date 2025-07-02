using SAPLSServer.DTOs.Base;
using System.ComponentModel.DataAnnotations;

namespace SAPLSServer.DTOs.Concrete.ParkingLot
{
    public class UpdateParkingLotBasicInformationRequest : UpdateRequest
    {
        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        [Required(ErrorMessage = "Total number of parking slots is required.")]
        public int TotalParkingSlot { get; set; }
        [Required(ErrorMessage = "Settings are required.")]
        public string Settings { get; set; } = null!;
        [Required(ErrorMessage = "Status is required.")]
        public string Status { get; set; } = null!;

    }
}

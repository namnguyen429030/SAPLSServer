using SAPLSServer.DTOs.Base;
using System.ComponentModel.DataAnnotations;

namespace SAPLSServer.DTOs.Concrete.ParkingLot
{
    public class CreateParkingLotDto : CreateDto
    {
        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; } = null!;

        public string? Description { get; set; }
        [Required(ErrorMessage = "Address is required.")]
        public string Address { get; set; } = null!;
        [Required(ErrorMessage = "Total parking slot is required.")]
        public int TotalParkingSlot { get; set; }
    }
}

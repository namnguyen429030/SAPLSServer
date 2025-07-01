using SAPLSServer.DTOs.Base;
using System.ComponentModel.DataAnnotations;

namespace SAPLSServer.DTOs.Concrete.ShiftDiary
{
    public class CreateShiftDiaryDto : CreateDto
    {
        [Required(ErrorMessage = "Header is required.")]
        public string Header { get; set; } = null!;
        [Required(ErrorMessage = "Body is required.")]
        public string Body { get; set; } = null!;
        [Required(ErrorMessage = "Parking lot ID is required.")]
        public string ParkingLotId { get; set; } = null!;
        [Required(ErrorMessage = "Sender ID is required.")]
        public string SenderId { get; set; } = null!;
    }
}

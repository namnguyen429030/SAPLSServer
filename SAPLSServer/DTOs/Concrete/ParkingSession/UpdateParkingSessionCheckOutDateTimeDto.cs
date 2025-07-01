using SAPLSServer.DTOs.Base;
using System.ComponentModel.DataAnnotations;

namespace SAPLSServer.DTOs.Concrete.ParkingSession
{
    public class UpdateParkingSessionCheckOutDateTimeDto : UpdateDto
    {
        [Required(ErrorMessage = "Check-out date and time is required.")]
        public DateTime CheckOutDateTime { get; set; }
    }
}

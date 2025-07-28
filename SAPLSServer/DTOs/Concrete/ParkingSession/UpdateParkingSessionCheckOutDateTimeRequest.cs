using SAPLSServer.DTOs.Base;
using System.ComponentModel.DataAnnotations;

namespace SAPLSServer.DTOs.Concrete
{
    public class UpdateParkingSessionCheckOutDateTimeRequest : UpdateRequest
    {
        [Required(ErrorMessage = "Check-out date and time is required.")]
        public DateTime CheckOutDateTime { get; set; }
    }
}

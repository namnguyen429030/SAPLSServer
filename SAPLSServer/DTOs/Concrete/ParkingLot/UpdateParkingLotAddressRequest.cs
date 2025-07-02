using SAPLSServer.DTOs.Base;
using System.ComponentModel.DataAnnotations;

namespace SAPLSServer.DTOs.Concrete.ParkingLot
{
    public class UpdateParkingLotAddressRequest : UpdateRequest
    {
        [Required(ErrorMessage = "Address is required.")]
        public string Address { get; set; } = null!;
    }
}

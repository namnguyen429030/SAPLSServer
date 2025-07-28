using SAPLSServer.DTOs.Base;
using System.ComponentModel.DataAnnotations;

namespace SAPLSServer.DTOs.Concrete
{
    public class CreatePaymenSourceRequest : CreateRequest
    {
        [Required(ErrorMessage = "Bank name is required.")]
        public string BankName { get; set; } = null!;
        [Required(ErrorMessage = "Account name is required.")]
        public string AccountName { get; set; } = null!;
        [Required(ErrorMessage = "Account number is required.")]
        public string AccountNumber { get; set; } = null!;
        [Required(ErrorMessage = "Parking lot owner id is required.")]
        public string ParkingLotOwnerId { get; set; } = null!;
    }
}

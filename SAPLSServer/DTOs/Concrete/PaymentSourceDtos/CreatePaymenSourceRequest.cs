using SAPLSServer.DTOs.Base;
using System.ComponentModel.DataAnnotations;
using SAPLSServer.Constants;

namespace SAPLSServer.DTOs.Concrete.PaymentSourceDtos
{
    public class CreatePaymenSourceRequest
    {
        [Required(ErrorMessage = MessageKeys.BANK_NAME_REQUIRED)]
        public string BankName { get; set; } = null!;
        [Required(ErrorMessage = MessageKeys.ACCOUNT_NAME_REQUIRED)]
        public string AccountName { get; set; } = null!;
        [Required(ErrorMessage = MessageKeys.ACCOUNT_NUMBER_REQUIRED)]
        public string AccountNumber { get; set; } = null!;
        [Required(ErrorMessage = MessageKeys.PARKING_LOT_OWNER_ID_REQUIRED)]
        public string ParkingLotOwnerId { get; set; } = null!;
    }
}

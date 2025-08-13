using SAPLSServer.DTOs.Base;
using System.ComponentModel.DataAnnotations;
using SAPLSServer.Constants;

namespace SAPLSServer.DTOs.Concrete.PaymentSourceDtos
{
    public class UpdatePaymentSourceRequest : UpdateRequest
    {
        [Required(ErrorMessage = MessageKeys.BANK_NAME_REQUIRED)]
        public string BankName { get; set; } = null!;
        [Required(ErrorMessage = MessageKeys.ACCOUNT_NAME_REQUIRED)]
        public string AccountName { get; set; } = null!;
        [Required(ErrorMessage = MessageKeys.ACCOUNT_NUMBER_REQUIRED)]
        public string AccountNumber { get; set; } = null!;
        [Required(ErrorMessage = MessageKeys.PARKING_LOT_OWNER_ID_REQUIRED)]
        public string ParkingLotOwnerId { get; set; } = null!;
        [Required(ErrorMessage = MessageKeys.STATUS_REQUIRED)]
        [EnumDataType(typeof(PaymentSourceStatus), ErrorMessage = MessageKeys.INVALID_PAYMENT_SOURCE_STATUS)]
        public string? Status { get; set; } = null!;
    }
}

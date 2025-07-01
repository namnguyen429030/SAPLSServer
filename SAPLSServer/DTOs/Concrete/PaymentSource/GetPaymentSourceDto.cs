using SAPLSServer.DTOs.Base;
using System.ComponentModel.DataAnnotations;

namespace SAPLSServer.DTOs.Concrete.PaymentSource
{
    public class GetPaymentSourceDto : GetDto
    {
        public string BankName { get; set; } = null!;
        public string AccountName { get; set; } = null!;
        public string AccountNumber { get; set; } = null!;
        public string ParkingLotOwnerId { get; set; } = null!;
    }
}

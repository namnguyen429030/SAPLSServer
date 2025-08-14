using SAPLSServer.DTOs.Base;
using SAPLSServer.Models;

namespace SAPLSServer.DTOs.Concrete.PaymentSourceDtos
{
    public class GetPaymentSourceDto : GetResult
    {
        public string BankName { get; set; }
        public string AccountName { get; set; }
        public string AccountNumber { get; set; }
        public string ParkingLotOwnerId { get; set; }
        public string Status { get; set; }
        public GetPaymentSourceDto(PaymentSource paymentSource)
        {
            Id = paymentSource.Id;
            BankName = paymentSource.BankName;
            AccountName = paymentSource.AccountName;
            AccountNumber = paymentSource.AccountNumber;
            ParkingLotOwnerId = paymentSource.ParkingLotOwnerId;
            Status = paymentSource.Status;
        }
    }
}

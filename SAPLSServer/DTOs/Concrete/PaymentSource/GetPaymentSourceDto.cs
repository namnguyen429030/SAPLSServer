using SAPLSServer.DTOs.Base;

namespace SAPLSServer.DTOs.Concrete
{
    public class GetPaymentSourceDto : GetResult
    {
        public string BankName { get; set; } = null!;
        public string AccountName { get; set; } = null!;
        public string AccountNumber { get; set; } = null!;
        public string ParkingLotOwnerId { get; set; } = null!;
    }
}

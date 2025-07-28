using SAPLSServer.DTOs.Base;

namespace SAPLSServer.DTOs.Concrete
{
    public class UpdateWhitelistAttendantExpireDateRequest
    {
        public string ParkingLotId { get; set; } = null!;

        public string ClientId { get; set; } = null!;
        public DateTime? ExpiredDate { get; set; }
    }
}

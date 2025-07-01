using SAPLSServer.DTOs.Base;

namespace SAPLSServer.DTOs.Concrete.Whitelist
{
    public class WhitelistAttendantDto : GetDto
    {
        public string Email { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public DateTime AddedDate { get; set; }
        public DateTime? ExpiredDate { get; set; }
    }
}

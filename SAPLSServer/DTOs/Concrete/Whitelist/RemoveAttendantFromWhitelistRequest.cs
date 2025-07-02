namespace SAPLSServer.DTOs.Concrete.Whitelist
{
    public class RemoveAttendantFromWhitelistRequest
    {
        public string ParkingLotId { get; set; } = null!;

        public string ClientId { get; set; } = null!;
    }
}

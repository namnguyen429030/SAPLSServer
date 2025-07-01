namespace SAPLSServer.DTOs.Concrete.Whitelist
{
    public class RemoveAttendantFromWhitelistDto
    {
        public string ParkingLotId { get; set; } = null!;

        public string ClientId { get; set; } = null!;
    }
}

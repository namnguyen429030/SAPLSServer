namespace SAPLSServer.DTOs.Concrete.Whitelist
{
    public class AddAttendantToWhitelistRequest
    {
        public string ParkingLotId { get; set; } = null!;

        public string ClientId { get; set; } = null!;
    }
}

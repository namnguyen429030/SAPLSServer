namespace SAPLSServer.DTOs.Concrete
{
    public class RemoveAttendantFromWhiteListRequest
    {
        public string ParkingLotId { get; set; } = null!;

        public string ClientId { get; set; } = null!;
    }
}

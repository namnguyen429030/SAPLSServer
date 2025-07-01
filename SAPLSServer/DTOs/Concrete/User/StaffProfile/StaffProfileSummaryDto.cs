namespace SAPLSServer.DTOs.Concrete.User
{
    public class StaffProfileSummaryDto : UserSummaryDto
    {
        public string ParkingLotOwnerId { get; set; } = null!;
    }
}

using SAPLSServer.Models;

namespace SAPLSServer.DTOs.Concrete.UserDtos
{
    public class StaffProfileSummaryDto : UserSummaryDto
    {
        public string ParkingLotId { get; set; }

        public StaffProfileSummaryDto(StaffProfile staff) : base(staff.User)
        {
            ParkingLotId = staff.ParkingLotId;
        }
    }
}

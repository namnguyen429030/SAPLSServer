using SAPLSServer.Models;

namespace SAPLSServer.DTOs.Concrete
{
    public class StaffProfileSummaryDto : UserSummaryDto
    {
        public string ParkingLotOwnerId { get; set; }

        public StaffProfileSummaryDto(StaffProfile model)
        {
            Id = model.UserId;
            Email = model.User.Email;
            FullName = model.User.FullName;
            CreatedAt = model.User.CreatedAt;
            Status = model.User.Status;
            ParkingLotOwnerId = model.ParkingLotId;
        }
    }
}

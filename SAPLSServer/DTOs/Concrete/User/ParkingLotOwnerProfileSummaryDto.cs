using SAPLSServer.DTOs.Base;
using SAPLSServer.Models;

namespace SAPLSServer.DTOs.Concrete
{
    public class ParkingLotOwnerProfileSummaryDto : UserSummaryDto
    {
        public string ParkingLotOwnerId { get; set; } = null!;

        public ParkingLotOwnerProfileSummaryDto() { }

        public ParkingLotOwnerProfileSummaryDto(ParkingLotOwnerProfile model)
        {
            Id = model.UserId;
            Email = model.User.Email;
            FullName = model.User.FullName;
            CreatedAt = model.User.CreatedAt;
            Status = model.User.Status;
            ParkingLotOwnerId = model.ParkingLotOwnerId;
        }
    }
}

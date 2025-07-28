using SAPLSServer.Models;

namespace SAPLSServer.DTOs.Concrete
{
    public class ParkingLotOwnerProfileDetailsDto : UserDetailsDto
    {
        public string ParkingLotOwnerId { get; set; } = null!;

        public ParkingLotOwnerProfileDetailsDto() { }

        public ParkingLotOwnerProfileDetailsDto(ParkingLotOwnerProfile model)
        {
            Id = model.UserId;
            Email = model.User.Email;
            FullName = model.User.FullName;
            CreatedAt = model.User.CreatedAt;
            Status = model.User.Status;
            ProfileImageUrl = model.User.ProfileImageUrl;
            Phone = model.User.Phone;
            UpdatedAt = model.User.UpdatedAt;
            ParkingLotOwnerId = model.ParkingLotOwnerId;
        }
    }
}

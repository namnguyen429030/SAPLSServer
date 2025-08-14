using SAPLSServer.Models;

namespace SAPLSServer.DTOs.Concrete.UserDtos
{
    public class StaffProfileDetailsDto : UserDetailsDto
    {
        public string ParkingLotId { get; set; }

        public StaffProfileDetailsDto(StaffProfile staff) : base(staff.User)
        {
            ParkingLotId = staff.ParkingLotId;
        }
    }
}

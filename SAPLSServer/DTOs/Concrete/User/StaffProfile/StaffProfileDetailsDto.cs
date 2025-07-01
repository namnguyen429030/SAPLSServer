using System.ComponentModel.DataAnnotations;

namespace SAPLSServer.DTOs.Concrete.User
{
    public class StaffProfileDetailsDto : UserDetailsDto
    {
        public string ParkingLotOwnerId { get; set; } = null!;
    }
}

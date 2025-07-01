using SAPLSServer.DTOs.Base;
using SAPLSServer.DTOs.Concrete.Vehicle;

namespace SAPLSServer.DTOs.Concrete.SharedVehicle
{
    public class SharedVehicleDetailsDto : VehicleDetailsDto
    {
        public int? AccessDuration { get; set; }

        public DateTime InviteAt { get; set; }

        public DateTime? ExpirationDate { get; set; }
        public string OwnerName { get; set; } = null!;
        public string? Note { get; set; }
    }
}

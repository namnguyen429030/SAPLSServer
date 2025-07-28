using SAPLSServer.Models;

namespace SAPLSServer.DTOs.Concrete
{
    public class SharedVehicleSummaryDto : VehicleSummaryDto
    {
        public int? AccessDuration { get; set; }
        public string OwnerName { get; set; } = null!;
        public SharedVehicleSummaryDto(SharedVehicle sharedVehicle) : base(sharedVehicle.Vehicle)
        {
            AccessDuration = sharedVehicle.AccessDuration;
            OwnerName = sharedVehicle.Vehicle.Owner.User.FullName ?? string.Empty;
        }
    }
}

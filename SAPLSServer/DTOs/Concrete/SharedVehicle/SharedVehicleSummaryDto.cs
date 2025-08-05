using SAPLSServer.DTOs.Concrete.VehicleDto;
using SAPLSServer.Models;

namespace SAPLSServer.DTOs.Concrete.SharedVehicleDto
{
    public class SharedVehicleSummaryDto : VehicleSummaryDto
    {
        public int? AccessDuration { get; set; }
        public string OwnerName { get; set; }
        public SharedVehicleSummaryDto(SharedVehicle sharedVehicle) : base(sharedVehicle.Vehicle)
        {
            AccessDuration = sharedVehicle.AccessDuration;
            OwnerName = sharedVehicle.Vehicle.Owner.User.FullName;
        }
    }
}

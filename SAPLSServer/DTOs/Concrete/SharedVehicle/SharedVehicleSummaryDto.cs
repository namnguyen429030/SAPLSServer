using SAPLSServer.DTOs.Base;
using SAPLSServer.DTOs.Concrete.Vehicle;

namespace SAPLSServer.DTOs.Concrete.SharedVehicle
{
    public class SharedVehicleSummaryDto : VehicleSummaryDto
    {
        public int? AccessDuration { get; set; }
        public string OwnerName { get; set; } = null!;
    }
}

using SAPLSServer.DTOs.Base;

namespace SAPLSServer.DTOs.Concrete.Vehicle
{
    public class VehicleDetailsDto : VehicleSummaryDto
    {
        public string EngineNumber { get; set; } = null!;

        public string ChassisNumber { get; set; } = null!;
        public string OwnerVehicleFullName { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}

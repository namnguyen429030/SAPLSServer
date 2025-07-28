using SAPLSServer.Models;

namespace SAPLSServer.DTOs.Concrete
{
    public class VehicleDetailsDto : VehicleSummaryDto
    {
        public string EngineNumber { get; set; }

        public string ChassisNumber { get; set; }
        public string OwnerVehicleFullName { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public VehicleDetailsDto(Vehicle vehicle) : base(vehicle)
        {
            EngineNumber = vehicle.EngineNumber;
            ChassisNumber = vehicle.ChassisNumber;
            OwnerVehicleFullName = vehicle.OwnerVehicleFullName;
            CreatedAt = vehicle.CreatedAt;
            UpdatedAt = vehicle.UpdatedAt;
        }
    }
}

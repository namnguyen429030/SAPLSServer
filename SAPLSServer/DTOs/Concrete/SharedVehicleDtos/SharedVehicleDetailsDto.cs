using SAPLSServer.DTOs.Concrete.VehicleDtos;
using SAPLSServer.Models;

namespace SAPLSServer.DTOs.Concrete.SharedVehicleDtos
{
    public class SharedVehicleDetailsDto : VehicleDetailsDto
    {
        public int? AccessDuration { get; set; }

        public DateTime InviteAt { get; set; }

        public DateTime? ExpirationDate { get; set; }
        public string OwnerName { get; set; }
        public string? Note { get; set; }
        public SharedVehicleDetailsDto(SharedVehicle sharedVehicle) : base(sharedVehicle.Vehicle)
        {
            AccessDuration = sharedVehicle.AccessDuration;
            InviteAt = sharedVehicle.InviteAt;
            ExpirationDate = sharedVehicle.ExpireAt;
            OwnerName = sharedVehicle.Vehicle.OwnerVehicleFullName;
        }
    }
}

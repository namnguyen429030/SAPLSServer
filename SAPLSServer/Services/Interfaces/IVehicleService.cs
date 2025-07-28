using SAPLSServer.DTOs.Base;
using SAPLSServer.DTOs.Concrete;

namespace SAPLSServer.Services.Interfaces
{
    public interface IVehicleService
    {
        Task CreateVehicle(CreateVehicleRequest dto);
        Task UpdateVehicle(UpdateVehicleRequest dto);
        Task UpdateVehicleSharingStatus(UpdateVehicleSharingStatusRequest dto);
        Task UpdateVehicleStatus(UpdateVehicleStatusRequest dto);
        Task<VehicleDetailsDto?> GetVehicleDetails(string id);
        Task<PageResult<VehicleSummaryDto>> GetVehiclesPage(PageRequest request);
        Task<PageResult<VehicleSummaryDto>> GetVehiclesByOwnerPage(string ownerId, PageRequest request);
        Task<PageResult<VehicleSummaryDto>> GetVehiclesByLicensePlatePage(string licensePlate, PageRequest request);
        Task<PageResult<VehicleSummaryDto>> GetVehiclesByStatusPage(string status, PageRequest request);
        Task DeleteVehicle(DeleteRequest dto);
    }
}


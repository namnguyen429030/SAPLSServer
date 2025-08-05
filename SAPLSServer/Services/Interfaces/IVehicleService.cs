using SAPLSServer.DTOs.Base;
using SAPLSServer.DTOs.Concrete.VehicleDto;
using SAPLSServer.DTOs.PaginationDto;

namespace SAPLSServer.Services.Interfaces
{
    public interface IVehicleService
    {
        Task CreateVehicle(CreateVehicleRequest request);
        Task UpdateVehicle(UpdateVehicleRequest request);
        Task UpdateVehicleStatus(UpdateVehicleStatusRequest request);
        Task<VehicleDetailsDto?> GetVehicleDetails(GetDetailsRequest request);
        Task<PageResult<VehicleSummaryDto>> GetVehiclesPage(PageRequest pageRequest, GetVehicleListRequest request);
        Task DeleteVehicle(DeleteRequest request);
    }
}


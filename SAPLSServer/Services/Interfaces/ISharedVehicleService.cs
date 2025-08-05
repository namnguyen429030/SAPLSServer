using SAPLSServer.DTOs.Base;
using SAPLSServer.DTOs.Concrete.SharedVehicleDto;
using SAPLSServer.DTOs.PaginationDto;

namespace SAPLSServer.Services.Interfaces
{
    public interface ISharedVehicleService
    {
        /// <summary>
        /// Creates a new shared vehicle with the provided details.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task CreateSharedVehicle(CreateSharedVehicleRequest request);
        /// <summary>
        /// Retrieves the details of a shared vehicle by its unique identifier (ID).
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<SharedVehicleDetailsDto?> GetSharedVehicleDetails(GetDetailsRequest request);
        /// <summary>
        /// Retrieves a paginated list of shared vehicles based on the provided pageRequest and filter criteria.
        /// </summary>
        /// <param name="pageRequest"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<PageResult<SharedVehicleSummaryDto>> GetSharedVehiclesPage(PageRequest pageRequest, GetSharedVehicleList request);
        /// <summary>
        /// Updates the sharing status of a vehicle based on the provided request.
        /// </summary>
        /// <param name="request">The request containing the details required to update the vehicle's sharing status. This cannot be null.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task UpdateVehicleSharingStatus(UpdateVehicleSharingStatusRequest request);
    }
}


using SAPLSServer.DTOs.Base;
using SAPLSServer.DTOs.Concrete;

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
        Task<PageResult<SharedVehicleSummaryDto>> GetSharedVehiclesPage(PageRequest pageRequest, GetListRequest request);
        /// <summary>
        /// Retrieves a paginated list of shared vehicles owned by a specific owner, identified by their unique identifier (ownerId).
        /// </summary>
        /// <param name="ownerId"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<PageResult<SharedVehicleSummaryDto>> GetSharedVehiclesByOwnerPage(string ownerId, PageRequest request);
        /// <summary>
        /// Retrieves a paginated list of shared vehicles associated with a specific shared person, identified by their unique identifier (sharedPersonId).
        /// </summary>
        /// <param name="sharedPersonId"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<PageResult<SharedVehicleSummaryDto>> GetSharedVehiclesBySharedPersonPage(string sharedPersonId, PageRequest request);
    }
}


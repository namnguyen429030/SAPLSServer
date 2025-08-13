using SAPLSServer.DTOs.Base;
using SAPLSServer.DTOs.Concrete.UserDtos;
using SAPLSServer.DTOs.PaginationDto;

namespace SAPLSServer.Services.Interfaces
{
    /// <summary>
    /// Provides operations for managing parking lot owner profiles.
    /// </summary>
    public interface IParkingLotOwnerService
    {
        /// <summary>
        /// Creates a new parking lot owner profile.
        /// </summary>
        /// <param name="request">The request containing parking lot owner profile details.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task Create(CreateParkingLotOwnerProfileRequest request);

        /// <summary>
        /// Updates an existing parking lot owner profile.
        /// </summary>
        /// <param name="request">The request containing updated parking lot owner profile details.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task Update(UpdateParkingLotOwnerProfileRequest request);

        /// <summary>
        /// Retrieves detailed information about a parking lot owner by their parking lot owner identifier.
        /// </summary>
        /// <param name="parkingLotOwnerId">The unique identifier of the parking lot owner.</param>
        /// <returns>A task that returns the parking lot owner's detailed information.</returns>
        Task<ParkingLotOwnerProfileDetailsDto?> GetByParkingLotOwnerId(string parkingLotOwnerId);

        /// <summary>
        /// Retrieves detailed information about a parking lot owner by their user identifier.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <returns>A task that returns the parking lot owner's detailed information.</returns>
        Task<ParkingLotOwnerProfileDetailsDto?> GetByUserId(string userId);

        /// <summary>
        /// Retrieves a paginated list of parking lot owner profiles with optional search criteria.
        /// </summary>
        /// <param name="pageRequest">The pagination request containing page number and size.</param>
        /// <param name="request">The search/filter criteria for parking lot owner profiles.</param>
        /// <returns>A paginated result of parking lot owner profile details.</returns>
        Task<PageResult<ParkingLotOwnerProfileSummaryDto>> GetParkingLotOwnerProfilesPage(PageRequest pageRequest, GetParkingLotOwnerListRequest request);

        /// <summary>
        /// Retrieves a list of parking lot owner profiles with optional search criteria.
        /// </summary>
        /// <param name="request">The search/filter criteria for parking lot owner profiles.</param>
        /// <returns>A task that returns a list of parking lot owner profile summaries.</returns>
        Task<List<ParkingLotOwnerProfileSummaryDto>> GetParkingLotOwnerProfiles(GetParkingLotOwnerListRequest request);
    }
}
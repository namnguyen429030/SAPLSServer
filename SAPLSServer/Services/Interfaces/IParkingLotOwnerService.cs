using SAPLSServer.DTOs.Base;
using SAPLSServer.DTOs.Concrete.UserDto;
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
        Task CreateParkingLotOwner(CreateParkingLotOwnerProfileRequest request);

        /// <summary>
        /// Updates an existing parking lot owner profile.
        /// </summary>
        /// <param name="request">The request containing updated parking lot owner profile details.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task UpdateParkingLotOwner(UpdateParkingLotOwnerProfileRequest request);

        /// <summary>
        /// Retrieves the details of a parking lot owner profile.
        /// </summary>
        /// <param name="request">The request containing the parking lot owner profile identifier.</param>
        /// <returns>The parking lot owner profile details if found; otherwise, <see langword="null"/>.</returns>
        Task<ParkingLotOwnerProfileDetailsDto?> GetParkingLotOwnerProfileDetails(GetDetailsRequest request);

        /// <summary>
        /// Retrieves a paginated list of parking lot owner profiles with optional search criteria.
        /// </summary>
        /// <param name="pageRequest">The pagination request.</param>
        /// <param name="request">The search/filter criteria.</param>
        /// <returns>A paginated result of parking lot owner profile details.</returns>
        Task<PageResult<ParkingLotOwnerProfileSummaryDto>> GetParkingLotOwnerProfilesPage(PageRequest pageRequest, GetParkingLotOwnerListRequest request);
    }
}
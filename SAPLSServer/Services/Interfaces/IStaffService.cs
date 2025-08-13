using SAPLSServer.DTOs.Concrete;
using SAPLSServer.DTOs.Concrete.UserDtos;
using SAPLSServer.DTOs.PaginationDto;

namespace SAPLSServer.Services.Interfaces
{
    /// <summary>
    /// Provides operations for managing staff profiles.
    /// </summary>
    public interface IStaffService
    {
        /// <summary>
        /// Creates a new staff profile.
        /// </summary>
        /// <param name="request">The request containing staff profile details.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task Create(CreateStaffProfileRequest request);

        /// <summary>
        /// Updates an existing staff profile.
        /// </summary>
        /// <param name="request">The request containing updated staff profile details.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task Update(UpdateStaffProfileRequest request);

        /// <summary>
        /// Retrieves detailed information about a staff member by their staff identifier.
        /// </summary>
        /// <param name="staffId">The unique identifier of the staff member.</param>
        /// <returns>A task that returns the staff member's detailed information.</returns>
        Task<StaffProfileDetailsDto?> FindByStaffId(string staffId);

        /// <summary>
        /// Retrieves detailed information about a staff member by their user identifier.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <returns>A task that returns the staff member's detailed information.</returns>
        Task<StaffProfileDetailsDto?> FindByUserId(string userId);

        /// <summary>
        /// Retrieves a paginated list of staff profiles with optional search criteria.
        /// </summary>
        /// <param name="pageRequest">The pagination request containing page number and size.</param>
        /// <param name="request">The optional search criteria for filtering staff profiles.</param>
        /// <returns>A task representing the asynchronous operation, with a paginated result of staff profile details.</returns>
        Task<PageResult<StaffProfileSummaryDto>> GetStaffProfilesPage(PageRequest pageRequest, GetStaffListRequest request);

        /// <summary>
        /// Retrieves a list of staff profiles with optional search criteria.
        /// </summary>
        /// <param name="request">The optional search criteria for filtering staff profiles.</param>
        /// <returns>A task that returns a list of staff profile summaries.</returns>
        Task<List<StaffProfileSummaryDto>> GetStaffProfiles(GetStaffListRequest request);

        /// <summary>
        /// Retrieves the parking lot ID associated with a staff member by their user ID or staff ID.
        /// </summary>
        /// <param name="userIdOrstaffId">The user ID or staff ID to identify the staff member.</param>
        /// <returns>A task that returns the parking lot ID associated with the staff member.</returns>
        Task<string> GetParkingLotId(string userIdOrstaffId);
    }
}
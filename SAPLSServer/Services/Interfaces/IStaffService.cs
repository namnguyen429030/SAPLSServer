using SAPLSServer.DTOs.Concrete;
using SAPLSServer.DTOs.Concrete.UserDto;
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
        Task CreateStaff(CreateStaffProfileRequest request);

        /// <summary>
        /// Updates an existing staff profile.
        /// </summary>
        /// <param name="request">The request containing updated staff profile details.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task UpdateStaff(UpdateStaffProfileRequest request);

        /// <summary>
        /// Retrieves the details of a staff profile by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the staff profile.</param>
        /// <returns>The staff profile details if found; otherwise, <see langword="null"/>.</returns>
        Task<StaffProfileDetailsDto?> GetStaffProfileDetails(string id);
        /// <summary>
        /// Retrieves a paginated list of staff profiles with optional search criteria.
        /// </summary>
        /// <param name="pageRequest">The pagination request.</param>
        /// <param name="request">The optional search criteria.</param>
        /// <returns>A task representing the asynchronous operation, with a paginated result of staff profile details.</returns>
        Task<PageResult<StaffProfileSummaryDto>> GetStaffProfilesPage(PageRequest pageRequest, GetStaffListRequest request);
    }
}
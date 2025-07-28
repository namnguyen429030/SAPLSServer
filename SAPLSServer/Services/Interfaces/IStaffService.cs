using SAPLSServer.DTOs.Concrete;

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
        /// Retrieves a paginated list of staff profiles for a parking lot with optional search criteria.
        /// </summary>
        /// <param name="request">The pagination and filter request.</param>
        /// <returns>A paginated result of staff profile details.</returns>
        Task<PageResult<StaffProfileDetailsDto>> GetStaffsPageByParkingLot(PageRequest request);
    }
}
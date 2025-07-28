using SAPLSServer.DTOs.Base;
using SAPLSServer.DTOs.Concrete;

namespace SAPLSServer.Services.Interfaces
{
    /// <summary>
    /// Provides operations for managing admin profiles.
    /// </summary>
    public interface IAdminService
    {
        /// <summary>
        /// Creates a new admin profile.
        /// </summary>
        /// <param name="request">The request containing admin profile details.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task CreateAdmin(CreateAdminProfileRequest request);

        /// <summary>
        /// Updates an existing admin profile.
        /// </summary>
        /// <param name="request">The request containing updated admin profile details.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task UpdateAdmin(UpdateAdminProfileRequest request);

        /// <summary>
        /// Retrieves the details of an admin profile.
        /// </summary>
        /// <param name="request">The request containing the admin profile identifier.</param>
        /// <returns>The admin profile details if found; otherwise, <see langword="null"/>.</returns>
        Task<AdminProfileDetailsDto?> GetAdminProfileDetails(GetDetailsRequest request);

        /// <summary>
        /// Retrieves a paginated list of admin profiles with optional search criteria.
        /// </summary>
        /// <param name="pageRequest">The pagination request.</param>
        /// <param name="request">The search/filter criteria.</param>
        /// <returns>A paginated result of admin profile details.</returns>
        Task<PageResult<AdminProfileDetailsDto>> GetAdminProfilesPage(PageRequest pageRequest, GetListRequest request);
    }
}
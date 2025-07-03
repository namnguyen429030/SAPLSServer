using SAPLSServer.DTOs.Base;
using SAPLSServer.DTOs.Concrete.Pagination;
using SAPLSServer.DTOs.Concrete.User;

namespace SAPLSServer.Services.Interfaces
{
    public interface IAdminProfileService
    {
        /// <summary>
        /// Creates a new admin profile.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task CreateAdmin(CreateAdminProfileRequest request);
        /// <summary>
        /// Updates an existing admin profile.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task UpdateAdmin(UpdateAdminProfileRequest request);
        /// <summary>
        /// Deletes an admin profile by its ID.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<AdminProfileDetailsDto?> GetAdminProfileDetails(GetDetailsRequest request);
        /// <summary>
        /// Get a paginated list of admin profiles with optional search criteria.
        /// </summary>
        /// <param name="pageRequest"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<PageResult<AdminProfileDetailsDto>> GetAdminProfilesPage(PageRequest pageRequest, GetListRequest request);
    }
}

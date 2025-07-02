using SAPLSServer.DTOs.Base;
using SAPLSServer.DTOs.Concrete.Pagination;
using SAPLSServer.DTOs.Concrete.User;

namespace SAPLSServer.Services.Interfaces
{
    public interface IClientProfileService
    {
        /// <summary>
        /// Creates a new client profile.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task CreateClient(CreateClientProfileRequest request);
        /// <summary>
        /// Updates an existing client profile.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task UpdateClient(UpdateClientProfileRequest request);
        /// <summary>
        /// Get client profile's details by request.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<ClientProfileDetailsDto?> GetClientProfileDetails(GetDetailsRequest request);
        /// <summary>
        /// Get a paginated list of client profiles with optional search criteria.
        /// </summary>
        /// <param name="pageRequest"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<PageResult<ClientProfileDetailsDto>> GetClientsPage(PageRequest pageRequest, GetListRequest request);
    }
}

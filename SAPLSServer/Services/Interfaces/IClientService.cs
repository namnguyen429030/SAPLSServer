using SAPLSServer.DTOs.Base;
using SAPLSServer.DTOs.Concrete;

namespace SAPLSServer.Services.Interfaces
{
    /// <summary>
    /// Provides operations for managing client profiles.
    /// </summary>
    public interface IClientService
    {
        /// <summary>
        /// Creates a new client profile.
        /// </summary>
        /// <param name="request">The request containing client profile details.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task CreateClient(CreateClientProfileRequest request);

        /// <summary>
        /// Updates an existing client profile.
        /// </summary>
        /// <param name="request">The request containing updated client profile details.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task UpdateClient(UpdateClientProfileRequest request);

        /// <summary>
        /// Retrieves the details of a client profile.
        /// </summary>
        /// <param name="request">The request containing the client profile identifier.</param>
        /// <returns>The client profile details if found; otherwise, <see langword="null"/>.</returns>
        Task<ClientProfileDetailsDto?> GetClientProfileDetails(GetDetailsRequest request);

        /// <summary>
        /// Retrieves a paginated list of client profiles with optional search criteria.
        /// </summary>
        /// <param name="pageRequest">The pagination request.</param>
        /// <param name="request">The search/filter criteria.</param>
        /// <returns>A paginated result of client profile details.</returns>
        Task<PageResult<ClientProfileDetailsDto>> GetClientProfilesPage(PageRequest pageRequest, GetListRequest request);
    }
}
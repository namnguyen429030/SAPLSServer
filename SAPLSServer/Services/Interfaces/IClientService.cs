using SAPLSServer.DTOs.Concrete.UserDtos;
using SAPLSServer.DTOs.PaginationDto;

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
        Task Create(CreateClientProfileRequest request);

        /// <summary>
        /// Updates an existing client profile.
        /// </summary>
        /// <param name="request">The request containing updated client profile details.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task Update(UpdateClientProfileRequest request, string updatePerformerId);

        /// <summary>
        /// Retrieves detailed information about a client by their citizen ID number.
        /// </summary>
        /// <param name="citizenIdNo">The citizen ID number of the client.</param>
        /// <returns>A task that returns the client's detailed information.</returns>
        Task<ClientProfileDetailsDto?> GetByCitizenIdNo(string citizenIdNo);

        /// <summary>
        /// Retrieves detailed information about a client by their user identifier.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <returns>A task that returns the client's detailed information.</returns>
        Task<ClientProfileDetailsDto?> GetByUserId(string userId);

        /// <summary>
        /// Retrieves a paginated list of client profiles with optional search criteria.
        /// </summary>
        /// <param name="pageRequest">The pagination request containing page number and size.</param>
        /// <param name="request">The search/filter criteria for client profiles.</param>
        /// <returns>A paginated result of client profile details.</returns>
        Task<PageResult<ClientProfileSummaryDto>> GetClientProfilesPage(PageRequest pageRequest, GetClientListRequest request);
        Task UpdateDeviceToken(string userId, string? deviceToken);
        Task<List<ClientProfileSummaryDto>> GetAllClients(GetClientListRequest request);
        Task<string?> GetDeviceToken(string userId);
    }
}
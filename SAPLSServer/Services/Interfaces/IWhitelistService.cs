using SAPLSServer.DTOs.Base;
using SAPLSServer.DTOs.Concrete;
using SAPLSServer.DTOs.PaginationDto;

namespace SAPLSServer.Services.Interfaces
{
    public interface IWhitelistService
    {
        /// <summary>
        /// Adds an attendant to the whitelist based on the provided request details.
        /// </summary>
        /// <param name="request">The request object containing the details of the attendant to be added to the whitelist. Cannot be null.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task AddAttendantToWhitelist(AddAttendantToWhiteListRequest request);
        /// <summary>
        /// Updates the expiration date for a whitelist attendant.
        /// </summary>
        /// <param name="request">The request object containing the details required to update the expiration date.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task UpdateWhitelistAttendantExpireDate(UpdateWhiteListAttendantExpireDateRequest request);
        /// <summary>
        /// Retrieves the details of a whitelist attendant based on the specified request.
        /// </summary>
        /// <param name="request">The request containing the identifier of the whitelist attendant to retrieve.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a <see
        /// cref="WhiteListAttendantDto"/> object with the attendant's details, or <see langword="null"/> if no
        /// attendant is found.</returns>
        Task<WhiteListAttendantDto?> GetWhitelistAttendantDetails(GetWhiteListAttendantRequest request);
        /// <summary>
        /// Retrieves a paginated list of whitelist attendants based on the specified criteria.
        /// </summary>
        /// <param name="request">The pagination and sorting information for the request.</param>
        /// <param name="listRequest">The criteria used to filter the whitelist attendants.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a <see
        /// cref="PageResult{WhiteListAttendantDto}"/> with the list of whitelist attendants matching the specified
        /// criteria.</returns>
        Task<PageResult<WhiteListAttendantDto>> GetWhitelistAttendantsPage(PageRequest request, GetWhiteListAttendantListRequest listRequest);
        /// <summary>
        /// Removes an attendant from the whitelist based on the specified request.
        /// </summary>
        /// <param name="request">The request containing the details of the attendant to be removed from the whitelist.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task RemoveAttendantFromWhitelist(RemoveAttendantFromWhiteListRequest request);
    }
}


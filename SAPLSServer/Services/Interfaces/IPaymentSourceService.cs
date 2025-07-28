using SAPLSServer.DTOs.Base;
using SAPLSServer.DTOs.Concrete;

namespace SAPLSServer.Services.Interfaces
{
    public interface IPaymentSourceService
    {
        /// <summary>
        /// Creates a new payment source with the provided details.
        /// </summary>
        /// <param name="request">Payment source creation request</param>
        /// <returns>A task representing the asynchronous operation</returns>
        Task CreatePaymentSource(CreatePaymenSourceRequest request);

        /// <summary>
        /// Updates an existing payment source with the provided details.
        /// </summary>
        /// <param name="request">Payment source update request</param>
        /// <returns>A task representing the asynchronous operation</returns>
        Task UpdatePaymentSource(UpdatePaymentSourceRequest request);

        /// <summary>
        /// Deletes a payment source by its unique identifier (ID).
        /// </summary>
        /// <param name="request">Delete request containing the payment source ID</param>
        /// <returns>A task representing the asynchronous operation</returns>
        Task DeletePaymentSource(DeleteRequest request);

        /// <summary>
        /// Retrieves the detailed information of a payment source by its unique identifier (ID).
        /// </summary>
        /// <param name="request">Get details request containing the payment source ID</param>
        /// <returns>Payment source details or null if not found</returns>
        Task<GetPaymentSourceDto?> GetPaymentSourceDetails(GetDetailsRequest request);

        /// <summary>
        /// Retrieves a paginated list of payment sources with optional search and filter criteria.
        /// </summary>
        /// <param name="pageRequest">Pagination parameters</param>
        /// <param name="request">List request with optional filtering and search criteria</param>
        /// <returns>Paginated result containing payment sources</returns>
        Task<PageResult<GetPaymentSourceDto>> GetPaymentSourcesPage(PageRequest pageRequest, GetListRequest request);

        /// <summary>
        /// Retrieves a list of payment sources owned by a specific parking lot owner.
        /// </summary>
        /// <param name="ownerId">Unique identifier of the parking lot owner</param>
        /// <returns>Collection of payment sources belonging to the specified owner</returns>
        Task<IEnumerable<GetPaymentSourceDto>> GetPaymentSourcesByOwner(string ownerId);

        /// <summary>
        /// Retrieves a paginated list of payment sources owned by a specific parking lot owner.
        /// </summary>
        /// <param name="ownerId">Unique identifier of the parking lot owner</param>
        /// <param name="pageRequest">Pagination parameters</param>
        /// <param name="request">List request with optional filtering and search criteria</param>
        /// <returns>Paginated result containing payment sources for the specified owner</returns>
        Task<PageResult<GetPaymentSourceDto>> GetPaymentSourcesByOwnerPage(string ownerId, PageRequest pageRequest, GetListRequest request);

        /// <summary>
        /// Retrieves a paginated list of payment sources filtered by their status.
        /// </summary>
        /// <param name="status">Status to filter payment sources by</param>
        /// <param name="pageRequest">Pagination parameters</param>
        /// <param name="request">List request with optional additional filtering criteria</param>
        /// <returns>Paginated result containing payment sources with the specified status</returns>
        Task<PageResult<GetPaymentSourceDto>> GetPaymentSourcesByStatusPage(string status, PageRequest pageRequest, GetListRequest request);
    }
}


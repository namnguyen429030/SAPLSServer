namespace SAPLSServer.Services.Interfaces {
    using SAPLSServer.DTOs.Base;
    using SAPLSServer.DTOs.Concrete.PaymentSourceDtos;
    using SAPLSServer.DTOs.PaginationDto;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// Provides methods for managing payment sources
    /// </summary>
    public interface IPaymentSourceService {
        /// <summary>
        /// Creates a new payment source
        /// </summary>
        /// <param name="request">Payment source creation request</param>
        Task CreatePaymentSource(CreatePaymenSourceRequest request);

        /// <summary>
        /// Updates an existing payment source
        /// </summary>
        /// <param name="request">Payment source update request</param>
        Task UpdatePaymentSource(UpdatePaymentSourceRequest request);

        /// <summary>
        /// Deletes a payment source
        /// </summary>
        /// <param name="request">Delete request with ID</param>
        Task DeletePaymentSource(DeleteRequest request);

        /// <summary>
        /// Gets details of a specific payment source
        /// </summary>
        /// <param name="request">Get details request with ID</param>
        /// <returns>Payment source details or null if not found</returns>
        Task<GetPaymentSourceDto?> GetPaymentSourceDetails(GetDetailsRequest request);

        /// <summary>
        /// Gets a paginated list of payment sources
        /// </summary>
        /// <param name="pageRequest">Pagination parameters</param>
        /// <param name="request">List filtering/sorting request</param>
        /// <returns>Paginated result of payment sources</returns>
        Task<PageResult<GetPaymentSourceDto>> GetPaymentSourcesPage(PageRequest pageRequest, GetListRequest request);

        /// <summary>
        /// Gets all payment sources for a specific parking lot owner
        /// </summary>
        /// <param name="ownerId">Parking lot owner ID</param>
        /// <returns>Collection of payment sources</returns>
        Task<IEnumerable<GetPaymentSourceDto>> GetPaymentSourcesByOwner(string ownerId);

        /// <summary>
        /// Gets a paginated list of payment sources for a specific parking lot owner
        /// </summary>
        /// <param name="ownerId">Parking lot owner ID</param>
        /// <param name="pageRequest">Pagination parameters</param>
        /// <param name="request">List filtering/sorting request</param>
        /// <returns>Paginated result of payment sources</returns>
        Task<PageResult<GetPaymentSourceDto>> GetPaymentSourcesByOwnerPage(string ownerId, PageRequest pageRequest, GetListRequest request);

        /// <summary>
        /// Gets a paginated list of payment sources filtered by status
        /// </summary>
        /// <param name="status">Payment source status (Active/Inactive)</param>
        /// <param name="pageRequest">Pagination parameters</param>
        /// <param name="request">List filtering/sorting request</param>
        /// <returns>Paginated result of payment sources</returns>
        Task<PageResult<GetPaymentSourceDto>> GetPaymentSourcesByStatusPage(string status, PageRequest pageRequest, GetListRequest request);
    }
}

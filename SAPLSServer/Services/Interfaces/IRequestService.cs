using SAPLSServer.DTOs.Base;
using SAPLSServer.DTOs.Concrete.Pagination;
using SAPLSServer.DTOs.Concrete.Request;

namespace SAPLSServer.Services.Interfaces
{
    public interface IRequestService
    {
        /// <summary>
        /// Creates a new pageRequest with the provided details.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task CreateRequest(CreateRequestRequest request);
        /// <summary>
        /// Updates an existing pageRequest with the provided details.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task UpdateRequest(UpdateRequestRequest request);
        /// <summary>
        /// Retrieves the details of a pageRequest by its unique identifier (ID).
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<RequestDetailsDto?> GetRequestDetails(GetDetailsRequest request);
        /// <summary>
        /// Retrieves a paginated list of requests based on the provided pageRequest and filter criteria.
        /// </summary>
        /// <param name="pageRequest"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<PageResult<RequestSummaryDto>> GetRequestsPage(PageRequest pageRequest, GetListRequest request);
        /// <summary>
        /// Retrieves a paginated list of requests filtered by type.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<PageResult<RequestSummaryDto>> GetRequestsByTypePage(string type, PageRequest request);
        /// <summary>
        /// Retrieves a paginated list of requests filtered by status.
        /// </summary>
        /// <param name="status"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<PageResult<RequestSummaryDto>> GetRequestsByStatusPage(string status, PageRequest request);
        /// <summary>
        /// Retrieves a paginated list of requests made by a specific staff member, identified by their unique identifier (staffId).
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<PageResult<RequestSummaryDto>> GetRequestsByDateRangePage(DateTime startDate, DateTime endDate, PageRequest request);
    }
}


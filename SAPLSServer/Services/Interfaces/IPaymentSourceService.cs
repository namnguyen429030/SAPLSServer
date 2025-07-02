using SAPLSServer.DTOs.Concrete.Pagination;
using SAPLSServer.DTOs.Concrete.PaymentSource;

namespace SAPLSServer.Services.Interfaces
{
    public interface IPaymentSourceService
    {
        /// <summary>
        /// Creates a new payment source with the provided details.
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task CreatePaymentSource(CreatePaymenSourceRequest request);
        /// <summary>
        /// Updates an existing payment source with the provided details.
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task UpdatePaymentSource(UpdatePaymentSourceRequest request);
        /// <summary>
        /// Retrieves a list of payment sources owned by a specific owner, identified by their unique identifier (ID).
        /// </summary>
        /// <param name="ownerId"></param>
        /// <returns></returns>
        Task<IEnumerable<GetPaymentSourceDto>> GetPaymentSourcesByOwner(string ownerId);
    }
}


using SAPLSServer.Models;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace SAPLSServer.Repositories.Interfaces
{
    public interface IPaymentSourceRepository : IRepository<PaymentSource, string>
    {
        /// <summary>
        /// L?y danh sách các ngu?n thanh toán theo ID c?a ch? bãi ?? xe
        /// </summary>
        /// <param name="ownerId">ID c?a ch? bãi ?? xe</param>
        /// <returns>Danh sách các ngu?n thanh toán</returns>
        Task<List<PaymentSource>> GetPaymentSourcesByOwnerIdAsync(string ownerId);
        
        /// <summary>
        /// Tìm ngu?n thanh toán theo ID, x? lý chuy?n ??i ki?u d? li?u
        /// </summary>
        /// <param name="id">ID c?a ngu?n thanh toán</param>
        /// <returns>Ngu?n thanh toán n?u tìm th?y, null n?u không tìm th?y</returns>
        Task<PaymentSource> FindById(string id);
    }
}

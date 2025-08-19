using SAPLSServer.Models;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace SAPLSServer.Repositories.Interfaces
{
    public interface IPaymentSourceRepository : IRepository<PaymentSource, string>
    {
        /// <summary>
        /// L?y danh s�ch c�c ngu?n thanh to�n theo ID c?a ch? b�i ?? xe
        /// </summary>
        /// <param name="ownerId">ID c?a ch? b�i ?? xe</param>
        /// <returns>Danh s�ch c�c ngu?n thanh to�n</returns>
        Task<List<PaymentSource>> GetPaymentSourcesByOwnerIdAsync(string ownerId);
        
        /// <summary>
        /// T�m ngu?n thanh to�n theo ID, x? l� chuy?n ??i ki?u d? li?u
        /// </summary>
        /// <param name="id">ID c?a ngu?n thanh to�n</param>
        /// <returns>Ngu?n thanh to�n n?u t�m th?y, null n?u kh�ng t�m th?y</returns>
        Task<PaymentSource> FindById(string id);
    }
}

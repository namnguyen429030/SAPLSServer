using Microsoft.EntityFrameworkCore;
using SAPLSServer.Models;
using SAPLSServer.Repositories.Interfaces;
using System.Linq.Expressions;

namespace SAPLSServer.Repositories.Implementations
{
    public class PaymentSourceRepository : Repository<PaymentSource, string>, IPaymentSourceRepository
    {
        public PaymentSourceRepository(SaplsContext context) : base(context)
        {
        }
        
        /// <summary>
        /// L?y danh s�ch c�c ngu?n thanh to�n theo ID c?a ch? b�i ?? xe
        /// </summary>
        public async Task<List<PaymentSource>> GetPaymentSourcesByOwnerIdAsync(string ownerId)
        {
            try
            {
                // S? d?ng FromSqlRaw ?? l?y d? li?u t? DB v� ki?m so�t qu� tr�nh mapping
                var sql = @"
                    SELECT 
                        CONVERT(VARCHAR(36), Id) as Id, 
                        BankName, 
                        AccountName, 
                        AccountNumber, 
                        Status, 
                        UpdatedAt, 
                        ParkingLotOwnerId 
                    FROM PaymentSource 
                    WHERE ParkingLotOwnerId = @ownerId";

                var paymentSources = await _context.PaymentSources
                    .FromSqlRaw(sql, new Microsoft.Data.SqlClient.SqlParameter("@ownerId", ownerId))
                    .AsNoTracking()
                    .ToListAsync();
                    
                return paymentSources;
            }
            catch (Exception ex)
            {
                // Log l?i
                Console.WriteLine($"Error in GetPaymentSourcesByOwnerIdAsync: {ex.Message}");
                throw;
            }
        }
        
        /// <summary>
        /// T�m ngu?n thanh to�n theo ID, x? l� chuy?n ??i ki?u d? li?u
        /// </summary>
        public async Task<PaymentSource> FindById(string id)
        {
            try
            {
                // S? d?ng FromSqlRaw ?? l?y d? li?u v� x? l� chuy?n ??i ki?u
                var sql = @"
                    SELECT 
                        CONVERT(VARCHAR(36), Id) as Id, 
                        BankName, 
                        AccountName, 
                        AccountNumber, 
                        Status, 
                        UpdatedAt, 
                        ParkingLotOwnerId 
                    FROM PaymentSource 
                    WHERE Id = @id";

                return await _context.PaymentSources
                    .FromSqlRaw(sql, new Microsoft.Data.SqlClient.SqlParameter("@id", id))
                    .AsNoTracking()
                    .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in FindById: {ex.Message}");
                throw;
            }
        }
        
        protected override Expression<Func<PaymentSource, bool>> CreateIdPredicate(string id)
        {
            return ps => ps.Id == id;
        }
    }
}

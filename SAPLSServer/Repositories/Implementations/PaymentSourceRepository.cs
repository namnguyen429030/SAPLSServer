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
        protected override Expression<Func<PaymentSource, bool>> CreateIdPredicate(string id)
        {
            return ps => ps.Id == id;
        }
    }
}

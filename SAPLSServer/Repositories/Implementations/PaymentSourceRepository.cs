using SAPLSServer.Models;
using SAPLSServer.Repositories.Interfaces;

namespace SAPLSServer.Repositories.Implementations
{
    public class PaymentSourceRepository : Repository<PaymentSource, string>, IPaymentSourceRepository
    {
        public PaymentSourceRepository(SaplsContext context) : base(context)
        {
        }
    }
}

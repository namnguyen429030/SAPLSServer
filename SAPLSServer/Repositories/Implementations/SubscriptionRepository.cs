using SAPLSServer.Models;
using SAPLSServer.Repositories.Interfaces;
using System.Linq.Expressions;

namespace SAPLSServer.Repositories.Implementations
{
    public class SubscriptionRepository : Repository<Subscription, string>, ISubscriptionRepository
    {
        public SubscriptionRepository(SaplsContext context) : base(context)
        {
        }

        protected override Expression<Func<Subscription, bool>> CreateIdPredicate(string id)
        {
            return s => s.Id == id;
        }
    }
}
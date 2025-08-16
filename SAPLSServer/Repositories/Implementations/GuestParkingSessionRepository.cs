using SAPLSServer.Models;
using SAPLSServer.Repositories.Interfaces;
using System.Linq.Expressions;

namespace SAPLSServer.Repositories.Implementations
{
    public class GuestParkingSessionRepository : Repository<GuestParkingSession, string>, IGuestParkingSessionRepository
    {
        public GuestParkingSessionRepository(SaplsContext context) : base(context)
        {
        }

        protected override Expression<Func<GuestParkingSession, bool>> CreateIdPredicate(string id)
        {
            return session => session.Id == id;
        }
    }
}

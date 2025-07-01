using SAPLSServer.Models;
using SAPLSServer.Repositories.Interfaces;

namespace SAPLSServer.Repositories
{
    public class ParkingSessionRepository : Repository<ParkingSession, string>, IParkingSessionRepository
    {
        public ParkingSessionRepository(SaplsContext context) : base(context)
        {
        }
    }
}

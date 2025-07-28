using SAPLSServer.Models;
using SAPLSServer.Repositories.Interfaces;

namespace SAPLSServer.Repositories.Implementations
{
    public class ParkingLotOwnerProfileRepository : Repository<ParkingLotOwnerProfile, string>, IParkingLotOwnerProfileRepository
    {
        public ParkingLotOwnerProfileRepository(SaplsContext context) : base(context)
        {
        }
    }
}

using SAPLSServer.Models;
using SAPLSServer.Repositories.Interfaces;

namespace SAPLSServer.Repositories
{
    public class ParkingLotOwnerProfileRepository : Repository<ParkingLotOwnerProfile, string>, IParkingLotOwnerProfileRepository
    {
        public ParkingLotOwnerProfileRepository(SaplsContext context) : base(context)
        {
        }
    }
}

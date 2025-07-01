using SAPLSServer.Models;
using SAPLSServer.Repositories.Interfaces;

namespace SAPLSServer.Repositories
{
    public class ParkingLotRepository : Repository<ParkingLot, string>, IParkingLotRepository
    {
        public ParkingLotRepository(SaplsContext context) : base(context)
        {
        }
    }
}

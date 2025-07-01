using SAPLSServer.Models;
using SAPLSServer.Repositories.Interfaces;

namespace SAPLSServer.Repositories
{
    public class SharedVehicleRepository : Repository<SharedVehicle, string>, ISharedVehicleRepository
    {
        public SharedVehicleRepository(SaplsContext context) : base(context)
        {
        }
    }
}

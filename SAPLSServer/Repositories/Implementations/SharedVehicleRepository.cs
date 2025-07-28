using SAPLSServer.Models;
using SAPLSServer.Repositories.Interfaces;

namespace SAPLSServer.Repositories.Implementations
{
    public class SharedVehicleRepository : Repository<SharedVehicle, string>, ISharedVehicleRepository
    {
        public SharedVehicleRepository(SaplsContext context) : base(context)
        {
        }
    }
}

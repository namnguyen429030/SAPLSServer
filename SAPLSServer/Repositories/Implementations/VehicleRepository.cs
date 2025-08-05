using SAPLSServer.Models;
using SAPLSServer.Repositories.Interfaces;
using System.Linq.Expressions;

namespace SAPLSServer.Repositories.Implementations
{
    public class VehicleRepository : Repository<Vehicle, string>, IVehicleRepository
    {
        public VehicleRepository(SaplsContext context) : base(context)
        {
        }
        protected override Expression<Func<Vehicle, bool>> CreateIdPredicate(string id)
        {
            return v => v.Id == id;
        }
    }
}

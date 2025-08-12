using Microsoft.EntityFrameworkCore;
using SAPLSServer.Models;
using SAPLSServer.Repositories.Interfaces;
using System.Linq.Expressions;

namespace SAPLSServer.Repositories.Implementations
{
    public class SharedVehicleRepository : Repository<SharedVehicle, string>, ISharedVehicleRepository
    {
        public SharedVehicleRepository(SaplsContext context) : base(context)
        {
        }

        public async Task<SharedVehicle?> FindIncludingVehicleAndOwner(string id)
        {
            return await _dbSet.Include(sv => sv.Vehicle.Owner).FirstOrDefaultAsync(CreateIdPredicate(id));
        }

        public async Task<SharedVehicle?> FindIncludingVehicleAndOwnerReadOnly(string id)
        {
            return await _dbSet
                .Include(sv => sv.Vehicle)
                .Include(sv => sv.Vehicle.Owner)
                .Include(sv => sv.Vehicle.Owner.User)
                .AsNoTracking()
                .FirstOrDefaultAsync(CreateIdPredicate(id));
        }

        public async Task<SharedVehicle?> FindIncludingVehicle(string id)
        {
            return await _dbSet.Include(sv => sv.Vehicle).FirstOrDefaultAsync(CreateIdPredicate(id));
        }

        public async Task<SharedVehicle?> FindIncludingVehicleReadOnly(string id)
        {
            return await _dbSet.Include(sv => sv.Vehicle)
                .AsNoTracking()
                .FirstOrDefaultAsync(CreateIdPredicate(id));
        }

        protected override Expression<Func<SharedVehicle, bool>> CreateIdPredicate(string id)
        {
            return sv => sv.VehicleId == id;
        }
    }
}

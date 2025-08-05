using Microsoft.EntityFrameworkCore;
using SAPLSServer.Models;
using SAPLSServer.Repositories.Interfaces;
using System.Linq.Expressions;

namespace SAPLSServer.Repositories.Implementations
{
    public class ParkingLotOwnerProfileRepository : Repository<ParkingLotOwnerProfile, string>, IParkingLotOwnerProfileRepository
    {
        public ParkingLotOwnerProfileRepository(SaplsContext context) : base(context)
        {
        }

        public async Task<ParkingLotOwnerProfile?> FindIncludingUser(string userId)
        {
            return await _dbSet.Include(plo => plo.User)
                            .FirstOrDefaultAsync(plo => plo.UserId == userId);
        }

        public async Task<ParkingLotOwnerProfile?> FindIncludingUserReadOnly(string userId)
        {
            return await _dbSet.Include(plo => plo.User)
                .AsNoTracking()
                .FirstOrDefaultAsync(plo => plo.UserId == userId);
        }

        protected override Expression<Func<ParkingLotOwnerProfile, bool>> CreateIdPredicate(string id)
        {
            return plo => plo.UserId == id;
        }
    }
}

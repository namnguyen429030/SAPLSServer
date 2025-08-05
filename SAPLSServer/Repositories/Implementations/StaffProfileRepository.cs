using Microsoft.EntityFrameworkCore;
using SAPLSServer.Models;
using SAPLSServer.Repositories.Interfaces;
using System.Linq.Expressions;

namespace SAPLSServer.Repositories.Implementations
{
    public class StaffProfileRepository : Repository<StaffProfile, string>, IStaffProfileRepository
    {
        public StaffProfileRepository(SaplsContext context) : base(context)
        {
        }

        public async Task<StaffProfile?> FindIncludingUser(string userId)
        {
            return await _dbSet.Include(sp => sp.User).FirstOrDefaultAsync(CreateIdPredicate(userId));
        }

        public async Task<StaffProfile?> FindIncludingUserReadOnly(string userId)
        {
            return await _dbSet.Include(sp => sp.User)
                .AsNoTracking()
                .FirstOrDefaultAsync(CreateIdPredicate(userId));
        }
        protected override Expression<Func<StaffProfile, bool>> CreateIdPredicate(string id)
        {
            return sp => sp.UserId == id;
        }
    }
}

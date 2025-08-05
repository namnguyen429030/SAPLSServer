using Microsoft.EntityFrameworkCore;
using SAPLSServer.Models;
using SAPLSServer.Repositories.Interfaces;
using System.Linq.Expressions;

namespace SAPLSServer.Repositories.Implementations
{
    public class ClientProfileRepository : Repository<ClientProfile, string>, IClientProfileRepository
    {
        public ClientProfileRepository(SaplsContext context) : base(context)
        {
        }

        public async Task<ClientProfile?> FindIncludingUser(string userId)
        {
            return await _dbSet.Include(cp => cp.User)
                            .FirstOrDefaultAsync(CreateIdPredicate(userId));
        }

        public async Task<ClientProfile?> FindIncludingUserReadOnly(string userId)
        {
            return await _dbSet.Include(cp => cp.User)
                .AsNoTracking()
                .FirstOrDefaultAsync(CreateIdPredicate(userId));
        }

        protected override Expression<Func<ClientProfile, bool>> CreateIdPredicate(string id)
        {
            return cp => cp.UserId == id;
        }
    }
}

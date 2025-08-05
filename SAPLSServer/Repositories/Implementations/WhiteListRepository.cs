using Microsoft.EntityFrameworkCore;
using SAPLSServer.Models;
using SAPLSServer.Repositories.Interfaces;
using System.Linq.Expressions;

namespace SAPLSServer.Repositories.Implementations
{
    public class WhiteListRepository : Repository<WhiteList, string[]>, IWhiteListRepository
    {
        public WhiteListRepository(SaplsContext context) : base(context)
        {
        }

        public async Task<WhiteList?> FindIncludingClientReadOnly(string[] ids)
        {
            return await _dbSet.Include(wl => wl.Client)
                .AsNoTracking()
                .FirstOrDefaultAsync(CreateIdPredicate(ids));
        }
        public async Task<WhiteList?> FindIncludingClient(string[] ids)
        {
            return await _dbSet.Include(wl => wl.Client)
                .FirstOrDefaultAsync(CreateIdPredicate(ids));
        }
        protected override Expression<Func<WhiteList, bool>> CreateIdPredicate(string[] id)
        {
            return wl => wl.ParkingLotId == id[0] && wl.ClientId == id[1];
        }
    }
}

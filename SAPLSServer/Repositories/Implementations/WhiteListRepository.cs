using SAPLSServer.Models;
using SAPLSServer.Repositories.Interfaces;

namespace SAPLSServer.Repositories.Implementations
{
    public class WhiteListRepository : Repository<WhiteList, string[]>, IWhiteListRepository
    {
        public WhiteListRepository(SaplsContext context) : base(context)
        {
        }

        public Task<IEnumerable<WhiteList>> GetByParkingLotIdAsync(string parkingLotId)
        {
            throw new NotImplementedException();
        }
    }
}

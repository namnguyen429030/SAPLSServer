using SAPLSServer.Models;
using SAPLSServer.Repositories.Interfaces;

namespace SAPLSServer.Repositories
{
    public class StaffProfileRepository : Repository<StaffProfile, string>, IStaffProfileRepository
    {
        public StaffProfileRepository(SaplsContext context) : base(context)
        {
        }
    }
}

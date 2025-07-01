using SAPLSServer.Models;
using SAPLSServer.Repositories.Interfaces;

namespace SAPLSServer.Repositories
{
    public class AdminProfileRepository : Repository<AdminProfile, string>, IAdminProfileRepository
    {
        public AdminProfileRepository(SaplsContext context) : base(context)
        {
        }
    }
}

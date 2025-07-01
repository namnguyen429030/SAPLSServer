using SAPLSServer.Models;
using SAPLSServer.Repositories.Interfaces;

namespace SAPLSServer.Repositories
{
    public class UserRepository : Repository<User, string>, IUserRepository
    {
        public UserRepository(SaplsContext context) : base(context)
        {
        }
    }
}

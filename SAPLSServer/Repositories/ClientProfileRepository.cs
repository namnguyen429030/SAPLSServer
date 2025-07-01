using SAPLSServer.Models;
using SAPLSServer.Repositories.Interfaces;

namespace SAPLSServer.Repositories
{
    public class ClientProfileRepository : Repository<ClientProfile, string>, IClientProfileRepository
    {
        public ClientProfileRepository(SaplsContext context) : base(context)
        {
        }
    }
}

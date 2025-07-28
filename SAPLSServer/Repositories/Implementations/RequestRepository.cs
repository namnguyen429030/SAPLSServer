using SAPLSServer.Models;
using SAPLSServer.Repositories.Interfaces;

namespace SAPLSServer.Repositories.Implementations
{
    public class RequestRepository : Repository<Request, string>, IRequestRepository
    {
        public RequestRepository(SaplsContext context) : base(context)
        {
        }
    }
}

using SAPLSServer.Models;
using SAPLSServer.Repositories.Interfaces;

namespace SAPLSServer.Repositories.Implementations
{
    public class RequestAttachedFileRepository : Repository<RequestAttachedFile, string>, IRequestAttachedFileRepository
    {
        public RequestAttachedFileRepository(SaplsContext context) : base(context)
        {
        }
    }
}

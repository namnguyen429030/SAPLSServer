using SAPLSServer.Models;
using SAPLSServer.Repositories.Interfaces;
using System.Linq.Expressions;

namespace SAPLSServer.Repositories.Implementations
{
    public class RequestAttachedFileRepository : Repository<RequestAttachedFile, string>, IRequestAttachedFileRepository
    {
        public RequestAttachedFileRepository(SaplsContext context) : base(context)
        {
        }
        protected override Expression<Func<RequestAttachedFile, bool>> CreateIdPredicate(string id)
        {
            return raf => raf.Id == id;
        }
    }
}

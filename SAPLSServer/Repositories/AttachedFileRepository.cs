using SAPLSServer.Models;
using SAPLSServer.Repositories.Interfaces;

namespace SAPLSServer.Repositories
{
    public class AttachedFileRepository : Repository<AttachedFile, string>, IAttachedFileRepository
    {
        public AttachedFileRepository(SaplsContext context) : base(context)
        {
        }
    }
}

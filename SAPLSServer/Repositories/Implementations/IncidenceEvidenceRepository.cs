using SAPLSServer.Models;
using SAPLSServer.Repositories.Interfaces;

namespace SAPLSServer.Repositories.Implementations
{
    public class IncidenceEvidenceRepository : Repository<IncidenceEvidence, string>, IIncidenceEvidenceRepository
    {
        public IncidenceEvidenceRepository(SaplsContext context) : base(context)
        {
        }
    }
}

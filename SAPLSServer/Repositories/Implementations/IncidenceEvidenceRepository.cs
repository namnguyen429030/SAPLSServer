using SAPLSServer.Models;
using SAPLSServer.Repositories.Interfaces;
using System.Linq.Expressions;

namespace SAPLSServer.Repositories.Implementations
{
    public class IncidenceEvidenceRepository : Repository<IncidenceEvidence, string>, IIncidenceEvidenceRepository
    {
        public IncidenceEvidenceRepository(SaplsContext context) : base(context)
        {
        }
        protected override Expression<Func<IncidenceEvidence, bool>> CreateIdPredicate(string id)
        {
            return ie => ie.Id == id;
        }
    }
}

using SAPLSServer.Models;
using SAPLSServer.Repositories.Interfaces;

namespace SAPLSServer.Repositories.Implementations
{
    public class IncidenceReportRepository : Repository<IncidenceReport, string>, IIncidenceReportRepository
    {
        public IncidenceReportRepository(SaplsContext context) : base(context)
        {
        }
    }
}

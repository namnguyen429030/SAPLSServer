using SAPLSServer.DTOs.Base;

namespace SAPLSServer.DTOs.Concrete.IncidenceReport
{
    public class GetIncidenReportListRequest : GetListRequest
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}

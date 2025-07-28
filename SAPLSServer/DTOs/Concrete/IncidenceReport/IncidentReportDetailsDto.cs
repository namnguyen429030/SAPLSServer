using SAPLSServer.DTOs.Base;
using SAPLSServer.DTOs.Concrete;

namespace SAPLSServer.DTOs.Concrete
{
    public class IncidentReportDetailsDto : GetResult
    {
        public string Header { get; set; } = null!;

        public DateTime ReportedDate { get; set; }

        public string Priority { get; set; } = null!;

        public string Description { get; set; } = null!;

        public string Status { get; set; } = null!;
        public StaffProfileSummaryDto Reporter { get; set; } = null!;
    }
}

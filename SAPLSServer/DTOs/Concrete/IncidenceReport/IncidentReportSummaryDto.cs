using SAPLSServer.DTOs.Base;

namespace SAPLSServer.DTOs.Concrete
{
    public class IncidentReportSummaryDto : GetResult
    {
        public string Header { get; set; } = null!;

        public DateTime ReportedDate { get; set; }

        public string Priority { get; set; } = null!;
        public string Status { get; set; } = null!;
        public string ReporterId { get; set; } = null!;
    }
}

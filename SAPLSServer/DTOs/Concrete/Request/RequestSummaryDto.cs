using SAPLSServer.DTOs.Base;

namespace SAPLSServer.DTOs.Concrete.Request
{
    public class RequestSummaryDto : GetResult
    {
        public string Header { get; set; } = null!;

        public string Status { get; set; } = null!;

        public DateTime SubmittedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}

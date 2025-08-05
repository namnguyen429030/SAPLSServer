using SAPLSServer.DTOs.Concrete.UserDto;
using SAPLSServer.Models;

namespace SAPLSServer.DTOs.Concrete.IncidentReportDto
{
    public class IncidentReportDetailsDto : IncidentReportSummaryDto
    {
        public string Description { get; set; }

        public StaffProfileSummaryDto Reporter { get; set; }
        public IncidentReportDetailsDto(IncidenceReport incidentReport) : base(incidentReport)
        {
            Description = incidentReport.Description;
            Reporter = new StaffProfileSummaryDto(incidentReport.Reporter);
        }
    }
}

using SAPLSServer.DTOs.Concrete.UserDtos;
using SAPLSServer.Models;

namespace SAPLSServer.DTOs.Concrete.IncidenceReportDtos
{
    public class IncidentReportDetailsDto : OwnedIncidentReportDetailsDto
    {
        public StaffProfileSummaryDto Reporter { get; set; }
        public IncidentReportDetailsDto(IncidenceReport incidentReport) : base(incidentReport)
        {
            Description = incidentReport.Description;
            Reporter = new StaffProfileSummaryDto(incidentReport.Reporter);
        }
    }
}

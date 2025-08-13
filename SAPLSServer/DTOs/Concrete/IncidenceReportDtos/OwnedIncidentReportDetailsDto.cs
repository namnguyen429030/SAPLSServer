using SAPLSServer.DTOs.Concrete.AttachedFileDtos;
using SAPLSServer.Models;

namespace SAPLSServer.DTOs.Concrete.IncidenceReportDtos
{
    public class OwnedIncidentReportDetailsDto : IncidentReportSummaryDto
    {
        public string Description { get; set; }
        public GetAttachedFileDto[]? AttacheFiles { get; set; }
        public OwnedIncidentReportDetailsDto(IncidenceReport incidentReport) : base(incidentReport)
        {
            Description = incidentReport.Description;
        }
    }
}

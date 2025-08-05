using SAPLSServer.DTOs.Base;
using System.ComponentModel.DataAnnotations;
using SAPLSServer.Constants;

namespace SAPLSServer.DTOs.Concrete.IncidentReportDto
{
    public class CreateIncidentReportRequest
    {
        [Required(ErrorMessage = MessageKeys.SHIFT_DIARY_HEADER_REQUIRED)]
        public string Header { get; set; } = null!;
        [Required(ErrorMessage = MessageKeys.INCIDENT_PRIORITY_REQUIRED)]
        [EnumDataType(typeof(IncidentReportPriority), ErrorMessage = MessageKeys.INVALID_INCIDENT_REPORT_PRIORITY)]
        public string Priority { get; set; } = null!;
        [Required(ErrorMessage = MessageKeys.SHIFT_DIARY_BODY_REQUIRED)]
        public string Description { get; set; } = null!;
        [Required(ErrorMessage = MessageKeys.STATUS_REQUIRED)]
        [EnumDataType(typeof(IncidentReportStatus), ErrorMessage = MessageKeys.INVALID_INCIDENT_REPORT_STATUS)]
        public string Status { get; set; } = null!;
        [Required(ErrorMessage = MessageKeys.PARKING_LOT_ID_REQUIRED)]
        public string ParkingLotId { get; set; } = null!;
    }
}

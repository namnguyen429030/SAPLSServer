using SAPLSServer.DTOs.Base;
using System.ComponentModel.DataAnnotations;

namespace SAPLSServer.DTOs.Concrete
{
    public class UpdateIncidentReportRequest : UpdateRequest
    {
        [Required(ErrorMessage = "Status is required.")]
        public string Status { get; set; } = null!;
    }
}

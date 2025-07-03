using SAPLSServer.DTOs.Base;
using System.ComponentModel.DataAnnotations;

namespace SAPLSServer.DTOs.Concrete.IncidenceReport
{
    public class CreateIncidentReportRequest : CreateRequest
    {
        [Required(ErrorMessage = "Header is required.")]
        public string Header { get; set; } = null!;
        [Required(ErrorMessage = "Priority is required.")]
        public string Priority { get; set; } = null!;
        [Required(ErrorMessage = "Description is required.")]
        public string Description { get; set; } = null!;
        [Required(ErrorMessage = "Status is required.")]
        public string Status { get; set; } = null!;
        [Required(ErrorMessage = "Reporter ID is required.")]
        public string ReporterId { get; set; } = null!;
        [Required(ErrorMessage = "Parking Lot ID is required.")]
        public string ParkingLotId { get; set; } = null!;
    }
}

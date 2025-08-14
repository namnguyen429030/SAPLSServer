using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SAPLSServer.DTOs.Concrete.ParkingLotShiftDtos
{
    public class CreateParkingLotShiftRequest
    {
        [Required]
        public string ParkingLotId { get; set; } = null!;
        [Required]
        public int StartTime { get; set; } // minutes from midnight
        [Required]
        public int EndTime { get; set; }
        [Required]
        public string ShiftType { get; set; } = null!;
        public string? DayOfWeeks { get; set; }
        public string? SpecificDate { get; set; }
        public string? Status { get; set; }
        public string? Notes { get; set; }
        public List<string>? StaffIds { get; set; }
    }
}
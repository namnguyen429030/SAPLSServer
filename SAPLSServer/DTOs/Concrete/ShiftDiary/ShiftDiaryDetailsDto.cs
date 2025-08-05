using SAPLSServer.Models;

namespace SAPLSServer.DTOs.Concrete.ShiftDiaryDto
{
    public class ShiftDiaryDetailsDto : ShiftDiarySummaryDto
    {
        public string StaffId { get; set; }
        public string StaffName { get; set; }
        
        public ShiftDiaryDetailsDto(ShiftDiary shiftDiary) : base(shiftDiary)
        {
            StaffId = shiftDiary.Sender?.StaffId ?? string.Empty;
            StaffName = shiftDiary.Sender?.User.FullName ?? string.Empty;
        }
    }
}

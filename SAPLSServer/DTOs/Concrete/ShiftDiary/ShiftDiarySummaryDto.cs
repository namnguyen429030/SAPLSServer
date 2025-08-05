using SAPLSServer.DTOs.Base;
using SAPLSServer.Models;

namespace SAPLSServer.DTOs.Concrete.ShiftDiaryDto
{
    public class ShiftDiarySummaryDto : GetResult
    {

        public string Header { get; set; }

        public string Body { get; set; }

        public DateTime CreatedAt { get; set; }
        public ShiftDiarySummaryDto(ShiftDiary shiftDiary)
        {
            Id = shiftDiary.Id;
            Header = shiftDiary.Header;
            Body = shiftDiary.Body;
            CreatedAt = shiftDiary.CreatedAt;
        }
    }
}

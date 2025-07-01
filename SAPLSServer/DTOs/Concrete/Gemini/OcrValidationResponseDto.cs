// OcrValidationResponseDto.cs
using SAPLSServer.DTOs.Base;

namespace SAPLSServer.DTOs.Concrete.GeminiOcr
{
    public class OcrValidationResponseDto : GetDto
    {
        public bool IsValid { get; set; }
        public Dictionary<string, string> CorrectedData { get; set; } = new();
        public List<string> ValidationErrors { get; set; } = new();
        public List<string> Suggestions { get; set; } = new();
        public double OverallConfidence { get; set; }
        public DateTime ValidatedAt { get; set; }
    }
}

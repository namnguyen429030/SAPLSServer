// OcrServiceHealthDto.cs
using SAPLSServer.DTOs.Base;

namespace SAPLSServer.DTOs.Concrete.GeminiOcr
{
    public class OcrServiceHealthDto : GetDto
    {
        public bool IsHealthy { get; set; }
        public string ServiceStatus { get; set; } = null!;
        public List<string> AvailableModels { get; set; } = new();
        public double ResponseTimeMs { get; set; }
        public DateTime CheckedAt { get; set; }
        public string? ErrorMessage { get; set; }
        public Dictionary<string, object> AdditionalInfo { get; set; } = new();
    }
}

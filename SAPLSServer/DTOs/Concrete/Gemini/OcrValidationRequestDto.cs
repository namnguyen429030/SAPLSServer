// OcrValidationRequestDto.cs
using SAPLSServer.DTOs.Base;

namespace SAPLSServer.DTOs.Concrete.GeminiOcr
{
    public class OcrValidationRequestDto : CreateDto
    {
        public string DocumentType { get; set; } = null!; // "CitizenId" or "VehicleRegistration"
        public Dictionary<string, string> ExtractedData { get; set; } = new();
        public string OriginalImageBase64 { get; set; } = null!;
    }
}


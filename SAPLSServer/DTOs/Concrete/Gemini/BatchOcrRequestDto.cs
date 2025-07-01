// BatchOcrRequestDto.cs
using SAPLSServer.DTOs.Base;

namespace SAPLSServer.DTOs.Concrete.GeminiOcr
{
    public class BatchOcrRequestDto : CreateDto
    {
        public List<DocumentImageDto> Documents { get; set; } = new();
        public bool EnhanceAccuracy { get; set; } = true;
        public string Language { get; set; } = "vi";
    }

    public class DocumentImageDto
    {
        public string DocumentType { get; set; } = null!; // "CitizenId" or "VehicleRegistration"
        public string ImageBase64 { get; set; } = null!;
        public string ImageFormat { get; set; } = null!;
        public string? ReferenceId { get; set; }
    }
}


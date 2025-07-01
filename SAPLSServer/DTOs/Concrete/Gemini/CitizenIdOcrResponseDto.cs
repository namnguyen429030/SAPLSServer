using SAPLSServer.DTOs.Base;

namespace SAPLSServer.DTOs.Concrete.GeminiOcr
{
    public class CitizenIdOcrResponseDto : GetDto
    {
        public string CitizenId { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public DateTime DateOfBirth { get; set; }
        public string Sex { get; set; } = null!;
        public string Nationality { get; set; } = null!;
        public string PlaceOfOrigin { get; set; } = null!;
        public string PlaceOfResidence { get; set; } = null!;
        public DateTime? ExpiryDate { get; set; }
        public double ConfidenceScore { get; set; }
        public List<OcrFieldConfidenceDto> FieldConfidences { get; set; } = new();
        public DateTime ProcessedAt { get; set; }
        public string ProcessingTimeMs { get; set; } = null!;
    }
}


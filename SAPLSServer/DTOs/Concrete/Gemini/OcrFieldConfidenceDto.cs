namespace SAPLSServer.DTOs.Concrete.GeminiOcr
{
    public class OcrFieldConfidenceDto
    {
        public string FieldName { get; set; } = null!;
        public string ExtractedValue { get; set; } = null!;
        public double Confidence { get; set; }
        public string? AlternativeValues { get; set; }
    }
}


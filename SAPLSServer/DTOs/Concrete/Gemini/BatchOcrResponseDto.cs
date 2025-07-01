// BatchOcrResponseDto.cs
using SAPLSServer.DTOs.Base;

namespace SAPLSServer.DTOs.Concrete.GeminiOcr
{
    public class BatchOcrResponseDto : GetDto
    {
        public List<BatchOcrResultDto> Results { get; set; } = new();
        public int TotalProcessed { get; set; }
        public int SuccessfulExtractions { get; set; }
        public int FailedExtractions { get; set; }
        public DateTime ProcessedAt { get; set; }
        public string TotalProcessingTimeMs { get; set; } = null!;
    }

    public class BatchOcrResultDto
    {
        public string? ReferenceId { get; set; }
        public string DocumentType { get; set; } = null!;
        public bool IsSuccess { get; set; }
        public object? ExtractedData { get; set; } // CitizenIdOcrResponseDto or VehicleRegistrationOcrResponseDto
        public string? ErrorMessage { get; set; }
        public double ConfidenceScore { get; set; }
    }
}

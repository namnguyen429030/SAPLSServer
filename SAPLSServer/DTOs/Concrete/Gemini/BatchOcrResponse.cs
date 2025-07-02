using SAPLSServer.DTOs.Base;

namespace SAPLSServer.DTOs.Concrete.GeminiOcr
{
    public class BatchOcrResponse : GetResult
    {
        public List<BatchOcrResult> Results { get; set; } = new();
        public int TotalProcessed { get; set; }
        public int SuccessfulExtractions { get; set; }
        public int FailedExtractions { get; set; }
        public DateTime ProcessedAt { get; set; }
        public string TotalProcessingTimeMs { get; set; } = null!;
    }

    public class BatchOcrResult
    {
        public string? ReferenceId { get; set; }
        public string DocumentType { get; set; } = null!;
        public bool IsSuccess { get; set; }
        public object? ExtractedData { get; set; } // CitizenIdOcrResponse or VehicleRegistrationOcrResponse
        public string? ErrorMessage { get; set; }
        public double ConfidenceScore { get; set; }
    }
}

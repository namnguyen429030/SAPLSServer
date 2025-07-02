using SAPLSServer.DTOs.Base;

namespace SAPLSServer.DTOs.Concrete.GeminiOcr
{
    public class VehicleRegistrationOcrResponse : GetResult
    {
        public string LicensePlate { get; set; } = null!;
        public string Brand { get; set; } = null!;
        public string Model { get; set; } = null!;
        public string EngineNumber { get; set; } = null!;
        public string ChassisNumber { get; set; } = null!;
        public string Color { get; set; } = null!;
        public string OwnerVehicleFullName { get; set; } = null!;
        public DateTime? RegistrationDate { get; set; }
        public string VehicleType { get; set; } = null!;
        public string? PlateType { get; set; }
        public double ConfidenceScore { get; set; }
        public List<OcrFieldConfidenceDto> FieldConfidences { get; set; } = new();
        public DateTime ProcessedAt { get; set; }
        public string ProcessingTimeMs { get; set; } = null!;
    }
}


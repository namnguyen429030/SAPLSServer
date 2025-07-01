// VehicleRegistrationOcrRequestDto.cs
using SAPLSServer.DTOs.Base;
using System.ComponentModel.DataAnnotations;

namespace SAPLSServer.DTOs.Concrete.GeminiOcr
{
    public class VehicleRegistrationOcrRequestDto : CreateDto
    {
        [Required(ErrorMessage = "Image data is required.")]
        public string ImageBase64 { get; set; } = null!;
        
        public string? ImageUrl { get; set; }
        
        [Required(ErrorMessage = "Image format is required.")]
        public string ImageFormat { get; set; } = null!;
        
        public string Language { get; set; } = "vi";
        
        public bool EnhanceAccuracy { get; set; } = true;
    }
}


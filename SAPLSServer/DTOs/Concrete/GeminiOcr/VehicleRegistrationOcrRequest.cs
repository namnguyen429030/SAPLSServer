using SAPLSServer.DTOs.Base;
using System.ComponentModel.DataAnnotations;

namespace SAPLSServer.DTOs.Concrete.OcrDto
{
    public class VehicleRegistrationOcrRequest
    {
        [Required(ErrorMessage = "Front image data is required.")]
        public string FrontImageBase64 { get; set; } = null!;
        [Required(ErrorMessage = "Back image data is required.")]
        public string BackImageBase64 { get; set; } = null!;
        [Required(ErrorMessage = "Image format is required.")]
        public string ImageFormat { get; set; } = null!;
    }
}
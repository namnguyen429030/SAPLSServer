using SAPLSServer.DTOs.Base;
using System.ComponentModel.DataAnnotations;

namespace SAPLSServer.DTOs.Concrete
{
    public class CitizenIdOcrRequest : CreateRequest
    {
        [Required(ErrorMessage = "Front image data is required.")]
        public string FrontImageBase64 { get; set; } = null!;
        [Required(ErrorMessage = "Back image data is required.")]
        public string BackImageBase64 { get; set; } = null!;
        [Required(ErrorMessage = "Image format is required.")]
        public string ImageFormat { get; set; } = null!;
        public string Language { get; set; } = "vi";
        public bool EnhanceAccuracy { get; set; }
    }
}
using Microsoft.AspNetCore.Http;

namespace SAPLSServer.DTOs.Concrete
{
    public class VehicleRegistrationOcrFileRequest
    {
        public IFormFile FrontImage { get; set; } = null!;
        public IFormFile BackImage { get; set; } = null!;
        public string ImageFormat { get; set; } = "jpeg";
        public string Language { get; set; } = "vi";
        public bool EnhanceAccuracy { get; set; } = false;
    }
}
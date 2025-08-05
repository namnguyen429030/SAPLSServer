using SAPLSServer.Constants;
using System.ComponentModel.DataAnnotations;

namespace SAPLSServer.DTOs.Concrete.OcrDto
{
    public class CitizenIdOcrFileRequest
    {
        [Required(ErrorMessage = MessageKeys.FILE_REQUIRED)]
        [DataType(DataType.Upload, ErrorMessage = MessageKeys.INVALID_FILE_UPLOADED)]
        public IFormFile? FrontImage { get; set; }
        [Required(ErrorMessage = MessageKeys.FILE_REQUIRED)]
        [DataType(DataType.Upload, ErrorMessage = MessageKeys.INVALID_FILE_UPLOADED)]
        public IFormFile? BackImage { get; set; }
    }
}
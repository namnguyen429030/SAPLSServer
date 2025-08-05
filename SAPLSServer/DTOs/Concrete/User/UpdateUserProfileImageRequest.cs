using SAPLSServer.Constants;
using SAPLSServer.DTOs.Base;
using System.ComponentModel.DataAnnotations;

namespace SAPLSServer.DTOs.Concrete.UserDto
{
    public class UpdateUserProfileImageRequest : UpdateRequest
    {
        [DataType(DataType.Upload, ErrorMessage = MessageKeys.INVALID_FILE_UPLOADED)]
        public IFormFile? ProfileImage { get; set; } = null!;
    }
}

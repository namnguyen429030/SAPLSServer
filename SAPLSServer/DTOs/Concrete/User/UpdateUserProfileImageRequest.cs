using SAPLSServer.DTOs.Base;

namespace SAPLSServer.DTOs.Concrete
{
    public class UpdateUserProfileImageRequest : UpdateRequest
    {
        public string ProfileImageUrl { get; set; } = null!;
    }
}

using SAPLSServer.DTOs.Base;

namespace SAPLSServer.DTOs.Concrete.User
{
    public class UpdateUserProfileImageRequest : UpdateRequest
    {
        public string? ProfileImageUrl { get; set; }
    }
}

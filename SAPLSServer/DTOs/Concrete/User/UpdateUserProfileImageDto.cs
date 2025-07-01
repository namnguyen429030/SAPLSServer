using SAPLSServer.DTOs.Base;

namespace SAPLSServer.DTOs.Concrete.User
{
    public class UpdateUserProfileImageDto : UpdateDto
    {
        public string? ProfileImageUrl { get; set; }
    }
}

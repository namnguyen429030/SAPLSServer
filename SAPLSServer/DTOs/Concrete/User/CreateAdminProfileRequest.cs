using SAPLSServer.Constants;
using System.ComponentModel.DataAnnotations;

namespace SAPLSServer.DTOs.Concrete.UserDto
{
    public class CreateAdminProfileRequest : CreateUserRequest
    {
        [Required(ErrorMessage = MessageKeys.ADMIN_PROFILE_ID_REQUIRED)]
        [EnumDataType(typeof(AdminRole), ErrorMessage = MessageKeys.INVALID_ADMIN_ROLE)]
        public string AdminId { get; set; } = null!;
    }
}

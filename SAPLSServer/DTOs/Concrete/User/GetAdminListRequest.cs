using SAPLSServer.Constants;
using SAPLSServer.DTOs.Base;
using System.ComponentModel.DataAnnotations;

namespace SAPLSServer.DTOs.Concrete.UserDto
{
    public class GetAdminListRequest : GetUserListRequest
    {
        [EnumDataType(typeof(AdminRole), ErrorMessage = MessageKeys.INVALID_ADMIN_ROLE)]
        public string? Role { get; set; }
    }
}

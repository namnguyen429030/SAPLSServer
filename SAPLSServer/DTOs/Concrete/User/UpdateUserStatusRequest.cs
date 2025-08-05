using SAPLSServer.DTOs.Base;

namespace SAPLSServer.DTOs.Concrete.UserDto
{
    public class UpdateUserStatusRequest : UpdateRequest
    {
        public string Status { get; set; } = null!;
    }
}

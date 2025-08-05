using SAPLSServer.Constants;
using System.ComponentModel.DataAnnotations;

namespace SAPLSServer.DTOs.Concrete.UserDto
{
    public class AuthenticateUserRequest
    {
        [EmailAddress(ErrorMessage = MessageKeys.INVALID_EMAIL_FORMAT)]
        public string? Email { get; set; }
        [Required(ErrorMessage =  MessageKeys.PASSWORD_REQUIRED)]
        public string? Password { get; set; }
    }
}

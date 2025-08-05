using SAPLSServer.Constants;
using SAPLSServer.DTOs.Base;
using System.ComponentModel.DataAnnotations;

namespace SAPLSServer.DTOs.Concrete.UserDto
{
    public class UpdateUserPasswordRequest : UpdateRequest
    {
        [Required(ErrorMessage = MessageKeys.OLD_PASSWORD_REQUIRED)]
        public string OldPassword { get; set; } = null!;
        [Required(ErrorMessage = MessageKeys.NEW_PASSWORD_REQUIRED)]
        [Length(minimumLength: 8, maximumLength: 24, ErrorMessage = MessageKeys.PASSWORD_LENGTH)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,24}$", 
            ErrorMessage = MessageKeys.PASSWORD_COMPLEXITY)]
        public string NewPassword { get; set; } = null!;
    }
}

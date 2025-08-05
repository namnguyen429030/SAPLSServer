using SAPLSServer.Constants;
using System.ComponentModel.DataAnnotations;

namespace SAPLSServer.DTOs.Concrete.UserDto
{
    public class AuthenticateClientProfileRequest
    {
        [Required(ErrorMessage = MessageKeys.EMAIL_OR_CITIZEN_ID_NUMBER_IS_REQUIRED)]
        public string? EmailOrCitizenIdNo { get; set; }
        public string? Password { get; set; }
    }
}

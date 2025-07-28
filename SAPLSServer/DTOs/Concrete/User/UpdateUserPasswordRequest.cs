using SAPLSServer.DTOs.Base;
using System.ComponentModel.DataAnnotations;

namespace SAPLSServer.DTOs.Concrete
{
    public class UpdateUserPasswordRequest : UpdateRequest
    {
        [Required(ErrorMessage = "Old password is required.")]
        public string OldPassword { get; set; } = null!;
        [Required(ErrorMessage = "New password is required.")]
        [Length(minimumLength: 8, maximumLength: 24, ErrorMessage = "Password must be between 8 and 24 characters.")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,24}$", 
            ErrorMessage = "Password must contain at least 1 uppercase letter, 1 lowercase letter, 1 number, and 1 special character.")]
        public string NewPassword { get; set; } = null!;
    }
}

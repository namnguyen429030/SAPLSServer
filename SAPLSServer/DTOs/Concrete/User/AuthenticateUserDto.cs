using System.ComponentModel.DataAnnotations;

namespace SAPLSServer.DTOs.Concrete.User
{
    public class AuthenticateUserDto
    {
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string? Email { get; set; }
        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; } = null!;
    }
}

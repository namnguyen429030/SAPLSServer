using SAPLSServer.DTOs.Base;
using System.ComponentModel.DataAnnotations;

namespace SAPLSServer.DTOs.Concrete
{
    public abstract class UpdateUserInformationRequest : UpdateRequest
    {
        [Required(ErrorMessage = "Full name is required.")]
        public string FullName { get; set; } = null!;
        [Required(ErrorMessage = "Phone number is required.")]
        [RegularExpression(@"^\d+$", ErrorMessage = "Phone number should contain only digits.")]
        public string Phone { get; set; } = null!;
    }
}  
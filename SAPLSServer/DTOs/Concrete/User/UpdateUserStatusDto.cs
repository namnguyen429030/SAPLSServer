using SAPLSServer.DTOs.Base;
using System.ComponentModel.DataAnnotations;

namespace SAPLSServer.DTOs.Concrete.User
{
    public class UpdateUserStatusDto : UpdateDto
    {
        [Required(ErrorMessage = "Status is required.")]
        public string Status { get; set; } = null!;
    }
}

using SAPLSServer.DTOs.Base;
using System.ComponentModel.DataAnnotations;

namespace SAPLSServer.DTOs.Concrete
{
    public class UpdateRequestRequest : UpdateRequest
    {
        public string? InternalNote { get; set; }

        public string? ResponseMessage { get; set; }
        [Required(ErrorMessage = "Status is required.")]
        public string Status { get; set; } = null!;
    }
}

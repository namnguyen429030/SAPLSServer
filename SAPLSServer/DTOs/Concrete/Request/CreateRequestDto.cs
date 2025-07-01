using SAPLSServer.DTOs.Base;
using System.ComponentModel.DataAnnotations;

namespace SAPLSServer.DTOs.Concrete.Request
{
    public class CreateRequestDto : CreateDto
    {
        [Required(ErrorMessage = "Header is required.")]
        public string Header { get; set; } = null!;
        [Required(ErrorMessage = "Description is required.")]
        public string Description { get; set; } = null!;
        [Required(ErrorMessage = "Sender ID is required.")]
        public string SenderId { get; set; } = null!;
        public string? Type { get; set; }
        public string? Data { get; set; }
    }
}

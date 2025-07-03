using System.ComponentModel.DataAnnotations;

namespace SAPLSServer.DTOs.Base
{
    public class DeleteRequest
    {
        public string? RequestSenderId { get; set; }
        [Required(ErrorMessage = "ID is required.")]
        public string Id { get; set; } = null!;
    }
}

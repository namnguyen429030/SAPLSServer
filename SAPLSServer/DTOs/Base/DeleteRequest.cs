using System.ComponentModel.DataAnnotations;

namespace SAPLSServer.DTOs.Base
{
    public class DeleteRequest
    {
        [Required(ErrorMessage = "ID is required.")]
        public string Id { get; set; } = null!;
    }
}

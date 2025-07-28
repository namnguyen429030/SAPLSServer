using System.ComponentModel.DataAnnotations;

namespace SAPLSServer.DTOs.Base
{
    public abstract class UpdateRequest
    {
        [Required(ErrorMessage = "ID is required.")]
        public string Id { get; set; } = null!;
    }
}

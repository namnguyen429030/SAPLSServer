using System.ComponentModel.DataAnnotations;

namespace SAPLSServer.DTOs.Base
{
    public abstract class DeleteDto
    {
        [Required(ErrorMessage = "ID is required.")]
        public string Id { get; set; } = null!;
    }
}

using System.ComponentModel.DataAnnotations;

namespace SAPLSServer.DTOs.Base
{
    public class GetDetailsRequest
    {
        [Required(ErrorMessage = "ID is required.")]
        public string Id { get; set; } = null!;
    }
}

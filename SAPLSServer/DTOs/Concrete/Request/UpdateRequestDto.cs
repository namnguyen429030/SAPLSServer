using SAPLSServer.DTOs.Base;

namespace SAPLSServer.DTOs.Concrete.Request
{
    public class UpdateRequestDto : UpdateDto
    {
        public string? InternalNote { get; set; }

        public string? ResponseMessage { get; set; }
        public string Status { get; set; } = null!;
    }
}

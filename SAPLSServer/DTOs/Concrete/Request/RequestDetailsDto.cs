using SAPLSServer.DTOs.Base;

namespace SAPLSServer.DTOs.Concrete.Request
{
    public class RequestDetailsDto : GetResult
    {
        public string Email { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public string Header { get; set; } = null!;

        public string Description { get; set; } = null!;

        public string Status { get; set; } = null!;

        public DateTime SubmittedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public string? InternalNote { get; set; }

        public string? ResponseMessage { get; set; }
        public string? LastUpdateAdminId { get; set; }
        public string? LastUpdateAdminFullName { get; set; }
    }
}

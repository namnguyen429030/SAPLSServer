using SAPLSServer.DTOs.Base;
using SAPLSServer.Models;

namespace SAPLSServer.DTOs.Concrete.RequestDtos
{
    public class RequestDetailsDto : RequestSummaryDto
    {
        public string Email { get; set; }
        public string FullName { get; set; }
        public string Description { get; set; }
        public string? InternalNote { get; set; }

        public string? ResponseMessage { get; set; }
        public string? LastUpdateAdminId { get; set; }
        public string? LastUpdateAdminFullName { get; set; }
        public string[]? FileAttachmentUrls { get; set; }
        public RequestDetailsDto(Request request) : base(request)
        {
            Email = request.Sender?.Email ?? string.Empty;
            FullName = request.Sender?.FullName ?? string.Empty;
            Description = request.Description;
            InternalNote = request.InternalNote;
            ResponseMessage = request.ResponseMessage;
            LastUpdateAdminId = request.LastUpdatePerson?.AdminId;
            LastUpdateAdminFullName = request.LastUpdatePerson?.User.FullName;
        }
    }
}

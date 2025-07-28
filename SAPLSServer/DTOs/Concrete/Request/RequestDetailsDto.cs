using SAPLSServer.DTOs.Base;
using SAPLSServer.Models;

namespace SAPLSServer.DTOs.Concrete
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
        public RequestDetailsDto(Request request) : base(request)
        {
            Id = request.Id;
            Email = request.Sender.Email;
            FullName = request.Sender.FullName;
            Description = request.Description;
            InternalNote = request.InternalNote;
            ResponseMessage = request.ResponseMessage;
            LastUpdateAdminId = request.LastUpdatePerson?.AdminId;
            LastUpdateAdminFullName = request.LastUpdatePerson?.User.FullName;
        }
    }
}

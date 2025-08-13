using SAPLSServer.DTOs.Base;
using System.ComponentModel.DataAnnotations;
using SAPLSServer.Constants;

namespace SAPLSServer.DTOs.Concrete.RequestDtos
{
    public class UpdateRequestStatusRequest : UpdateRequest
    {
        [Required(ErrorMessage = MessageKeys.STATUS_REQUIRED)]
        [EnumDataType(typeof(RequestStatus), ErrorMessage = MessageKeys.INVALID_REQUEST_STATUS)]
        public string Status { get; set; } = null!;
    }
}
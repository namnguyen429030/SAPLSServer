using SAPLSServer.Constants;
using System.ComponentModel.DataAnnotations;

namespace SAPLSServer.DTOs.Concrete.ParkingSessionDto
{
    public class GetParkingSessionListByClientIdRequest : GetOwnedParkingSessionListRequest
    {
        [Required(ErrorMessage = MessageKeys.CLIENT_ID_REQUIRED)]
        public string? ClientId { get; set; }
    }
}

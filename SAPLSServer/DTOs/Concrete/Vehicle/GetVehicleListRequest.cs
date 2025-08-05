using SAPLSServer.Constants;
using SAPLSServer.DTOs.Base;
using System.ComponentModel.DataAnnotations;

namespace SAPLSServer.DTOs.Concrete.VehicleDto
{
    public class GetVehicleListRequest : GetListRequest
    {
        [Required(ErrorMessage = MessageKeys.PARKING_LOT_ID_REQUIRED)]
        public string OwnerId { get; set; } = string.Empty;
        public string? SharingStatus { get; set; }
    }
}

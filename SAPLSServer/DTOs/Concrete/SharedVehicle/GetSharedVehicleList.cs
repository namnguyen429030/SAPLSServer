using SAPLSServer.Constants;
using SAPLSServer.DTOs.Base;
using System.ComponentModel.DataAnnotations;

namespace SAPLSServer.DTOs.Concrete.SharedVehicleDto
{
    public class GetSharedVehicleList : GetListRequest
    {
        public string? SharedPersonId { get; set; }
    }
}

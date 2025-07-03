using SAPLSServer.DTOs.Base;

namespace SAPLSServer.DTOs.Concrete.ParkingSession
{
    public class GetParkingSessionListRequest : GetListRequest
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}

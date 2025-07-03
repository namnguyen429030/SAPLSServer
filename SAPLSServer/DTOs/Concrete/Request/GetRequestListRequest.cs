using SAPLSServer.DTOs.Base;

namespace SAPLSServer.DTOs.Concrete.Request
{
    public class GetRequestListRequest : GetListRequest
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}

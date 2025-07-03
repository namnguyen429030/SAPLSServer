namespace SAPLSServer.DTOs.Base
{
    public class GetListRequest
    {
        public string? RequestSenderId { get; set; }
        public string Order { get; set; } = "asc";
        public string Filter { get; set; } = null!;
        public string SearchCriteria { get; set; } = null!;
    }
}

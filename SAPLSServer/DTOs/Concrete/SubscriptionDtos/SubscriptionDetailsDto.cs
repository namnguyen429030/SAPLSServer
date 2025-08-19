using SAPLSServer.Models;

namespace SAPLSServer.DTOs.Concrete.SubscriptionDtos
{
    public class SubscriptionDetailsDto : SubscriptionSummaryDto
    {
        public string? Description { get; set; }
        public SubscriptionDetailsDto(Subscription model) : base(model)
        {
            Description = model.Description;
        }
    }
}
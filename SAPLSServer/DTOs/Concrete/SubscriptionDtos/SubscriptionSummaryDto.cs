using SAPLSServer.Models;

namespace SAPLSServer.DTOs.Concrete.SubscriptionDtos
{
    public class SubscriptionSummaryDto
    {
        public string Id { get; set; } = null!;
        public string Name { get; set; } = null!;
        public int Duration { get; set; }
        public double Price { get; set; }
        public string Status { get; set; } = null!;
        public SubscriptionSummaryDto(Subscription model)
        {
            Id = model.Id;
            Name = model.Name;
            Duration = model.Duration;
            Price = model.Price;
            Status = model.Status;
        }
    }
}
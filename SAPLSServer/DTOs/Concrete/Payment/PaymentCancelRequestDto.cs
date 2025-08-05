using System.Text.Json.Serialization;

namespace SAPLSServer.DTOs.Concrete.PaymentDto
{
    public class PaymentCancelRequestDto
    {
        [JsonPropertyName("cancellationReason")]
        public string? CancellationReason { get; set; }
    }
}

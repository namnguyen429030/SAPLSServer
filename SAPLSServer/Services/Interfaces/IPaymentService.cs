using SAPLSServer.DTOs.Concrete.PaymentDto;

namespace SAPLSServer.Services.Interfaces
{
    public interface IPaymentService
    {
        Task<PaymentStatusResponseDto> SendPaymentRequest(PaymentRequestDto request);
        Task<PaymentStatusResponseDto> SendCancelPaymentRequest(PaymentCancelRequestDto request);
        Task<PaymentStatusResponseDto> GetPaymentStatus(int paymentId);
    }
}

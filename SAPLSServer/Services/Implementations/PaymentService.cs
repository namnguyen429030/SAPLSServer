using SAPLSServer.DTOs.Concrete.PaymentDto;
using SAPLSServer.Services.Interfaces;

namespace SAPLSServer.Services.Implementations
{
    public class PaymentService : IPaymentService
    {
        private readonly IConfiguration _configuration;
        public PaymentService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public Task<PaymentStatusResponseDto> GetPaymentStatus(int paymentId)
        {
            throw new NotImplementedException();
        }

        public Task<PaymentStatusResponseDto> SendCancelPaymentRequest(PaymentCancelRequestDto request)
        {
            throw new NotImplementedException();
        }

        public Task<PaymentStatusResponseDto> SendPaymentRequest(PaymentRequestDto request)
        {
            throw new NotImplementedException();
        }
    }
}

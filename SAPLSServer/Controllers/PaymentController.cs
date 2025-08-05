using Microsoft.AspNetCore.Mvc;
using SAPLSServer.DTOs.Concrete.PaymentDto;
using SAPLSServer.Services.Interfaces;

namespace SAPLSServer.Controllers
{
    /// <summary>
    /// Controller for managing payment operations including requests, cancellations, and status checks.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpPost("request")]
        public async Task<IActionResult> SendPaymentRequest([FromBody] PaymentRequestDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _paymentService.SendPaymentRequest(request);
            return Ok(result);
        }

        [HttpPost("cancel")]
        public async Task<IActionResult> SendCancelPaymentRequest([FromBody] PaymentCancelRequestDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _paymentService.SendCancelPaymentRequest(request);
            return Ok(result);
        }

        [HttpGet("status/{paymentId}")]
        public async Task<IActionResult> GetPaymentStatus(int paymentId)
        {
            var result = await _paymentService.GetPaymentStatus(paymentId);
            return Ok(result);
        }
    }
}
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SAPLSServer.Constants;
using SAPLSServer.DTOs.Concrete.PaymentDtos;
using SAPLSServer.Exceptions;
using SAPLSServer.Services.Interfaces;

namespace SAPLSServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        private readonly string _apiKey;
        private readonly string _clientKey;
        private readonly string _checkSumKey;
        public PaymentController(IPaymentService paymentService, IPaymentSettings paymentSettings)
        {
            _paymentService = paymentService;
            _apiKey = paymentSettings.PaymentGatewayApiKey;
            _clientKey = paymentSettings.PaymentGatewayClientKey;
            _checkSumKey = paymentSettings.PaymentGatewayCheckSumKey;
        }

        /// <summary>
        /// Creates a new payment request
        /// </summary>
        /// <param name="request">Payment request details</param>
        /// <returns>Payment status response with payment information</returns>
        [HttpPost("request")]
        public async Task<ActionResult<PaymentStatusResponseDto>> CreatePaymentRequest([FromBody] PaymentRequestDto request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var result = await _paymentService.SendPaymentRequest(request, _clientKey, 
                    _apiKey, _checkSumKey);
                
                if (result == null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, 
                        new { message = MessageKeys.PAYOS_SERVICE_UNAVAILABLE });
                }

                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(StatusCodes.Status503ServiceUnavailable, 
                    new { message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, 
                    new { message = MessageKeys.UNEXPECTED_ERROR });
            }
        }

        /// <summary>
        /// Gets the status of a specific payment
        /// </summary>
        /// <param name="paymentId">The payment ID to check status for</param>
        /// <returns>Payment status information</returns>
        [HttpGet("{paymentId}/status")]
        public async Task<ActionResult<PaymentStatusResponseDto>> GetPaymentStatus(int paymentId)
        {
            try
            {
                if (paymentId <= 0)
                {
                    return BadRequest(new { message = "Invalid payment ID" });
                }

                var result = await _paymentService.GetPaymentStatus(paymentId, _clientKey, _apiKey);
                
                if (result == null)
                {
                    return NotFound(new { message = "Payment not found" });
                }

                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(StatusCodes.Status503ServiceUnavailable, 
                    new { message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, 
                    new { message = MessageKeys.UNEXPECTED_ERROR });
            }
        }
        /// <summary>
        /// Cancels a payment request
        /// </summary>
        /// <param name="paymentId">The payment ID to cancel</param>
        /// <param name="request">Cancellation request details</param>
        /// <returns>Payment status response after cancellation</returns>
        [HttpPost("{paymentId}/cancel")]
        public async Task<ActionResult<PaymentStatusResponseDto>> CancelPayment(int paymentId, [FromBody] PaymentCancelRequestDto request)
        {
            try
            {
                if (paymentId <= 0)
                {
                    return BadRequest(new { message = "Invalid payment ID" });
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var result = await _paymentService.SendCancelPaymentRequest(paymentId, _clientKey, 
                    _apiKey, request);
                
                if (result == null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, 
                        new { message = MessageKeys.PAYOS_SERVICE_UNAVAILABLE });
                }

                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(StatusCodes.Status503ServiceUnavailable, 
                    new { message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, 
                    new { message = MessageKeys.UNEXPECTED_ERROR });
            }
        }
    }
}
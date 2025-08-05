using Microsoft.AspNetCore.Mvc;
using SAPLSServer.DTOs.Base;
using SAPLSServer.DTOs.Concrete.PaymentSourceDto;
using SAPLSServer.DTOs.PaginationDto;
using SAPLSServer.Services.Interfaces;

namespace SAPLSServer.Controllers
{
    /// <summary>
    /// Controller for managing payment source operations including creation, updates, retrieval, and deletion.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentSourceController : ControllerBase
    {
        private readonly IPaymentSourceService _paymentSourceService;

        public PaymentSourceController(IPaymentSourceService paymentSourceService)
        {
            _paymentSourceService = paymentSourceService;
        }

        [HttpPost]
        public async Task<IActionResult> CreatePaymentSource([FromBody] CreatePaymenSourceRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _paymentSourceService.CreatePaymentSource(request);
            return Ok(new { message = "Payment source created successfully" });
        }

        [HttpPut]
        public async Task<IActionResult> UpdatePaymentSource([FromBody] UpdatePaymentSourceRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _paymentSourceService.UpdatePaymentSource(request);
            return Ok(new { message = "Payment source updated successfully" });
        }

        [HttpDelete]
        public async Task<IActionResult> DeletePaymentSource([FromQuery] DeleteRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _paymentSourceService.DeletePaymentSource(request);
            return Ok(new { message = "Payment source deleted successfully" });
        }

        [HttpGet("details")]
        public async Task<IActionResult> GetPaymentSourceDetails([FromQuery] GetDetailsRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _paymentSourceService.GetPaymentSourceDetails(request);
            if (result == null)
                return NotFound(new { message = "Payment source not found" });

            return Ok(result);
        }

        [HttpGet("page")]
        public async Task<IActionResult> GetPaymentSourcesPage(
            [FromQuery] PageRequest pageRequest,
            [FromQuery] GetListRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _paymentSourceService.GetPaymentSourcesPage(pageRequest, request);
            return Ok(result);
        }

        [HttpGet("owner/{ownerId}")]
        public async Task<IActionResult> GetPaymentSourcesByOwner(string ownerId)
        {
            if (string.IsNullOrEmpty(ownerId))
                return BadRequest("Owner ID is required");

            var result = await _paymentSourceService.GetPaymentSourcesByOwner(ownerId);
            return Ok(result);
        }

        [HttpGet("owner/{ownerId}/page")]
        public async Task<IActionResult> GetPaymentSourcesByOwnerPage(
            string ownerId,
            [FromQuery] PageRequest pageRequest,
            [FromQuery] GetListRequest request)
        {
            if (string.IsNullOrEmpty(ownerId))
                return BadRequest("Owner ID is required");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _paymentSourceService.GetPaymentSourcesByOwnerPage(ownerId, pageRequest, request);
            return Ok(result);
        }

        [HttpGet("status/{status}/page")]
        public async Task<IActionResult> GetPaymentSourcesByStatusPage(
            string status,
            [FromQuery] PageRequest pageRequest,
            [FromQuery] GetListRequest request)
        {
            if (string.IsNullOrEmpty(status))
                return BadRequest("Status is required");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _paymentSourceService.GetPaymentSourcesByStatusPage(status, pageRequest, request);
            return Ok(result);
        }
    }
}
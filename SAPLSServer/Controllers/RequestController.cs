using Microsoft.AspNetCore.Mvc;
using SAPLSServer.DTOs.Base;
using SAPLSServer.DTOs.Concrete.RequestDto;
using SAPLSServer.DTOs.PaginationDto;
using SAPLSServer.Services.Interfaces;

namespace SAPLSServer.Controllers
{
    /// <summary>
    /// Controller for managing request operations including creation, updates, and retrieval.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class RequestController : ControllerBase
    {
        private readonly IRequestService _requestService;

        public RequestController(IRequestService requestService)
        {
            _requestService = requestService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateRequest([FromBody] CreateRequestRequest request, [FromQuery] string senderId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (string.IsNullOrEmpty(senderId))
                return BadRequest("Sender ID is required");

            await _requestService.CreateRequest(request, senderId);
            return Ok(new { message = "Request created successfully" });
        }

        [HttpPut]
        public async Task<IActionResult> UpdateRequest([FromBody] UpdateRequestRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _requestService.UpdateRequest(request);
            return Ok(new { message = "Request updated successfully" });
        }

        [HttpGet("details")]
        public async Task<IActionResult> GetRequestDetails([FromQuery] GetDetailsRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _requestService.GetRequestDetails(request);
            if (result == null)
                return NotFound(new { message = "Request not found" });

            return Ok(result);
        }

        [HttpGet("page")]
        public async Task<IActionResult> GetRequestsPage(
            [FromQuery] PageRequest pageRequest,
            [FromQuery] GetListRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _requestService.GetRequestsPage(pageRequest, request);
            return Ok(result);
        }

        [HttpGet("type/{type}/page")]
        public async Task<IActionResult> GetRequestsByTypePage(string type, [FromQuery] PageRequest request)
        {
            if (string.IsNullOrEmpty(type))
                return BadRequest("Type is required");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _requestService.GetRequestsByTypePage(type, request);
            return Ok(result);
        }

        [HttpGet("status/{status}/page")]
        public async Task<IActionResult> GetRequestsByStatusPage(string status, [FromQuery] PageRequest request)
        {
            if (string.IsNullOrEmpty(status))
                return BadRequest("Status is required");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _requestService.GetRequestsByStatusPage(status, request);
            return Ok(result);
        }

        [HttpGet("date-range/page")]
        public async Task<IActionResult> GetRequestsByDateRangePage(
            [FromQuery] DateTime startDate,
            [FromQuery] DateTime endDate,
            [FromQuery] PageRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _requestService.GetRequestsByDateRangePage(startDate, endDate, request);
            return Ok(result);
        }
    }
}
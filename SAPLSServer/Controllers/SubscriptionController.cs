using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SAPLSServer.Constants;
using SAPLSServer.DTOs.Concrete.SubscriptionDtos;
using SAPLSServer.DTOs.PaginationDto;
using SAPLSServer.Services.Interfaces;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SAPLSServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SubscriptionController : ControllerBase
    {
        private readonly ISubscriptionService _subscriptionService;

        public SubscriptionController(ISubscriptionService subscriptionService)
        {
            _subscriptionService = subscriptionService;
        }

        /// <summary>
        /// Get a filtered list of subscriptions.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetList([FromQuery] GetSubscriptionListRequest request)
        {
            var result = await _subscriptionService.GetListAsync(request);
            return Ok(result);
        }

        /// <summary>
        /// Get a paginated, filtered list of subscriptions.
        /// </summary>
        [HttpGet("page")]
        public async Task<IActionResult> GetPage([FromQuery] PageRequest pageRequest, [FromQuery] GetSubscriptionListRequest request)
        {
            var result = await _subscriptionService.GetPageAsync(pageRequest, request);
            return Ok(result);
        }

        /// <summary>
        /// Get details of a specific subscription.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDetails(string id)
        {
            var result = await _subscriptionService.GetDetailsAsync(id);
            if (result == null)
                return NotFound();
            return Ok(result);
        }

        /// <summary>
        /// CheckIn a new subscription.
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] CreateSubscriptionRequest request)
        {
            var adminId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if(adminId == null)
            {
                return Unauthorized(new { message = MessageKeys.UNAUTHORIZED_ACCESS });
            }
            await _subscriptionService.CreateAsync(request, adminId);
            return NoContent();
        }

        /// <summary>
        /// Update the status of a subscription.
        /// </summary>
        [HttpPut("status")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateStatus([FromBody] UpdateSubscriptionStatusRequest request)
        {
            var adminId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (adminId == null)
            {
                return Unauthorized(new { message = MessageKeys.UNAUTHORIZED_ACCESS });
            }
            await _subscriptionService.UpdateStatusAsync(request, adminId);
            return NoContent();
        }
    }
}
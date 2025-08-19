using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SAPLSServer.Constants;
using SAPLSServer.DTOs.Concrete.SubscriptionDtos;
using SAPLSServer.DTOs.PaginationDto;
using SAPLSServer.Exceptions;
using SAPLSServer.Services.Interfaces;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SAPLSServer.Controllers {
    [ApiController]
    [Route("api/[controller]")]
    public class SubscriptionController : ControllerBase {
        private readonly ISubscriptionService _subscriptionService;

        public SubscriptionController(ISubscriptionService subscriptionService) {
            _subscriptionService = subscriptionService;
        }

        /// <summary>
        /// Get a filtered list of subscriptions.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetList([FromQuery] GetSubscriptionListRequest request) {
            var result = await _subscriptionService.GetListAsync(request);
            return Ok(result);
        }

        /// <summary>
        /// Get a paginated, filtered list of subscriptions.
        /// </summary>
        [HttpGet("page")]
        public async Task<IActionResult> GetPage([FromQuery] PageRequest pageRequest, [FromQuery] GetSubscriptionListRequest request) {
            var result = await _subscriptionService.GetPageAsync(pageRequest, request);
            return Ok(result);
        }

        /// <summary>
        /// Get details of a specific subscription.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDetails(string id) {
            var result = await _subscriptionService.GetDetailsAsync(id);
            if (result == null)
                return NotFound();
            return Ok(result);
        }

        /// <summary>
        /// Create a new subscription.
        /// </summary>
        [HttpPost]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] CreateSubscriptionRequest request) {
            try {
                if (!ModelState.IsValid) {
                    return BadRequest(ModelState);
                }
                //var adminId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                //if (adminId == null) {
                //    return Unauthorized(new { message = MessageKeys.UNAUTHORIZED_ACCESS });
                //}
                await _subscriptionService.CreateAsync(request, "1001");
                return NoContent();
            }
            catch (InvalidInformationException ex) {
                return BadRequest(new { message = ex.Message });
            }
            catch {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = MessageKeys.UNEXPECTED_ERROR });
            }
        }

        /// <summary>
        /// Update an existing subscription.
        /// </summary>
        [HttpPut]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update([FromBody] UpdateSubscriptionRequest request) {
            try {
                if (!ModelState.IsValid) {
                    return BadRequest(ModelState);
                }
                //var adminId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                //if (adminId == null) {
                //    return Unauthorized(new { message = MessageKeys.UNAUTHORIZED_ACCESS });
                //}
                await _subscriptionService.UpdateAsync(request, "1001");
                return NoContent();
            }
            catch (InvalidInformationException ex) {
                return BadRequest(new { message = ex.Message });
            }
            catch {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = MessageKeys.UNEXPECTED_ERROR });
            }
        }

        /// <summary>
        /// Update the status of a subscription.
        /// </summary>
        [HttpPut("status")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateStatus([FromBody] UpdateSubscriptionStatusRequest request) {
            try {
                if (!ModelState.IsValid) {
                    return BadRequest(ModelState);
                }
                var adminId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (adminId == null) {
                    return Unauthorized(new { message = MessageKeys.UNAUTHORIZED_ACCESS });
                }
                await _subscriptionService.UpdateStatusAsync(request, "1001");
                return NoContent();
            }
            catch (InvalidInformationException ex) {
                return BadRequest(new { message = ex.Message });
            }
            catch {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = MessageKeys.UNEXPECTED_ERROR });
            }
        }
    }
}
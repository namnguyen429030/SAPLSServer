using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SAPLSServer.Constants;
using SAPLSServer.DTOs.Base;
using SAPLSServer.DTOs.Concrete.ParkingLotDtos;
using SAPLSServer.DTOs.Concrete.PaymentDtos;
using SAPLSServer.DTOs.PaginationDto;
using SAPLSServer.Exceptions;
using SAPLSServer.Services.Implementations;
using SAPLSServer.Services.Interfaces;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;

namespace SAPLSServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ParkingLotController : ControllerBase
    {
        private readonly IParkingLotService _parkingLotService;
        private readonly ILogger<ParkingLotController> _logger;
        public ParkingLotController(IParkingLotService parkingLotService, ILogger<ParkingLotController> logger)
        {
            _parkingLotService = parkingLotService;
            _logger = logger;
        }

        /// <summary>
        /// Creates a new parking lot.
        /// </summary>
        [HttpPost]
        [Authorize(Policy = Accessibility.ADMIN_ACCESS)]
        public async Task<IActionResult> Create([FromBody] CreateParkingLotRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var performerAdminId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
            await _parkingLotService.CreateParkingLot(request, performerAdminId);
            return Ok(new { message = MessageKeys.PARKING_LOT_CREATED_SUCCESSFULLY });
        }

        /// <summary>
        /// Updates the basic information of a parking lot.
        /// </summary>
        [HttpPut("basic-info")]
        [Authorize(Policy = Accessibility.PARKING_LOT_OWNER_ACCESS)]
        public async Task<IActionResult> UpdateBasicInfo([FromBody] UpdateParkingLotBasicInformationRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var performerId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
            await _parkingLotService.UpdateParkingLotBasicInformation(request, performerId);
            return Ok(new { message = MessageKeys.PARKING_LOT_BASIC_INFO_UPDATED_SUCCESSFULLY });
        }

        /// <summary>
        /// Updates the address of a parking lot.
        /// </summary>
        [HttpPut("address")]
        [Authorize(Policy = Accessibility.ADMIN_ACCESS)]
        public async Task<IActionResult> UpdateAddress([FromBody] UpdateParkingLotAddressRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var performerAdminId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
            await _parkingLotService.UpdateParkingLotAddress(request, performerAdminId);
            return Ok(new { message = MessageKeys.PARKING_LOT_ADDRESS_UPDATED_SUCCESSFULLY });
        }

        /// <summary>
        /// Updates the subscription of a parking lot.
        /// </summary>
        [HttpPut("subscription")]
        [Authorize(Policy = Accessibility.PARKING_LOT_OWNER_ACCESS)]
        public async Task<IActionResult> UpdateSubscription([FromBody] UpdateParkingLotSubscriptionRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var performerId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
            await _parkingLotService.UpdateParkingLotSubscription(request, performerId);
            return Ok(new { message = MessageKeys.PARKING_LOT_BASIC_INFO_UPDATED_SUCCESSFULLY });
        }

        /// <summary>
        /// Gets the details of a parking lot by ID.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDetails(string id)
        {
            var result = await _parkingLotService.GetParkingLotDetails(id);
            if (result == null)
                return NotFound(new { message = MessageKeys.PARKING_LOT_NOT_FOUND });
            return Ok(result);
        }

        /// <summary>
        /// Gets a paginated list of parking lots.
        /// </summary>
        [HttpGet("page")]
        public async Task<IActionResult> GetPage([FromQuery] PageRequest pageRequest, [FromQuery] GetParkingLotListRequest request)
        {
            var result = await _parkingLotService.GetParkingLotsPage(pageRequest, request);
            return Ok(result);
        }

        /// <summary>
        /// Gets a list of parking lots.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetList([FromQuery] GetParkingLotListRequest request)
        {
            var result = await _parkingLotService.GetParkingLots(request);
            return Ok(result);
        }

        /// <summary>
        /// Deletes a parking lot.
        /// </summary>
        [HttpDelete]
        [Authorize(Policy = Accessibility.HEAD_ADMIN_ACCESS)]
        public async Task<IActionResult> Delete([FromBody] DeleteRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _parkingLotService.DeleteParkingLot(request);
            return Ok(new { message = MessageKeys.PARKING_LOT_DELETED_SUCCESSFULLY });
        }
        [HttpPost("complete-payment")]
        [AllowAnonymous]
        public async Task<IActionResult> CompletePayment([FromBody] PaymentWebHookRequest paymentWebHookRequest)
        {
            // Log the incoming webhook request
            _logger.LogInformation("Received PaymentWebHookRequest: {Request}", JsonSerializer.Serialize(paymentWebHookRequest));
            await _parkingLotService.ConfirmTransaction(paymentWebHookRequest);
            return Ok();
        }
        [HttpGet("subscription/{parkingLotId}")]
        [Authorize(Policy = Accessibility.ADMIN_PARKINGLOT_OWNER_ACCESS)]
        public async Task<IActionResult> GetSubscriptionByParkingLotId(string parkingLotId)
        {
            try
            {
                var result = await _parkingLotService.GetSubscriptionByParkingLotId(parkingLotId);
                if (result == null)
                    return NotFound(new { message = MessageKeys.SUBSCRIPTION_NOT_FOUND });
                return Ok(result);
            }
            catch(InvalidInformationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = MessageKeys.UNEXPECTED_ERROR });
            }
        }
    }
}
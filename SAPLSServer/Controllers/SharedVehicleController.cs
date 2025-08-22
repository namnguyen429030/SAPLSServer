using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SAPLSServer.Constants;
using SAPLSServer.DTOs.Concrete.SharedVehicleDtos;
using SAPLSServer.DTOs.PaginationDto;
using SAPLSServer.Exceptions;
using SAPLSServer.Services.Interfaces;
using System.Security.Claims;

namespace SAPLSServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class SharedVehicleController : ControllerBase
    {
        private readonly ISharedVehicleService _sharedVehicleService;

        public SharedVehicleController(ISharedVehicleService sharedVehicleService)
        {
            _sharedVehicleService = sharedVehicleService;
        }

        /// <summary>
        /// Creates a new shared vehicle invitation
        /// </summary>
        /// <param name="request">Shared vehicle creation request</param>
        /// <returns>Success response</returns>
        [HttpPost("share")]
        [Authorize(Policy = Accessibility.CLIENT_ACCESS)]
        public async Task<IActionResult> ShareVehicle([FromBody] CreateSharedVehicleRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                await _sharedVehicleService.Create(request);
                return Ok(new { message = MessageKeys.VEHICLE_SHARED_SUCCESSFULLY });
            }
            catch (UnauthorizedAccessException)
            {
                return StatusCode(403, new { message = MessageKeys.UNAUTHORIZED_ACCESS });
            }
            catch (InvalidInformationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = MessageKeys.UNEXPECTED_ERROR });
            }
        }

        [HttpGet("page")]
        [Authorize]
        public async Task<IActionResult> GetSharedVehiclesPage([FromQuery] PageRequest pageRequest,
            [FromQuery] GetSharedVehicleList request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrWhiteSpace(userId))
                {
                    return Unauthorized(new { message = MessageKeys.UNAUTHORIZED_ACCESS });
                }
                var result = await _sharedVehicleService.GetSharedVehiclesPage(pageRequest, request);
                return Ok(result);
            }
            catch (UnauthorizedAccessException)
            {
                return StatusCode(403, new { message = MessageKeys.UNAUTHORIZED_ACCESS });
            }
            catch (InvalidInformationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = MessageKeys.UNEXPECTED_ERROR });
            }
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetSharedVehicles([FromQuery] GetSharedVehicleList request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrWhiteSpace(userId))
                {
                    return Unauthorized(new { message = MessageKeys.UNAUTHORIZED_ACCESS });
                }
                var result = await _sharedVehicleService.GetSharedVehiclesList(request);
                return Ok(result);
            }
            catch (UnauthorizedAccessException)
            {
                return StatusCode(403, new { message = MessageKeys.UNAUTHORIZED_ACCESS });
            }
            catch (InvalidInformationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = MessageKeys.UNEXPECTED_ERROR });
            }
        }

        [HttpGet("{sharedVehicleId}")]
        [Authorize]
        public async Task<IActionResult> GetSharedVehicleById(string sharedVehicleId)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrWhiteSpace(userId))
                {
                    return Unauthorized(new { message = MessageKeys.UNAUTHORIZED_ACCESS });
                }
                var vehicle = await _sharedVehicleService.GetSharedVehicleDetails(sharedVehicleId, userId);
                if (vehicle == null)
                {
                    return NotFound(new { message = MessageKeys.VEHICLE_NOT_FOUND });
                }
                return Ok(vehicle);
            }
            catch (UnauthorizedAccessException)
            {
                return StatusCode(403, new { message = MessageKeys.UNAUTHORIZED_ACCESS });
            }
            catch (InvalidInformationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = MessageKeys.UNEXPECTED_ERROR });
            }
        }

        [HttpPost("{id}/accept")]
        [Authorize(Policy = Accessibility.CLIENT_ACCESS)]
        public async Task<IActionResult> AcceptSharedVehicle(string id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrWhiteSpace(userId))
                return Unauthorized(new { message = MessageKeys.UNAUTHORIZED_ACCESS });

            await _sharedVehicleService.AcceptSharedVehicle(id, userId);
            return Ok(new { message = MessageKeys.VEHICLE_SHARE_ACCEPTED });
        }

        [HttpPost("{id}/reject")]
        [Authorize(Policy = Accessibility.CLIENT_ACCESS)]
        public async Task<IActionResult> RejectSharedVehicle(string id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrWhiteSpace(userId))
                return Unauthorized(new { message = MessageKeys.UNAUTHORIZED_ACCESS });

            await _sharedVehicleService.RejectSharedVehicle(id, userId);
            return Ok(new { message = MessageKeys.VEHICLE_SHARE_REJECTED });
        }

        [HttpPost("{id}/recall")]
        [Authorize(Policy = Accessibility.CLIENT_ACCESS)]
        public async Task<IActionResult> RecallSharedVehicle(string id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrWhiteSpace(userId))
                return Unauthorized(new { message = MessageKeys.UNAUTHORIZED_ACCESS });

            await _sharedVehicleService.RecallSharedVehicle(id, userId);
            return Ok(new { message = MessageKeys.VEHICLE_SHARE_RECALLED });
        }
        /// <summary>
        /// Gets detailed information about a shared vehicle by its vehicle ID.
        /// </summary>
        /// <param name="vehicleId">The unique identifier of the vehicle.</param>
        /// <returns>Shared vehicle details if found; otherwise, NotFound.</returns>
        [HttpGet("by-vehicle/{vehicleId}")]
        [Authorize(Policy = Accessibility.CLIENT_ACCESS)]
        public async Task<IActionResult> GetByVehicleId(string vehicleId)
        {
            try
            {
                var result = await _sharedVehicleService.GetByVehicleId(vehicleId);
                if (result == null)
                {
                    return NotFound(new { message = MessageKeys.SHARED_VEHICLE_NOT_FOUND });
                }
                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = MessageKeys.UNEXPECTED_ERROR });
            }
        }
    }
}
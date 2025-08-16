using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SAPLSServer.Constants;
using SAPLSServer.DTOs.Base;
using SAPLSServer.DTOs.Concrete.VehicleDtos;
using SAPLSServer.DTOs.PaginationDto;
using SAPLSServer.Exceptions;
using SAPLSServer.Services.Interfaces;
using System.Security.Claims;

namespace SAPLSServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class VehicleController : ControllerBase
    {
        private readonly IVehicleService _vehicleService;

        public VehicleController(IVehicleService vehicleService)
        {
            _vehicleService = vehicleService;
        }

        /// <summary>
        /// Registers a new vehicle in the system
        /// </summary>
        /// <param name="request">Vehicle registration request with registration certificates</param>
        /// <returns>Success response</returns>
        [HttpPost("register")]
        [Authorize(Policy = Accessibility.CLIENT_ACCESS)]
        public async Task<IActionResult> RegisterVehicle([FromForm] CreateVehicleRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;
                await _vehicleService.Create(request, currentUserId);
                return Ok(new { message = MessageKeys.VEHICLE_CREATED_SUCCESSFULLY });
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

        /// <summary>
        /// Updates an existing vehicle's information
        /// </summary>
        /// <param name="request">Vehicle update request</param>
        /// <returns>Success response</returns>
        [HttpPut]
        [Authorize(Policy = Accessibility.CLIENT_ACCESS)]
        public async Task<IActionResult> UpdateVehicle([FromForm] UpdateVehicleRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;
                await _vehicleService.Update(request, currentUserId);
                return Ok(new { message = MessageKeys.VEHICLE_UPDATED_SUCCESSFULLY });
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

        /// <summary>
        /// Updates a vehicle's status (active/inactive)
        /// </summary>
        /// <param name="request">Vehicle status update request</param>
        /// <returns>Success response</returns>
        [HttpPut("status")]
        [Authorize(Policy = Accessibility.CLIENT_ACCESS)]
        public async Task<IActionResult> UpdateVehicleStatus([FromBody] UpdateVehicleStatusRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;
                await _vehicleService.UpdateStatus(request, currentUserId);
                return Ok(new { message = MessageKeys.VEHICLE_UPDATED_SUCCESSFULLY });
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

        /// <summary>
        /// Gets vehicle details by vehicle ID
        /// </summary>
        /// <param name="id">Vehicle ID</param>
        /// <returns>Vehicle details</returns>
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<VehicleDetailsDto>> GetVehicleById(string id)
        {
            try
            {
                var result = await _vehicleService.GetById(id);
                if (result == null)
                {
                    return NotFound(new { message = MessageKeys.VEHICLE_NOT_FOUND });
                }

                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, 
                    new { message = MessageKeys.UNEXPECTED_ERROR });
            }
        }

        /// <summary>
        /// Gets vehicle details by license plate
        /// </summary>
        /// <param name="licensePlate">Vehicle license plate</param>
        /// <returns>Vehicle details</returns>
        [HttpGet("license-plate/{licensePlate}")]
        [Authorize(Policy = Accessibility.ADMIN_PARKINGLOT_OWNER_ACCESS)]
        public async Task<ActionResult<VehicleDetailsDto>> GetVehicleByLicensePlate(string licensePlate)
        {
            try
            {
                var result = await _vehicleService.GetByLicensePlate(licensePlate);
                if (result == null)
                {
                    return NotFound(new { message = MessageKeys.VEHICLE_NOT_FOUND });
                }

                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, 
                    new { message = MessageKeys.UNEXPECTED_ERROR });
            }
        }

        /// <summary>
        /// Gets paginated list of vehicles for a specific owner
        /// </summary>
        /// <param name="pageRequest">Pagination parameters</param>
        /// <param name="request">Filter criteria (OwnerId required)</param>
        /// <returns>Paginated vehicle list</returns>
        [HttpGet("page")]
        [Authorize]
        public async Task<ActionResult<PageResult<VehicleSummaryDto>>> GetVehiclesPage([FromQuery] PageRequest pageRequest, [FromQuery] GetVehicleListRequest request)
        {
            try
            {
                var result = await _vehicleService.GetVehiclesPage(pageRequest, request);
                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, 
                    new { message = MessageKeys.UNEXPECTED_ERROR });
            }
        }

        /// <summary>
        /// Gets list of vehicles for a specific owner
        /// </summary>
        /// <param name="request">Filter criteria (OwnerId required)</param>
        /// <returns>List of vehicles</returns>
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<List<VehicleSummaryDto>>> GetVehiclesList([FromQuery] GetVehicleListRequest request)
        {
            try
            {
                var result = await _vehicleService.GetAllVehicles(request);
                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, 
                    new { message = MessageKeys.UNEXPECTED_ERROR });
            }
        }

        /// <summary>
        /// Deletes a vehicle from the system
        /// </summary>
        /// <param name="request">Delete request with vehicle ID</param>
        /// <returns>Success response</returns>
        [HttpDelete]
        [Authorize(Policy = Accessibility.CLIENT_ACCESS)]
        public async Task<IActionResult> DeleteVehicle([FromBody] DeleteRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;
                await _vehicleService.DeleteVehicle(request, currentUserId);
                return Ok(new { message = MessageKeys.VEHICLE_DELETED_SUCCESSFULLY });
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

        /// <summary>
        /// Gets vehicles owned by the current user
        /// </summary>
        /// <returns>List of user's vehicles</returns>
        [HttpGet("my-vehicles")]
        [Authorize(Policy = Accessibility.CLIENT_ACCESS)]
        public async Task<ActionResult<List<VehicleSummaryDto>>> GetMyVehicles([FromQuery] string? status = null, [FromQuery] string? sharingStatus = null)
        {
            try
            {
                var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrWhiteSpace(currentUserId))
                {
                    return Unauthorized(new { message = MessageKeys.UNAUTHORIZED_ACCESS });
                }

                var request = new GetVehicleListRequest
                {
                    OwnerId = currentUserId,
                    Status = status,
                    SharingStatus = sharingStatus
                };

                var result = await _vehicleService.GetAllVehicles(request);
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
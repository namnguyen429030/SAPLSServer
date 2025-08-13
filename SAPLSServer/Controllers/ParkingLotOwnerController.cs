using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SAPLSServer.Constants;
using SAPLSServer.DTOs.Base;
using SAPLSServer.DTOs.Concrete.UserDtos;
using SAPLSServer.DTOs.PaginationDto;
using SAPLSServer.Exceptions;
using SAPLSServer.Services.Interfaces;

namespace SAPLSServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ParkingLotOwnerController : ControllerBase
    {
        private readonly IParkingLotOwnerService _parkingLotOwnerService;

        public ParkingLotOwnerController(IParkingLotOwnerService parkingLotOwnerService)
        {
            _parkingLotOwnerService = parkingLotOwnerService;
        }

        /// <summary>
        /// Registers a new parking lot owner profile (Only HeadAdmin can create)
        /// </summary>
        /// <param name="request">Parking lot owner profile creation request</param>
        /// <returns>Success response</returns>
        [HttpPost("register")]
        [Authorize(Policy = Accessibility.HEAD_ADMIN_ACCESS)]
        public async Task<IActionResult> RegisterParkingLotOwner([FromBody] CreateParkingLotOwnerProfileRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                await _parkingLotOwnerService.Create(request);
                return Ok(new { message = MessageKeys.PARKING_LOT_OWNER_PROFILE_CREATED_SUCCESSFULLY });
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
        /// Updates an existing parking lot owner profile
        /// </summary>
        /// <param name="request">Parking lot owner profile update request</param>
        /// <returns>Success response</returns>
        [HttpPut]
        [Authorize(Policy = Accessibility.PARKING_LOT_OWNER_ACCESS)]
        public async Task<IActionResult> UpdateParkingLotOwner([FromBody] UpdateParkingLotOwnerProfileRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                await _parkingLotOwnerService.Update(request);
                return Ok(new { message = MessageKeys.PARKING_LOT_OWNER_PROFILE_UPDATED_SUCCESSFULLY });
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
        /// Gets parking lot owner profile by owner ID
        /// </summary>
        /// <param name="ownerId">Parking lot owner ID</param>
        /// <returns>Parking lot owner profile details</returns>
        [HttpGet("{ownerId}")]
        [Authorize(Policy = Accessibility.WEB_APP_ACCESS)]
        public async Task<ActionResult<ParkingLotOwnerProfileDetailsDto>> GetByOwnerId(string ownerId)
        {
            try
            {
                var result = await _parkingLotOwnerService.GetByParkingLotOwnerId(ownerId);
                if (result == null)
                {
                    return NotFound(new { message = MessageKeys.PARKING_LOT_OWNER_PROFILE_NOT_FOUND });
                }

                return Ok(result);
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
        /// Gets parking lot owner profile by user ID
        /// </summary>
        /// <param name="userId">User ID</param>
        /// <returns>Parking lot owner profile details</returns>
        [HttpGet("user/{userId}")]
        [Authorize(Policy = Accessibility.WEB_APP_ACCESS)]
        public async Task<ActionResult<ParkingLotOwnerProfileDetailsDto>> GetByUserId(string userId)
        {
            try
            {
                var result = await _parkingLotOwnerService.GetByUserId(userId);
                if (result == null)
                {
                    return NotFound(new { message = MessageKeys.PARKING_LOT_OWNER_PROFILE_NOT_FOUND });
                }

                return Ok(result);
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
        /// Gets paginated list of parking lot owner profiles
        /// </summary>
        /// <param name="pageRequest">Pagination parameters</param>
        /// <param name="request">Filter criteria</param>
        /// <returns>Paginated parking lot owner profiles</returns>
        [HttpGet("page")]
        [Authorize(Policy = Accessibility.WEB_APP_ACCESS)]
        public async Task<ActionResult<PageResult<ParkingLotOwnerProfileSummaryDto>>> GetParkingLotOwnerProfilesPage([FromQuery] PageRequest pageRequest, [FromQuery] GetParkingLotOwnerListRequest request)
        {
            try
            {
                var result = await _parkingLotOwnerService.GetParkingLotOwnerProfilesPage(pageRequest, request);
                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, 
                    new { message = MessageKeys.UNEXPECTED_ERROR });
            }
        }

        /// <summary>
        /// Gets list of parking lot owner profiles
        /// </summary>
        /// <param name="request">Filter criteria</param>
        /// <returns>List of parking lot owner profiles</returns>
        [HttpGet]
        [Authorize(Policy = Accessibility.WEB_APP_ACCESS)]
        public async Task<ActionResult<List<ParkingLotOwnerProfileSummaryDto>>> GetParkingLotOwnerProfiles([FromQuery] GetParkingLotOwnerListRequest request)
        {
            try
            {
                var result = await _parkingLotOwnerService.GetParkingLotOwnerProfiles(request);
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
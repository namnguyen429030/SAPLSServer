using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SAPLSServer.Constants;
using SAPLSServer.DTOs.Concrete;
using SAPLSServer.DTOs.Concrete.UserDtos;
using SAPLSServer.DTOs.PaginationDto;
using SAPLSServer.Exceptions;
using SAPLSServer.Services.Interfaces;

namespace SAPLSServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class StaffController : ControllerBase
    {
        private readonly IStaffService _staffService;

        public StaffController(IStaffService staffService)
        {
            _staffService = staffService;
        }

        /// <summary>
        /// Registers a new staff profile (Only ParkingLotOwner can create staff)
        /// </summary>
        /// <param name="request">Staff profile creation request</param>
        /// <returns>Success response</returns>
        [HttpPost("register")]
        [Authorize(Policy = Accessibility.PARKING_LOT_OWNER_ACCESS)]
        public async Task<IActionResult> RegisterStaff([FromBody] CreateStaffProfileRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                await _staffService.Create(request);
                return Ok(new { message = MessageKeys.STAFF_PROFILE_CREATED_SUCCESSFULLY });
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
        /// Updates an existing staff profile
        /// </summary>
        /// <param name="request">Staff profile update request</param>
        /// <returns>Success response</returns>
        [HttpPut]
        [Authorize(Policy = Accessibility.STAFF_ACCESS)]
        public async Task<IActionResult> UpdateStaff([FromBody] UpdateStaffProfileRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                await _staffService.Update(request);
                return Ok(new { message = MessageKeys.STAFF_PROFILE_UPDATED_SUCCESSFULLY });
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
        /// Gets staff profile by staff ID
        /// </summary>
        /// <param name="staffId">Staff ID</param>
        /// <returns>Staff profile details</returns>
        [HttpGet("{staffId}")]
        [Authorize(Policy = Accessibility.STAFF_ACCESS)]
        public async Task<ActionResult<StaffProfileDetailsDto>> GetByStaffId(string staffId)
        {
            try
            {
                var result = await _staffService.FindByStaffId(staffId);
                if (result == null)
                {
                    return NotFound(new { message = MessageKeys.STAFF_PROFILE_NOT_FOUND });
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
        /// Gets staff profile by user ID
        /// </summary>
        /// <param name="userId">User ID</param>
        /// <returns>Staff profile details</returns>
        [HttpGet("user/{userId}")]
        [Authorize(Policy = Accessibility.STAFF_ACCESS)]
        public async Task<ActionResult<StaffProfileDetailsDto>> GetByUserId(string userId)
        {
            try
            {
                var result = await _staffService.FindByUserId(userId);
                if (result == null)
                {
                    return NotFound(new { message = MessageKeys.STAFF_PROFILE_NOT_FOUND });
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
        /// Gets paginated list of staff profiles
        /// </summary>
        /// <param name="pageRequest">Pagination parameters</param>
        /// <param name="request">Filter criteria</param>
        /// <returns>Paginated staff profiles</returns>
        [HttpGet("page")]
        [Authorize(Policy = Accessibility.ADMIN_PARKINGLOT_OWNER_ACCESS)]
        public async Task<ActionResult<PageResult<StaffProfileSummaryDto>>> GetStaffProfilesPage([FromQuery] PageRequest pageRequest, [FromQuery] GetStaffListRequest request)
        {
            try
            {
                var result = await _staffService.GetStaffProfilesPage(pageRequest, request);
                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, 
                    new { message = MessageKeys.UNEXPECTED_ERROR });
            }
        }

        /// <summary>
        /// Gets list of staff profiles
        /// </summary>
        /// <param name="request">Filter criteria</param>
        /// <returns>List of staff profiles</returns>
        [HttpGet]
        [Authorize(Policy = Accessibility.ADMIN_PARKINGLOT_OWNER_ACCESS)]
        public async Task<ActionResult<List<StaffProfileSummaryDto>>> GetStaffProfiles([FromQuery] GetStaffListRequest request)
        {
            try
            {
                var result = await _staffService.GetStaffProfiles(request);
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
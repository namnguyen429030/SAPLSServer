using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SAPLSServer.Constants;
using SAPLSServer.DTOs.Base;
using SAPLSServer.DTOs.Concrete.UserDtos;
using SAPLSServer.DTOs.PaginationDto;
using SAPLSServer.Exceptions;
using SAPLSServer.Services.Interfaces;
using System.Security.Claims;

namespace SAPLSServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        /// <summary>
        /// Registers a new admin profile (Only HeadAdmin can create new admins)
        /// </summary>
        /// <param name="request">Admin profile creation request</param>
        /// <returns>Success response</returns>
        [HttpPost("register")]
        [Authorize(Policy = Accessibility.HEAD_ADMIN_ACCESS)]
        public async Task<IActionResult> RegisterAdmin([FromBody] CreateAdminProfileRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var performedByAdminUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (performedByAdminUserId == null)
                {
                    return Unauthorized(new { message = MessageKeys.UNAUTHORIZED_ACCESS });
                }
                await _adminService.Create(request, performedByAdminUserId);
                return Ok(new { message = MessageKeys.ADMIN_PROFILE_CREATED_SUCCESSFULLY });
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
        /// Updates an existing admin profile
        /// </summary>
        /// <param name="request">Admin profile update request</param>
        /// <returns>Success response</returns>
        [HttpPut]
        [Authorize(Policy = Accessibility.HEAD_ADMIN_ACCESS)]
        public async Task<IActionResult> UpdateAdmin([FromBody] UpdateAdminProfileRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var performedByAdminUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (performedByAdminUserId == null)
                {
                    return Unauthorized(new { message = MessageKeys.UNAUTHORIZED_ACCESS });
                }
                await _adminService.Update(request, performedByAdminUserId);
                return Ok(new { message = MessageKeys.ADMIN_PROFILE_UPDATED_SUCCESSFULLY });
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
        /// Gets admin profile by admin ID
        /// </summary>
        /// <param name="adminId">Admin ID</param>
        /// <returns>Admin profile details</returns>
        [HttpGet("{adminId}")]
        [Authorize(Policy = Accessibility.WEB_APP_ACCESS)]
        public async Task<ActionResult<AdminProfileDetailsDto>> GetByAdminId(string adminId)
        {
            try
            {
                var result = await _adminService.GetByAdminIdAsync(adminId);
                if (result == null)
                {
                    return NotFound(new { message = MessageKeys.ADMIN_PROFILE_NOT_FOUND });
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
        /// Gets admin profile by user ID
        /// </summary>
        /// <param name="userId">User ID</param>
        /// <returns>Admin profile details</returns>
        [HttpGet("user/{userId}")]
        [Authorize(Policy = Accessibility.WEB_APP_ACCESS)]
        public async Task<ActionResult<AdminProfileDetailsDto>> GetByUserId(string userId)
        {
            try
            {
                var result = await _adminService.GetByUserIdAsync(userId);
                if (result == null)
                {
                    return NotFound(new { message = MessageKeys.ADMIN_PROFILE_NOT_FOUND });
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
        /// Gets paginated list of admin profiles
        /// </summary>
        /// <param name="pageRequest">Pagination parameters</param>
        /// <param name="request">Filter criteria</param>
        /// <returns>Paginated admin profiles</returns>
        [HttpGet("page")]
        [Authorize(Policy = Accessibility.WEB_APP_ACCESS)]
        public async Task<ActionResult<PageResult<AdminProfileSummaryDto>>> GetAdminProfilesPage([FromQuery] PageRequest pageRequest, [FromQuery] GetAdminListRequest request)
        {
            try
            {
                var result = await _adminService.GetAdminProfilesPage(pageRequest, request);
                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, 
                    new { message = MessageKeys.UNEXPECTED_ERROR });
            }
        }

        /// <summary>
        /// Gets list of admin profiles
        /// </summary>
        /// <param name="request">Filter criteria</param>
        /// <returns>List of admin profiles</returns>
        [HttpGet]
        [Authorize(Policy = Accessibility.WEB_APP_ACCESS)]
        public async Task<ActionResult<List<AdminProfileSummaryDto>>> GetAdminProfiles([FromQuery] GetAdminListRequest request)
        {
            try
            {
                var result = await _adminService.GetAdminProfiles(request);
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
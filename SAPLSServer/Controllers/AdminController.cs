using Microsoft.AspNetCore.Mvc;
using SAPLSServer.DTOs.Base;
using SAPLSServer.DTOs.Concrete.UserDto;
using SAPLSServer.DTOs.PaginationDto;
using SAPLSServer.Services.Interfaces;

namespace SAPLSServer.Controllers
{
    /// <summary>
    /// Controller for managing admin profile operations including creation, updates, and retrieval.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;

        /// <summary>
        /// Initializes a new instance of the AdminController.
        /// </summary>
        /// <param name="adminService">The admin service dependency.</param>
        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        /// <summary>
        /// Creates a new admin profile.
        /// </summary>
        /// <param name="request">The request containing admin profile details.</param>
        /// <returns>Ok result if successful.</returns>
        [HttpPost]
        public async Task<IActionResult> CreateAdmin([FromBody] CreateAdminProfileRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _adminService.CreateAdmin(request);
            return Ok(new { message = "Admin profile created successfully" });
        }

        /// <summary>
        /// Updates an existing admin profile.
        /// </summary>
        /// <param name="request">The request containing updated admin profile details.</param>
        /// <returns>Ok result if successful.</returns>
        [HttpPut]
        public async Task<IActionResult> UpdateAdmin([FromBody] UpdateAdminProfileRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _adminService.UpdateAdmin(request);
            return Ok(new { message = "Admin profile updated successfully" });
        }

        /// <summary>
        /// Retrieves the details of a specific admin profile.
        /// </summary>
        /// <param name="request">The request containing admin profile identifier.</param>
        /// <returns>The admin profile details or NotFound if not exists.</returns>
        [HttpGet("details")]
        public async Task<IActionResult> GetAdminProfileDetails([FromQuery] GetDetailsRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _adminService.GetAdminProfileDetails(request);
            if (result == null)
                return NotFound(new { message = "Admin profile not found" });

            return Ok(result);
        }

        /// <summary>
        /// Retrieves a paginated list of admin profiles.
        /// </summary>
        /// <param name="pageRequest">Pagination parameters.</param>
        /// <param name="request">Filter criteria for admin profiles.</param>
        /// <returns>Paginated list of admin profiles.</returns>
        [HttpGet("page")]
        public async Task<IActionResult> GetAdminProfilesPage(
            [FromQuery] PageRequest pageRequest,
            [FromQuery] GetAdminListRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _adminService.GetAdminProfilesPage(pageRequest, request);
            return Ok(result);
        }

        /// <summary>
        /// Retrieves a list of admin profiles without pagination.
        /// </summary>
        /// <param name="request">Filter criteria for admin profiles.</param>
        /// <returns>List of admin profiles.</returns>
        [HttpGet]
        public async Task<IActionResult> GetAdminProfiles([FromQuery] GetAdminListRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _adminService.GetAdminProfiles(request);
            return Ok(result);
        }
    }
}
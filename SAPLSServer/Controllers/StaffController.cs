using Microsoft.AspNetCore.Mvc;
using SAPLSServer.DTOs.Concrete.UserDto;
using SAPLSServer.DTOs.PaginationDto;
using SAPLSServer.Services.Interfaces;

namespace SAPLSServer.Controllers
{
    /// <summary>
    /// Controller for managing staff profile operations including creation, updates, and retrieval.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class StaffController : ControllerBase
    {
        private readonly IStaffService _staffService;

        public StaffController(IStaffService staffService)
        {
            _staffService = staffService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateStaff([FromBody] CreateStaffProfileRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _staffService.CreateStaff(request);
            return Ok(new { message = "Staff profile created successfully" });
        }

        [HttpPut]
        public async Task<IActionResult> UpdateStaff([FromBody] UpdateStaffProfileRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _staffService.UpdateStaff(request);
            return Ok(new { message = "Staff profile updated successfully" });
        }

        [HttpGet("details/{id}")]
        public async Task<IActionResult> GetStaffProfileDetails(string id)
        {
            if (string.IsNullOrEmpty(id))
                return BadRequest("Id is required");

            var result = await _staffService.GetStaffProfileDetails(id);
            if (result == null)
                return NotFound(new { message = "Staff profile not found" });

            return Ok(result);
        }

        [HttpGet("page")]
        public async Task<IActionResult> GetStaffProfilesPage(
            [FromQuery] PageRequest pageRequest,
            [FromQuery] GetStaffListRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _staffService.GetStaffProfilesPage(pageRequest, request);
            return Ok(result);
        }
    }
}
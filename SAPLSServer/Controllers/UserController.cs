using Microsoft.AspNetCore.Mvc;
using SAPLSServer.DTOs.Concrete.UserDto;
using SAPLSServer.Services.Interfaces;

namespace SAPLSServer.Controllers
{
    /// <summary>
    /// Controller for managing user operations including creation, updates, and profile management.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = await _userService.CreateUser(request);
            return Ok(new { message = "User created successfully", userId });
        }

        [HttpPut("password")]
        public async Task<IActionResult> UpdateUserPassword([FromBody] UpdateUserPasswordRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _userService.UpdateUserPassword(request);
            return Ok(new { message = "User password updated successfully" });
        }

        [HttpPut("profile-image")]
        public async Task<IActionResult> UpdateUserProfileImage([FromBody] UpdateUserProfileImageRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _userService.UpdateUserProfileImage(request);
            return Ok(new { message = "User profile image updated successfully" });
        }

        [HttpPut("status")]
        public async Task<IActionResult> UpdateUserStatus([FromBody] UpdateUserStatusRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _userService.UpdateUserStatus(request);
            return Ok(new { message = "User status updated successfully" });
        }
    }
}
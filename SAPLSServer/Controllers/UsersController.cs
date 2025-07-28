using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SAPLSServer.Constants;
using SAPLSServer.DTOs.Concrete;
using SAPLSServer.Services.Interfaces;

namespace SAPLSServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPut("password")]
        [Authorize]
        public async Task<IActionResult> UpdatePassword([FromBody] UpdateUserPasswordRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _userService.UpdateUserPassword(request);
            return Ok(new { message = MessageKeys.PASSWORD_UPDATED_SUCCESSFULLY });
        }

        [HttpPut("profile-image")]
        [Authorize]
        public async Task<IActionResult> UpdateProfileImage([FromBody] UpdateUserProfileImageRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _userService.UpdateUserProfileImage(request);
            return Ok(new { message = MessageKeys.PROFILE_IMAGE_UPDATED_SUCCESSFULLY });
        }

        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> UpdateUserStatus([FromBody] UpdateUserStatusRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _userService.UpdateUserStatus(request);
            return Ok(new { message = MessageKeys.USER_UPDATED_SUCCESSFULLY });
        }
    }
}
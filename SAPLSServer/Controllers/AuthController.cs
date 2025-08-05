using Microsoft.AspNetCore.Mvc;
using SAPLSServer.DTOs.Concrete.UserDto;
using SAPLSServer.Services.Interfaces;

namespace SAPLSServer.Controllers
{
    /// <summary>
    /// Controller for managing authentication operations including user login and profile authentication.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> AuthenticateUser([FromBody] AuthenticateUserRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.AuthenticateUser(request);
            if (result == null)
                return Unauthorized(new { message = "Invalid credentials" });

            return Ok(result);
        }

        [HttpPost("client/authenticate")]
        public async Task<IActionResult> AuthenticateClientProfile([FromBody] AuthenticateClientProfileRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.AuthenticateClientProfile(request);
            if (result == null)
                return Unauthorized(new { message = "Invalid client credentials" });

            return Ok(result);
        }
    }
}
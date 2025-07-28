using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SAPLSServer.Constants;
using SAPLSServer.DTOs.Concrete;
using SAPLSServer.Services.Interfaces;

namespace SAPLSServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("authenticate")]
        [AllowAnonymous]
        public async Task<IActionResult> Authenticate([FromBody] AuthenticateUserRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var response = await _authService.AuthenticateUser(request);
            if (response == null)
                return Unauthorized(new { message = MessageKeys.INVALID_CREDENTIALS_OR_USER_NOT_ACTIVE });

            return Ok(new
            {
                data = response,
                message = MessageKeys.USER_AUTHENTICATED_SUCCESSFULLY
            });
        }

        [HttpPost("authenticate-client")]
        [AllowAnonymous]
        public async Task<IActionResult> AuthenticateClient([FromBody] AuthenticateClientProfileRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var response = await _authService.AuthenticateClientProfile(request);
            if (response == null)
                return Unauthorized(new { message = MessageKeys.INVALID_CREDENTIALS_OR_CLIENT_NOT_ACTIVE });

            return Ok(new
            {
                data = response,
                message = MessageKeys.CLIENT_AUTHENTICATED_SUCCESSFULLY
            });
        }
    }
}
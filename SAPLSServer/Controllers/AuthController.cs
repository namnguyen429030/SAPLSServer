using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SAPLSServer.Constants;
using SAPLSServer.DTOs.Concrete.UserDtos;
using SAPLSServer.Exceptions;
using SAPLSServer.Services.Interfaces;
using System.Security.Authentication;
using System.Security.Claims;

namespace SAPLSServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IConfiguration _configuration;

        public AuthController(IAuthService authService, IConfiguration configuration)
        {
            _authService = authService;
            _configuration = configuration;
        }

        /// <summary>
        /// Authenticates a user with email and password
        /// </summary>
        /// <param name="request">User authentication request containing email and password</param>
        /// <returns>Authentication response with JWT token if successful</returns>
        [HttpPost("login")]
        public async Task<ActionResult<AuthenticateUserResponse>> AuthenticateUser([FromBody] AuthenticateUserRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var result = await _authService.AuthenticateUser(request);
                
                if (result == null)
                {
                    return Unauthorized(new { message = MessageKeys.INVALID_CREDENTIALS });
                }

                return Ok(result);
            }
            catch (InvalidInformationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = MessageKeys.AUTHENTICATION_FAILED });
            }
        }

        /// <summary>
        /// Authenticates a client profile with email/citizen ID and password
        /// </summary>
        /// <param name="request">Client profile authentication request</param>
        /// <returns>Authentication response with JWT token if successful</returns>
        [HttpPost("client/login")]
        public async Task<ActionResult<AuthenticateUserResponse>> AuthenticateClientProfile([FromBody] AuthenticateClientProfileRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var result = await _authService.AuthenticateClientProfile(request);
                
                if (result == null)
                {
                    return Unauthorized(new { message = MessageKeys.INVALID_CREDENTIALS });
                }

                return Ok(result);
            }
            catch (InvalidCredentialException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (InvalidInformationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = MessageKeys.AUTHENTICATION_FAILED });
            }
        }

        /// <summary>
        /// Initiates Google OAuth authentication flow - redirects user to Google sign-in
        /// </summary>
        /// <param name="returnUrl">URL to redirect to after successful authentication</param>
        /// <returns>Redirect to Google OAuth provider</returns>
        [HttpGet("google-login")]
        public IActionResult GoogleLogin(string? returnUrl = null)
        {
            var redirectUrl = Url.Action(nameof(GoogleCallback), "Auth", new { returnUrl });
            var properties = new AuthenticationProperties 
            { 
                RedirectUri = redirectUrl,
                Items = 
                {
                    { "returnUrl", returnUrl ?? "/" }
                }
            };
            
            return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        }

        /// <summary>
        /// Handles Google OAuth callback and processes authentication for existing users only
        /// </summary>
        /// <param name="returnUrl">URL to redirect to after processing</param>
        /// <returns>Redirect with authentication result</returns>
        [HttpGet("google-callback")]
        public async Task<IActionResult> GoogleCallback(string? returnUrl = null)
        {
            try
            {
                // Authenticate with Google
                var authenticateResult = await HttpContext.AuthenticateAsync(GoogleDefaults.AuthenticationScheme);
                
                if (!authenticateResult.Succeeded)
                {
                    return RedirectToFrontend($"/login?error={MessageKeys.GOOGLE_AUTH_FAILED}");
                }

                // Extract user information from Google claims
                var claims = authenticateResult.Principal?.Claims;
                var googleId = claims?.FirstOrDefault(c => c.Type == "google_id")?.Value;
                var email = claims?.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

                if (string.IsNullOrWhiteSpace(googleId) || string.IsNullOrWhiteSpace(email))
                {
                    return RedirectToFrontend($"/login?error={MessageKeys.MISSING_USER_INFO}");
                }

                // CheckIn a Google auth request with the ID token approach
                var googleAuthRequest = new GoogleAuthRequest
                {
                    AccessToken = googleId // This will be validated in AuthService
                };

                var result = await _authService.AuthenticateWithGoogle(googleAuthRequest);
                
                if (result == null)
                {
                    return RedirectToFrontend($"/login?error={MessageKeys.USER_NOT_FOUND}");
                }

                // Redirect with token for SPA
                var frontendUrl = GetFrontendUrl();
                var redirectWithToken = $"{frontendUrl}/auth/callback?token={result.AccessToken}&refreshToken={result.RefreshToken}&expiresAt={result.ExpiresAt:O}";
                
                return Redirect(redirectWithToken);
            }
            catch (Exception)
            {
                return RedirectToFrontend($"/login?error={MessageKeys.AUTHENTICATION_ERROR}");
            }
        }

        /// <summary>
        /// For mobile/desktop apps - authenticate with Google ID token for existing users only
        /// </summary>
        /// <param name="request">Google authentication request with ID token</param>
        /// <returns>Authentication response with JWT token if successful</returns>
        [HttpPost("google")]
        public async Task<ActionResult<AuthenticateUserResponse>> AuthenticateWithGoogleToken([FromBody] GoogleAuthRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (string.IsNullOrWhiteSpace(request?.AccessToken))
                {
                    return BadRequest(new { message = MessageKeys.ACCESS_TOKEN_REQUIRED });
                }

                var result = await _authService.AuthenticateWithGoogle(request);
                
                if (result == null)
                {
                    return BadRequest(new { message = MessageKeys.INVALID_GOOGLE_TOKEN_OR_USER_NOT_FOUND });
                }

                return Ok(result);
            }
            catch (InvalidInformationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = MessageKeys.AUTHENTICATION_FAILED });
            }
        }

        /// <summary>
        /// Logs out the current user by clearing authentication cookies
        /// </summary>
        /// <returns>Success response</returns>
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            try
            {
                // Sign out from all authentication schemes
                //await HttpContext.SignOutAsync(GoogleDefaults.AuthenticationScheme);
                await HttpContext.SignOutAsync();
                
                // Clear auth cookies if using them
                ClearAuthCookies();
                
                return Ok(new { message = MessageKeys.LOGGED_OUT_SUCCESSFULLY });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = MessageKeys.LOGOUT_FAILED });
            }
        }

        /// <summary>
        /// Sign out from Google OAuth specifically
        /// </summary>
        /// <returns>Success response</returns>
        [HttpPost("google-logout")]
        [Authorize]
        public async Task<IActionResult> GoogleLogout()
        {
            try
            {
                // Sign out from Google authentication
                await HttpContext.SignOutAsync(GoogleDefaults.AuthenticationScheme);
                
                // Clear auth cookies if using them
                ClearAuthCookies();
                
                return Ok(new { message = MessageKeys.LOGGED_OUT_FROM_GOOGLE_SUCCESSFULLY });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = MessageKeys.GOOGLE_LOGOUT_FAILED });
            }
        }

        /// <summary>
        /// Refreshes the access token using a valid refresh token
        /// </summary>
        /// <param name="request">Refresh token request containing the refresh token</param>
        /// <returns>New access token and refresh token if successful</returns>
        [HttpPost("refresh-token")]
        public async Task<ActionResult<AuthenticateUserResponse>> RefreshToken([FromBody] RefreshTokenRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;

                var result = await _authService.RefreshToken(currentUserId, request);
                
                if (result == null)
                {
                    return Unauthorized(new { message = MessageKeys.INVALID_REFRESH_TOKEN });
                }

                return Ok(result);
            }
            catch (InvalidInformationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = MessageKeys.TOKEN_REFRESH_FAILED });
            }
        }

        #region Private Helper Methods

        private string GetFrontendUrl()
        {
            var isDevelopment = HttpContext.RequestServices
                .GetRequiredService<IWebHostEnvironment>()
                .IsDevelopment();
                
            return isDevelopment 
                ? _configuration["Urls:Frontend:Development"] ?? "http://localhost:3000"
                : _configuration["Urls:Frontend:Production"] ?? "https://yourdomain.com";
        }

        private IActionResult RedirectToFrontend(string path)
        {
            var frontendUrl = GetFrontendUrl();
            return Redirect($"{frontendUrl}{path}");
        }

        private void ClearAuthCookies()
        {
            Response.Cookies.Delete("access_token");
            Response.Cookies.Delete("refresh_token");
        }

        #endregion
    }
}
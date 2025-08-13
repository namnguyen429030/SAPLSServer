using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SAPLSServer.Constants;
using SAPLSServer.DTOs.Concrete.UserDtos;
using SAPLSServer.Exceptions;
using SAPLSServer.Services.Interfaces;
using System.IO;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SAPLSServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PasswordController : ControllerBase
    {
        private readonly IPasswordService _passwordService;
        private readonly IOtpService _otpService;
        public PasswordController(IPasswordService passwordService, IOtpService otpService)
        {
            _passwordService = passwordService;
            _otpService = otpService;
        }
        [HttpGet("reset")]
        public async Task<IActionResult> ShowResetPage([FromQuery] string userId, [FromQuery] string otp)
        {
            if (!await _otpService.IsOtpValid(userId, otp))
            {
                var invalidOtpPage = Path.Combine(Directory.GetCurrentDirectory(), "Views", "InvalidOtp.html");
                if (!System.IO.File.Exists(invalidOtpPage))
                    return NotFound();
                var invalidOtpPageContent = await System.IO.File.ReadAllTextAsync(invalidOtpPage);
                return Content(invalidOtpPageContent, "text/html");
            }
            var resetPasswordPage = Path.Combine(Directory.GetCurrentDirectory(), "Views", "PasswordResetForm.html");
            if (!System.IO.File.Exists(resetPasswordPage))
                return NotFound();
            var resetPasswordPageContent = await System.IO.File.ReadAllTextAsync(resetPasswordPage);
            resetPasswordPageContent = resetPasswordPageContent.Replace("{userId}", userId)
                                    .Replace("{otp}", otp);
            return Content(resetPasswordPageContent, "text/html");
        }
        [HttpGet("request/reset")]
        public async Task<IActionResult> RequestResetPassword()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return Unauthorized(MessageKeys.UNAUTHORIZED_ACCESS);
            }
            try
            {
                await _passwordService.RequestResetPassword(userId);
                return Ok(MessageKeys.RESET_PASSWORD_REQUEST_SENT_SUCCESSFULLY);
            }
            catch (InvalidInformationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        [HttpPut]
        [Authorize]
        public async Task<IActionResult> UpdatePassword([FromBody] UpdateUserPasswordRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if(userId == null)
            {
                return Unauthorized(MessageKeys.UNAUTHORIZED_ACCESS);
            }
            try
            {
                await _passwordService.UpdatePassword(userId, request);
                return Ok(MessageKeys.PASSWORD_UPDATED_SUCCESSFULLY);
            }
            catch (InvalidInformationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        [HttpPost("reset")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetUserPasswordRequest request)
        {
            if (!ModelState.IsValid)
            {
                return await GetResetPasswordUnsuccessfully();
            }
            if (string.IsNullOrWhiteSpace(request.UserId) || string.IsNullOrWhiteSpace(request.Otp))
            {
                return await GetResetPasswordUnsuccessfully();
            }
            try
            {
                await _passwordService.ResetPassword(request);
                return await GetResetPasswordSuccessfully();
            }
            catch (Exception)
            {
                return await GetResetPasswordUnsuccessfully();
            }
        }

        private async Task<IActionResult> GetResetPasswordSuccessfully()
        {
            var successPage = Path.Combine(Directory.GetCurrentDirectory(), "Views", "ResetPasswordSuccessfully.html");
            if (!System.IO.File.Exists(successPage))
                return NotFound();
            var successPageContent = await System.IO.File.ReadAllTextAsync(successPage);
            return Content(successPageContent, "text/html");
        }

        private async Task<IActionResult> GetResetPasswordUnsuccessfully()
        {
            var errorPage = Path.Combine(Directory.GetCurrentDirectory(), "Views", "ResetPasswordUnsuccessfully.html");
            if (!System.IO.File.Exists(errorPage))
                return NotFound();
            var errorPageContent = await System.IO.File.ReadAllTextAsync(errorPage);
            return Content(errorPageContent, "text/html");
        }
    }
}

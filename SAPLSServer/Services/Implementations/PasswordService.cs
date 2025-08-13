using Microsoft.AspNetCore.Components.Web;
using SAPLSServer.Constants;
using SAPLSServer.DTOs.Concrete.UserDtos;
using SAPLSServer.Exceptions;
using SAPLSServer.Repositories.Interfaces;
using SAPLSServer.Services.Interfaces;

namespace SAPLSServer.Services.Implementations
{
    public class PasswordService : IPasswordService
    {
        private readonly IMailSenderService _mailSenderService;
        private readonly IUserRepository _userRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IOtpService _otpService;
        public const string PasswordResetSubject = "Reset SAPLS account's password";
        public const int RESET_PASSWORD_EXPIRATION_MINUTES = 15;

        public PasswordService(IHttpContextAccessor httpContextAccessor, IMailSenderService mailSenderService, IUserRepository userRepository, IOtpService otpService)
        {
            _mailSenderService = mailSenderService;
            _userRepository = userRepository;
            _httpContextAccessor = httpContextAccessor;
            _otpService = otpService;
        }

        public async Task ResetPassword(ResetUserPasswordRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Otp) 
                || string.IsNullOrWhiteSpace(request.NewPassword) || string.IsNullOrWhiteSpace(request.UserId))
            {
                return; //should not run here
            }

            // Use OtpService to validate the OTP
            bool isOtpValid = await _otpService.IsOtpValid(request.UserId, request.Otp);
            if (!isOtpValid)
            {
                throw new InvalidInformationException(MessageKeys.ACCESS_EXPIRED);
            }

            var user = await _userRepository.Find(request.UserId) ?? throw new InvalidInformationException(MessageKeys.USER_NOT_FOUND);
            user.Password = HashPassword(request.NewPassword);
            user.UpdatedAt = DateTime.UtcNow;
            user.OneTimePassword = null;
            _userRepository.Update(user);
            await _userRepository.SaveChangesAsync();
        }

        public async Task RequestResetPassword(string userId)
        {
            var user = await _userRepository.Find(userId) ?? throw new InvalidInformationException(MessageKeys.USER_NOT_FOUND);
            
            // Use OtpService to generate the OTP
            user.OneTimePassword = _otpService.GenerateOtp();
            user.UpdatedAt = DateTime.UtcNow;
            _userRepository.Update(user);
            await _userRepository.SaveChangesAsync();
            
            var request = _httpContextAccessor.HttpContext?.Request;
            var scheme = request?.Scheme ?? "https";
            var host = request?.Host.ToString() ?? "localhost";
            var link = $"{scheme}://{host}{UrlPaths.RESET_PASSWORD_PATH}?userId={user.Id}&otp={user.OneTimePassword}";

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Views", "ResetPasswordMailTemplate.html");
            if (!System.IO.File.Exists(filePath))
            {
                throw new FileNotFoundException();
            }
            var content = await System.IO.File.ReadAllTextAsync(filePath);
            content = content.Replace("{userFullName}", user.FullName)
                             .Replace("{resetUrl}", link)
                             .Replace("{tokenExpirationMinutes}", RESET_PASSWORD_EXPIRATION_MINUTES.ToString());
            await _mailSenderService.SendEmailAsync(user.Email, PasswordResetSubject, content);
        }

        public async Task UpdatePassword(string userId, UpdateUserPasswordRequest request)
        {
            var user = await _userRepository.Find(userId) ?? throw new InvalidInformationException(MessageKeys.USER_NOT_FOUND);
            if (!VerifyPassword(request.OldPassword, user.Password))
            {
                throw new InvalidInformationException(MessageKeys.WRONG_PASSWORD);
            }
            user.Password = HashPassword(request.NewPassword);
            _userRepository.Update(user);
            user.UpdatedAt = DateTime.UtcNow;
            await _userRepository.SaveChangesAsync();
        }

        public string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password, BCrypt.Net.BCrypt.GenerateSalt());
        }

        public bool VerifyPassword(string password, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }
    }
}

using SAPLSServer.Models;
using SAPLSServer.Repositories.Interfaces;
using SAPLSServer.Services.Interfaces;
using SAPLSServer.Helpers;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;
using SAPLSServer.Constants;
using System.Security.Authentication;
using SAPLSServer.DTOs.Concrete.UserDto;

namespace SAPLSServer.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IClientProfileRepository _clientProfileRepository;
        private readonly IConfiguration _configuration;

        public AuthService(
            IUserRepository userRepository,
            IClientProfileRepository clientProfileRepository,
            IConfiguration configuration)
        {
            _userRepository = userRepository;
            _clientProfileRepository = clientProfileRepository;
            _configuration = configuration;
        }

        public async Task<AuthenticateUserResponse?> AuthenticateUser(AuthenticateUserRequest request)
        {
            if(string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
            {
                return null;
            }
            var user = await _userRepository.Find([u => u.Email == request.Email]);
            if (user == null || user.Status != UserStatus.Active.ToString())
                return null;

            if (!PasswordHelper.VerifyPassword(request.Password, user.Password))
                return null;

            var accessToken = GenerateAccessToken(user);
            var refreshToken = GenerateRefreshToken();

            return new AuthenticateUserResponse
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                TokenType = "Bearer",
                ExpiresIn = 3600,
                ExpiresAt = DateTime.UtcNow.AddHours(1)
            };
        }

        public async Task<AuthenticateUserResponse?> AuthenticateClientProfile(AuthenticateClientProfileRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.EmailOrCitizenIdNo) || string.IsNullOrWhiteSpace(request.Password))
            {
                return null;
            }
            User? user = null;
            if (request.EmailOrCitizenIdNo.All(char.IsDigit))
            {
                var clientProfile = await _clientProfileRepository.Find([c => c.CitizenId == request.EmailOrCitizenIdNo]);
                if(clientProfile == null)
                {
                    return null;
                }
                user = await _userRepository.Find([u => u.Id == clientProfile.UserId]);
            }
            else
            {
                user = await _userRepository.Find([u => u.Email == request.EmailOrCitizenIdNo]);
            }
            if (user == null || user.Status != UserStatus.Active.ToString())
                return null;
            if (!PasswordHelper.VerifyPassword(request.Password, user.Password))
                throw new InvalidCredentialException(MessageKeys.WRONG_PASSWORD);

            var accessToken = GenerateAccessToken(user);
            var refreshToken = GenerateRefreshToken();

            return new AuthenticateUserResponse
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                TokenType = "Bearer",
                ExpiresIn = 3600,
                ExpiresAt = DateTime.UtcNow.AddHours(1)
            };
        }

        private string GenerateAccessToken(User user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:SecretKey"] ?? "DefaultSecretKey"));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.FullName),
                new Claim("role", user.Role ?? ""),
                new Claim("jti", Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["JwtSettings:Issuer"] ?? "SAPLS",
                audience: _configuration["JwtSettings:Audience"] ?? "SAPLS",
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private static string GenerateRefreshToken()
        {
            var randomBytes = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomBytes);
            return Convert.ToBase64String(randomBytes);
        }
    }
}
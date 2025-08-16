using Google.Apis.Auth;
using Microsoft.IdentityModel.Tokens;
using SAPLSServer.Constants;
using SAPLSServer.DTOs.Concrete.UserDtos;
using SAPLSServer.Exceptions;
using SAPLSServer.Services.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Authentication;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SAPLSServer.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly IUserService _userService;
        private readonly IClientService _clientService;
        private readonly IPasswordService _passwordService;
        private readonly IAdminService _adminService;
        private readonly IStaffService _staffService;

        private readonly string _issuer;
        private readonly string _audience;
        private readonly string _secretKey;
        private readonly string _clientId;
        public AuthService(
            IUserService userService,
            IClientService clientService,
            IAdminService adminService,
            IStaffService staffService,
            IAuthenticationSettings settings,
            IGoogleOAuthSettings googleOAuthSettings,
            IPasswordService passwordService)
        {
            _userService = userService;
            _adminService = adminService;
            _staffService = staffService;
            _clientService = clientService;
            _passwordService = passwordService;

            _issuer = settings.JwtIssuer;
            _audience = settings.JwtAudience;
            _secretKey = settings.JwtSecretKey;
            _clientId = googleOAuthSettings.ClientId;
        }

        public async Task<AuthenticateUserResponse?> AuthenticateUser(AuthenticateUserRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
            {
                return null;
            }
            var user = await _userService.GetByPhoneOrEmail(request.Email);
            if (user == null || user.Status != UserStatus.Active.ToString())
                return null;
            var userPassword = await _userService.GetPassword(user.Id);
            if (!_passwordService.VerifyPassword(request.Password, userPassword))
                return null;

            // Check if staff is in current shift
            if (user.Role == UserRole.Staff.ToString())
            {
                var isInCurrentShift = await _staffService.IsStaffInCurrentShift(user.Id);
                if (!isInCurrentShift)
                    return null;
            }

            var accessToken = await GenerateAccessToken(user);
            var refreshToken = GenerateRefreshToken();
            var refreshTokenExpires = DateTime.UtcNow.AddDays(7); // 7 days for refresh token

            // Store refresh token
            await _userService.StoreRefreshToken(user.Id, refreshToken, refreshTokenExpires);

            return new AuthenticateUserResponse
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                TokenType = "Bearer",
                ExpiresAt = DateTime.UtcNow.AddMinutes(5)
            };
        }

        public async Task<AuthenticateUserResponse?> AuthenticateClientProfile(AuthenticateClientProfileRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.EmailOrCitizenIdNo) || string.IsNullOrWhiteSpace(request.Password))
            {
                return null;
            }
            UserDetailsDto? user = null;
            if (request.EmailOrCitizenIdNo.All(char.IsDigit))
            {
                var clientProfile = await _clientService.GetByCitizenIdNo(request.EmailOrCitizenIdNo);
                if (clientProfile == null)
                {
                    return null;
                }
                user = await _userService.GetById(clientProfile.Id);
            }
            else
            {
                user = await _userService.GetByPhoneOrEmail(request.EmailOrCitizenIdNo);
            }
            if (user == null || user.Status != UserStatus.Active.ToString())
                return null;
            var userPassword = await _userService.GetPassword(user.Id);
            if (!_passwordService.VerifyPassword(request.Password, userPassword))
                throw new InvalidCredentialException(MessageKeys.WRONG_PASSWORD);

            var accessToken = await GenerateAccessToken(user);
            var refreshToken = GenerateRefreshToken();
            var refreshTokenExpires = DateTime.UtcNow.AddDays(7); // 7 days for refresh token

            // Store refresh token
            await _userService.StoreRefreshToken(user.Id, refreshToken, refreshTokenExpires);

            return new AuthenticateUserResponse
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                TokenType = "Bearer",
                ExpiresAt = DateTime.UtcNow.AddMinutes(5)
            };
        }

        public async Task<AuthenticateUserResponse?> AuthenticateWithGoogle(GoogleAuthRequest request)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.AccessToken))
                {
                    return null;
                }

                // Verify Google token and get user info
                var payload = await VerifyGoogleIdTokenAsync(request.AccessToken);
                if (payload == null)
                {
                    return null;
                }

                // Try to find existing user by Google ID or email
                var existingUser = await GetUserByGoogleIdOrEmail(payload.Subject, payload.Email) ?? 
                    throw new InvalidInformationException(MessageKeys.USER_NOT_FOUND);

                // Check if user is active
                if (existingUser.Status != UserStatus.Active.ToString())
                {
                    return null;
                }

                var accessToken = await GenerateAccessToken(existingUser);
                var refreshToken = GenerateRefreshToken();
                var refreshTokenExpires = DateTime.UtcNow.AddDays(7); // 7 days for refresh token

                // Store refresh token
                await _userService.StoreRefreshToken(existingUser.Id, refreshToken, refreshTokenExpires);

                return new AuthenticateUserResponse
                {
                    AccessToken = accessToken,
                    RefreshToken = refreshToken,
                    TokenType = "Bearer",
                    ExpiresAt = DateTime.UtcNow.AddMinutes(5)
                };
            }
            catch (Exception)
            {
                return null;
            }
        }
        private async Task<UserDetailsDto?> GetUserByGoogleIdOrEmail(string googleId, string email)
        {
            // First try to find by Google ID
            var userByGoogleId = await _userService.GetByGoogleId(googleId);
            if (userByGoogleId != null)
                return userByGoogleId;

            // Then try by email
            return await _userService.GetByPhoneOrEmail(email);
        }


        private async Task<GoogleJsonWebSignature.Payload?> VerifyGoogleIdTokenAsync(string idToken)
        {
            try
            {
                // Verify the ID token using Google's official library
                var settings = new GoogleJsonWebSignature.ValidationSettings()
                {
                    Audience = new List<string> { _clientId }
                };

                var payload = await GoogleJsonWebSignature.ValidateAsync(idToken, settings);

                // Check if email is verified
                return payload.EmailVerified ? payload : null;
            }
            catch
            {
                return null;
            }
        }



        private async Task<string> GenerateAccessToken(UserDetailsDto user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.MobilePhone, user.Phone ?? ""),
                new Claim(ClaimTypes.Name, user.FullName),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim("jti", Guid.NewGuid().ToString())
            };

            // Add Google ID claim if available
            if (!string.IsNullOrWhiteSpace(user.GoogleId))
            {
                claims.Add(new Claim("google_id", user.GoogleId));
            }

            if (user.Role == UserRole.Admin.ToString())
            {
                var adminRole = await _adminService.GetAdminRole(user.Id);
                claims.Add(new Claim(nameof(AdminRole), adminRole.ToString()));
            }
            else if (user.Role == UserRole.Staff.ToString())
            {
                var parkingLotId = await _staffService.GetParkingLotId(user.Id);
                claims.Add(new Claim("parking_lot_id", parkingLotId));
            }

            var token = new JwtSecurityToken(
                issuer: _issuer,
                audience: _audience,
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

        public async Task<AuthenticateUserResponse?> RefreshToken(string userId, RefreshTokenRequest request)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.RefreshToken))
                    return null;

                // Extract userId from the current (expired) access token
                // This assumes the client still sends the expired token in the Authorization header
                // or you can add userId to the RefreshTokenRequest
                                
                if (string.IsNullOrWhiteSpace(userId))
                    return null;

                // Validate refresh token for this specific user
                var isValidRefreshToken = await _userService.ValidateRefreshToken(userId, request.RefreshToken);
                if (!isValidRefreshToken)
                    return null;

                // Get user details
                var user = await _userService.GetById(userId);
                if (user == null)
                    return null;

                // Generate new tokens
                var newAccessToken = await GenerateAccessToken(user);
                var newRefreshToken = GenerateRefreshToken();
                var refreshTokenExpires = DateTime.UtcNow.AddDays(7);

                // Store new refresh token
                await _userService.StoreRefreshToken(user.Id, newRefreshToken, refreshTokenExpires);

                return new AuthenticateUserResponse
                {
                    AccessToken = newAccessToken,
                    RefreshToken = newRefreshToken,
                    TokenType = "Bearer",
                    ExpiresAt = DateTime.UtcNow.AddHours(1)
                };
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
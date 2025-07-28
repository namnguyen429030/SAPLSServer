using SAPLSServer.DTOs.Concrete;

namespace SAPLSServer.Services.Interfaces
{
    /// <summary>
    /// Provides authentication services for users and client profiles.
    /// </summary>
    public interface IAuthService
    {
        /// <summary>
        /// Authenticates a user based on the provided credentials.
        /// </summary>
        /// <param name="request">The authentication request containing user credentials.</param>
        /// <returns>An <see cref="AuthenticateUserResponse"/> if authentication is successful; otherwise, <see langword="null"/>.</returns>
        Task<AuthenticateUserResponse?> AuthenticateUser(AuthenticateUserRequest request);

        /// <summary>
        /// Authenticates a client profile using email, password, and optionally citizen ID.
        /// </summary>
        /// <param name="request">The authentication request for a client profile.</param>
        /// <returns>An <see cref="AuthenticateUserResponse"/> if authentication is successful; otherwise, <see langword="null"/>.</returns>
        Task<AuthenticateUserResponse?> AuthenticateClientProfile(AuthenticateClientProfileRequest request);
    }
}
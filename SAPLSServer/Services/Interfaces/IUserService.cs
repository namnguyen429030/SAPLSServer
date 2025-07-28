using SAPLSServer.DTOs.Concrete;

namespace SAPLSServer.Services.Interfaces
{
    /// <summary>
    /// Provides core user management operations.
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// Creates a new user based on the provided request.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<string> CreateUser(CreateUserRequest request);
        /// <summary>
        /// Updates the password for a specified user.
        /// </summary>
        /// <param name="request">The request containing the user identifier and new password details.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task UpdateUserPassword(UpdateUserPasswordRequest request);

        /// <summary>
        /// Updates the user's profile image.
        /// </summary>
        /// <param name="request">The request containing the new profile image data and user identifier.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task UpdateUserProfileImage(UpdateUserProfileImageRequest request);

        /// <summary>
        /// Deletes a user based on the specified request.
        /// </summary>
        /// <param name="request">The request containing the details of the user to be deleted.</param>
        /// <returns>A task representing the asynchronous delete operation.</returns>
        Task UpdateUserStatus(UpdateUserStatusRequest request);
    }
}

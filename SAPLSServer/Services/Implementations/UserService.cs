using SAPLSServer.Constants;
using SAPLSServer.DTOs.Concrete;
using SAPLSServer.DTOs.Concrete.UserDto;
using SAPLSServer.Exceptions;
using SAPLSServer.Helpers;
using SAPLSServer.Models;
using SAPLSServer.Repositories.Interfaces;
using SAPLSServer.Services.Interfaces;

namespace SAPLSServer.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<string> CreateUser(CreateUserRequest request)
        {
            // Check for unique Email
            bool emailExists = await _userRepository.ExistsAsync(u => u.Email == request.Email);
            bool phoneExists = await _userRepository.ExistsAsync(u => u.Phone == request.Phone);
            if (emailExists)
                throw new InvalidInformationException(MessageKeys.EMAIL_ALREADY_EXISTS);
            if (phoneExists)
                throw new InvalidInformationException(MessageKeys.PHONE_ALREADY_EXISTS);
            var user = new User
            {
                Id = Guid.NewGuid().ToString(),
                Email = request.Email,
                Password = PasswordHelper.HashPassword(request.Password),
                FullName = request.FullName,
                Phone = request.Phone,
                //ProfileImageUrl = request.ProfileImageUrl,
                Status = UserStatus.Inactive.ToString(),
                Role = UserRole.Client.ToString(),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            await _userRepository.AddAsync(user);
            return user.Id;
        }
        public async Task UpdateUserPassword(UpdateUserPasswordRequest request)
        {
            var user = await _userRepository.Find(request.Id);
            if (user == null)
                throw new InvalidInformationException(MessageKeys.USER_NOT_FOUND);

            if (!PasswordHelper.VerifyPassword(request.OldPassword, user.Password))
                throw new InvalidInformationException(MessageKeys.INVALID_OLD_PASSWORD);

            user.Password = PasswordHelper.HashPassword(request.NewPassword);
            user.UpdatedAt = DateTime.UtcNow;

            _userRepository.Update(user);
            await _userRepository.SaveChangesAsync();
        }

        public async Task UpdateUserProfileImage(UpdateUserProfileImageRequest request)
        {
            var user = await _userRepository.Find(request.Id);
            if (user == null)
                throw new InvalidInformationException(MessageKeys.USER_NOT_FOUND);

            //user.ProfileImageUrl = request.ProfileImage;
            user.UpdatedAt = DateTime.UtcNow;

            _userRepository.Update(user);
            await _userRepository.SaveChangesAsync();
        }

        public async Task UpdateUserStatus(UpdateUserStatusRequest request)
        {
            var user = await _userRepository.Find(request.Id);
            if (user == null)
                throw new InvalidInformationException(MessageKeys.USER_NOT_FOUND);
            if( request.Status != UserStatus.Active.ToString() && request.Status != UserStatus.Inactive.ToString())
                throw new InvalidInformationException(MessageKeys.INVALID_USER_STATUS);
            user.Status = request.Status;
            user.UpdatedAt = DateTime.UtcNow;

            _userRepository.Update(user);
            await _userRepository.SaveChangesAsync();
        }
    }
}
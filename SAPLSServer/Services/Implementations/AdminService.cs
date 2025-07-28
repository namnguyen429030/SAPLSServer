using SAPLSServer.Constants;
using SAPLSServer.DTOs.Base;
using SAPLSServer.DTOs.Concrete;
using SAPLSServer.Exceptions;
using SAPLSServer.Models;
using SAPLSServer.Repositories.Interfaces;
using SAPLSServer.Services.Interfaces;

namespace SAPLSServer.Services.Implementations
{
    public class AdminService : IAdminService
    {
        private readonly IAdminProfileRepository _adminProfileRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUserService _userService;

        public AdminService(IUserService userService, 
            IAdminProfileRepository adminProfileRepository, 
            IUserRepository userRepository)
        {
            _userService = userService;
            _adminProfileRepository = adminProfileRepository;
            _userRepository = userRepository;
        }

        public async Task CreateAdmin(CreateAdminProfileRequest request)
        {
            // Check for unique AdminId
            bool adminIdExists = await _adminProfileRepository.ExistsAsync(a => a.AdminId == request.AdminId);
            if (adminIdExists)
                throw new InvalidInformationException(MessageKeys.ADMIN_ID_ALREADY_EXISTS);

            var userId = await _userService.CreateUser(request);

            var adminProfile = new AdminProfile
            {
                UserId = userId,
                AdminId = request.AdminId,
                Role = AdminRole.Admin.ToString()
            };
            await _adminProfileRepository.AddAsync(adminProfile);

            await _userRepository.SaveChangesAsync();
        }

        public async Task UpdateAdmin(UpdateAdminProfileRequest request)
        {
            var adminProfile = await _adminProfileRepository.Find([ap => ap.UserId == request.Id]);
            if (adminProfile == null)
                throw new InvalidInformationException(MessageKeys.ADMIN_PROFILE_NOT_FOUND);

            adminProfile.AdminId = request.AdminId;
            adminProfile.Role = request.Role;
            _adminProfileRepository.Update(adminProfile);

            await _adminProfileRepository.SaveChangesAsync();
        }

        public async Task<AdminProfileDetailsDto?> GetAdminProfileDetails(GetDetailsRequest request)
        {
            var adminProfile = await _adminProfileRepository.Find([ap => ap.UserId == request.Id]);
            return adminProfile == null ? null : new AdminProfileDetailsDto(adminProfile);
        }

        public async Task<PageResult<AdminProfileDetailsDto>> GetAdminProfilesPage(PageRequest pageRequest, GetListRequest request)
        {
            var admins = await _adminProfileRepository.GetPageAsync(pageRequest.PageNumber, pageRequest.PageSize);
            var items = admins.Select(ap => new AdminProfileDetailsDto(ap)).ToList();

            return new PageResult<AdminProfileDetailsDto>
            {
                Items = items,
                TotalCount = items.Count,
                PageNumber = pageRequest.PageNumber,
                PageSize = pageRequest.PageSize,
            };
        }
    }
}
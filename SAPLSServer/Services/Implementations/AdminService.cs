using SAPLSServer.Constants;
using SAPLSServer.DTOs.Base;
using SAPLSServer.DTOs.Concrete.UserDto;
using SAPLSServer.DTOs.PaginationDto;
using SAPLSServer.Exceptions;
using SAPLSServer.Models;
using SAPLSServer.Repositories.Interfaces;
using SAPLSServer.Services.Interfaces;
using System.Linq.Expressions;

namespace SAPLSServer.Services.Implementations
{
    public class AdminService : IAdminService
    {
        private readonly IAdminProfileRepository _adminProfileRepository;
        private readonly IUserService _userService;

        public AdminService(IUserService userService, IAdminProfileRepository adminProfileRepository)
        {
            _userService = userService;
            _adminProfileRepository = adminProfileRepository;
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
        }

        public async Task UpdateAdmin(UpdateAdminProfileRequest request)
        {
            var adminProfile = await _adminProfileRepository.FindIncludingUser(request.Id);
            if (adminProfile == null)
                throw new InvalidInformationException(MessageKeys.ADMIN_PROFILE_NOT_FOUND);

            adminProfile.AdminId = request.AdminId;
            adminProfile.Role = request.AdminRole;
            _adminProfileRepository.Update(adminProfile);
            adminProfile.User.UpdatedAt = DateTime.UtcNow;

            await _adminProfileRepository.SaveChangesAsync();
        }

        public async Task<AdminProfileDetailsDto?> GetAdminProfileDetails(GetDetailsRequest request)
        {
            var adminProfile = await _adminProfileRepository.FindIncludingUserReadOnly(request.Id);
            if (adminProfile == null || adminProfile.User == null)
                return null;
            return new AdminProfileDetailsDto(adminProfile);
        }

        public async Task<PageResult<AdminProfileSummaryDto>> GetAdminProfilesPage(PageRequest pageRequest, GetAdminListRequest request)
        {
            var criterias = new Expression<Func<AdminProfile, bool>>[]
            {
                ap => !string.IsNullOrEmpty(request.Role) && ap.Role == request.Role,
                ap => !string.IsNullOrEmpty(request.Status) && ap.User.Status == request.Status,
                ap => !string.IsNullOrEmpty(request.SearchCriteria) && (
                        ap.AdminId.Contains(request.SearchCriteria) ||
                        ap.User.FullName.Contains(request.SearchCriteria) ||
                        ap.User.Email.Contains(request.SearchCriteria) ||
                        ap.User.Phone.Contains(request.SearchCriteria)
                    )
            };
            var totalCount = await _adminProfileRepository.CountAsync(criterias);
            var admins = await _adminProfileRepository.GetPageAsync(
                                        pageRequest.PageNumber, pageRequest.PageSize, 
                                        criterias, null, request.Order == OrderType.Asc.ToString());
            var items = new List<AdminProfileSummaryDto>();
            foreach (var admin in admins)
            {
                var adminIncludingUser = await _adminProfileRepository.FindIncludingUser(admin.UserId);
                if(adminIncludingUser == null)
                    continue; // Skip if admin profile is not found
                items.Add(new AdminProfileSummaryDto(admin));
            }
            return new PageResult<AdminProfileSummaryDto>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = pageRequest.PageNumber,
                PageSize = pageRequest.PageSize,
            };
        }

        public async Task<List<AdminProfileSummaryDto>> GetAdminProfiles(GetAdminListRequest request)
        {
            var criterias = new Expression<Func<AdminProfile, bool>>[]
            {
                ap => !string.IsNullOrEmpty(request.Role) && ap.Role == request.Role,
                ap => !string.IsNullOrEmpty(request.Status) && ap.User.Status == request.Status,
                ap => !string.IsNullOrEmpty(request.SearchCriteria) && (
                        ap.AdminId.Contains(request.SearchCriteria) ||
                        ap.User.FullName.Contains(request.SearchCriteria) ||
                        ap.User.Email.Contains(request.SearchCriteria) ||
                        ap.User.Phone.Contains(request.SearchCriteria)
                    )
            };
            var admins = await _adminProfileRepository.GetAllAsync(criterias, null, request.Order == OrderType.Asc.ToString());
            return admins
                .Select(admin => new AdminProfileSummaryDto(admin))
                .ToList();
        }
    }
}
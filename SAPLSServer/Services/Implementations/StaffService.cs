using SAPLSServer.Constants;
using SAPLSServer.DTOs.Concrete;
using SAPLSServer.DTOs.Concrete.UserDto;
using SAPLSServer.DTOs.PaginationDto;
using SAPLSServer.Exceptions;
using SAPLSServer.Models;
using SAPLSServer.Repositories.Interfaces;
using SAPLSServer.Services.Interfaces;
using System.Linq.Expressions;

namespace SAPLSServer.Services.Implementations
{
    public class StaffService : IStaffService
    {
        private readonly IStaffProfileRepository _staffProfileRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUserService _userService;

        public StaffService(IUserService userService, IStaffProfileRepository staffProfileRepository, IUserRepository userRepository)
        {
            _userService = userService;
            _staffProfileRepository = staffProfileRepository;
            _userRepository = userRepository;
        }

        public async Task CreateStaff(CreateStaffProfileRequest request)
        {
            // Check for unique StaffId
            bool staffIdExists = await _staffProfileRepository.ExistsAsync(s => s.StaffId == request.StaffId);
            if (staffIdExists)
                throw new InvalidInformationException(MessageKeys.STAFF_ID_ALREADY_EXISTS);
            var userId = await _userService.CreateUser(request);

            var staffProfile = new StaffProfile
            {
                UserId = userId,
                StaffId = request.StaffId,
                ParkingLotId = request.ParkingLotId
            };
            await _staffProfileRepository.AddAsync(staffProfile);

            await _userRepository.SaveChangesAsync();
        }

        public async Task UpdateStaff(UpdateStaffProfileRequest request)
        {
            var staffProfile = await _staffProfileRepository.Find([sp => sp.UserId == request.Id]);
            if (staffProfile == null)
                throw new InvalidInformationException(MessageKeys.STAFF_PROFILE_NOT_FOUND);

            staffProfile.StaffId = request.StaffId;
            _staffProfileRepository.Update(staffProfile);

            await _staffProfileRepository.SaveChangesAsync();
        }

        public async Task<StaffProfileDetailsDto?> GetStaffProfileDetails(string id)
        {
            var staffProfile = await _staffProfileRepository.FindIncludingUserReadOnly(id);
            if (staffProfile == null)
                return null;
            return new StaffProfileDetailsDto(staffProfile);
        }

        public async Task<PageResult<StaffProfileSummaryDto>> GetStaffProfilesPage(PageRequest pageRequest, GetStaffListRequest request)
        {
            var criterias = new Expression<Func<StaffProfile, bool>>[]
            {
                sp => !string.IsNullOrEmpty(request.Status) && sp.User.Status == request.Status,
                sp => !string.IsNullOrEmpty(request.ParkingLotId) && sp.ParkingLotId == request.ParkingLotId,
                sp => !string.IsNullOrEmpty(request.SearchCriteria) && (
                        sp.User.FullName.Contains(request.SearchCriteria) ||
                        sp.User.Email.Contains(request.SearchCriteria) ||
                        sp.User.Phone.Contains(request.SearchCriteria) ||
                        sp.StaffId.Contains(request.SearchCriteria)
                    )
            };
            var totalCount = await _staffProfileRepository.CountAsync(criterias);
            var staffs = await _staffProfileRepository.GetPageAsync(
                                        pageRequest.PageNumber, pageRequest.PageSize, criterias);
            var items = new List<StaffProfileSummaryDto>();
            foreach (var staff in staffs)
            {
                var staffIncludingUser = await _staffProfileRepository.FindIncludingUserReadOnly(staff.UserId);
                if (staffIncludingUser == null)
                {
                    continue; // Skip null staff profiles
                }
                items.Add(new StaffProfileSummaryDto(staffIncludingUser));
            }
            return new PageResult<StaffProfileSummaryDto>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = pageRequest.PageNumber,
                PageSize = pageRequest.PageSize,
            };
        }
    }
}
using SAPLSServer.Constants;
using SAPLSServer.DTOs.Concrete;
using SAPLSServer.Exceptions;
using SAPLSServer.Models;
using SAPLSServer.Repositories.Interfaces;
using SAPLSServer.Services.Interfaces;
using SAPLSServer.Helpers;

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
            var staffProfile = await _staffProfileRepository.Find([sp => sp.UserId == id]);
            return staffProfile == null ? null : new StaffProfileDetailsDto(staffProfile);
        }

        public async Task<PageResult<StaffProfileDetailsDto>> GetStaffsPageByParkingLot(PageRequest request)
        {
            var staffProfiles = await _staffProfileRepository.GetPageAsync(request.PageNumber, request.PageSize);
            var items = staffProfiles.Select(sp => new StaffProfileDetailsDto(sp)).ToList();

            return new PageResult<StaffProfileDetailsDto>
            {
                Items = items,
                TotalCount = items.Count,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
            };
        }
    }
}
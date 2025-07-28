using SAPLSServer.Constants;
using SAPLSServer.DTOs.Base;
using SAPLSServer.DTOs.Concrete;
using SAPLSServer.Exceptions;
using SAPLSServer.Models;
using SAPLSServer.Repositories.Interfaces;
using SAPLSServer.Services.Interfaces;
using SAPLSServer.Helpers;

namespace SAPLSServer.Services.Implementations
{
    public class ParkingLotOwnerService : IParkingLotOwnerService
    {
        private readonly IParkingLotOwnerProfileRepository _parkingLotOwnerProfileRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUserService _userService;

        public ParkingLotOwnerService(IUserService userService, 
            IParkingLotOwnerProfileRepository parkingLotOwnerProfileRepository, 
            IUserRepository userRepository)
        {
            _userService = userService;
            _parkingLotOwnerProfileRepository = parkingLotOwnerProfileRepository;
            _userRepository = userRepository;
        }

        public async Task CreateParkingLotOwner(CreateParkingLotOwnerProfileRequest request)
        {
            // Check for unique ParkingLotOwnerId
            bool ownerIdExists = await _parkingLotOwnerProfileRepository.ExistsAsync(o => o.ParkingLotOwnerId == request.ParkingLotOwnerId);
            if (ownerIdExists)
                throw new InvalidInformationException(MessageKeys.PARKING_LOT_OWNER_ID_ALREADY_EXISTS);

            var userId = await _userService.CreateUser(request);

            var ownerProfile = new ParkingLotOwnerProfile
            {
                UserId = userId,
                ParkingLotOwnerId = request.ParkingLotOwnerId
            };
            await _parkingLotOwnerProfileRepository.AddAsync(ownerProfile);

            await _userRepository.SaveChangesAsync();
        }

        public async Task UpdateParkingLotOwner(UpdateParkingLotOwnerProfileRequest request)
        {
            var ownerProfile = await _parkingLotOwnerProfileRepository.Find([op => op.UserId == request.Id]);
            if (ownerProfile == null)
                throw new InvalidInformationException(MessageKeys.PARKING_LOT_OWNER_PROFILE_NOT_FOUND);

            ownerProfile.ParkingLotOwnerId = request.ParkingLotOwnerId;
            _parkingLotOwnerProfileRepository.Update(ownerProfile);

            await _parkingLotOwnerProfileRepository.SaveChangesAsync();
        }

        public async Task<ParkingLotOwnerProfileDetailsDto?> GetParkingLotOwnerProfileDetails(GetDetailsRequest request)
        {
            var ownerProfile = await _parkingLotOwnerProfileRepository.Find([op => op.UserId == request.Id]);
            return ownerProfile == null ? null : new ParkingLotOwnerProfileDetailsDto(ownerProfile);
        }

        public async Task<PageResult<ParkingLotOwnerProfileDetailsDto>> GetParkingLotOwnersPage(PageRequest pageRequest, GetListRequest request)
        {
            var owners = await _parkingLotOwnerProfileRepository.GetPageAsync(pageRequest.PageNumber, pageRequest.PageSize);
            var items = owners.Select(op => new ParkingLotOwnerProfileDetailsDto(op)).ToList();

            return new PageResult<ParkingLotOwnerProfileDetailsDto>
            {
                Items = items,
                TotalCount = items.Count,
                PageNumber = pageRequest.PageNumber,
                PageSize = pageRequest.PageSize,
            };
        }
    }
}
using SAPLSServer.Constants;
using SAPLSServer.DTOs.Base;
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
    public class ParkingLotOwnerService : IParkingLotOwnerService
    {
        private readonly IParkingLotOwnerProfileRepository _parkingLotOwnerProfileRepository;
        private readonly IUserService _userService;

        public ParkingLotOwnerService(IUserService userService, 
            IParkingLotOwnerProfileRepository parkingLotOwnerProfileRepository)
        {
            _userService = userService;
            _parkingLotOwnerProfileRepository = parkingLotOwnerProfileRepository;
        }

        public async Task CreateParkingLotOwner(CreateParkingLotOwnerProfileRequest request)
        {
            // Check for unique ParkingLotId
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
        }

        public async Task UpdateParkingLotOwner(UpdateParkingLotOwnerProfileRequest request)
        {
            var ownerProfile = await _parkingLotOwnerProfileRepository.FindIncludingUser(request.Id);
            if (ownerProfile == null)
                throw new InvalidInformationException(MessageKeys.PARKING_LOT_OWNER_PROFILE_NOT_FOUND);

            ownerProfile.ParkingLotOwnerId = request.ParkingLotOwnerId;
            _parkingLotOwnerProfileRepository.Update(ownerProfile);
            ownerProfile.User.UpdatedAt = DateTime.UtcNow;
            await _parkingLotOwnerProfileRepository.SaveChangesAsync();
        }

        public async Task<ParkingLotOwnerProfileDetailsDto?> GetParkingLotOwnerProfileDetails(GetDetailsRequest request)
        {
            var ownerProfile = await _parkingLotOwnerProfileRepository.FindIncludingUserReadOnly(request.Id);
            if (ownerProfile == null)
                return null;
            return new ParkingLotOwnerProfileDetailsDto(ownerProfile);
        }

        public async Task<PageResult<ParkingLotOwnerProfileSummaryDto>> GetParkingLotOwnerProfilesPage(PageRequest pageRequest, GetParkingLotOwnerListRequest request)
        {
            var criterias = new Expression<Func<ParkingLotOwnerProfile, bool>>[]
            {
                plo => !string.IsNullOrEmpty(request.Status) && plo.User.Status == request.Status,
                plo => !string.IsNullOrEmpty(request.SearchCriteria) && (
                        plo.User.FullName.Contains(request.SearchCriteria) ||
                        plo.User.Email.Contains(request.SearchCriteria) ||
                        plo.User.Phone.Contains(request.SearchCriteria)
                    )
            };
            var totalCount = await _parkingLotOwnerProfileRepository.CountAsync(criterias);
            var owners = await _parkingLotOwnerProfileRepository.GetPageAsync(
                                        pageRequest.PageNumber, pageRequest.PageSize, criterias, 
                                        null, request.Order == OrderType.Asc.ToString());
            var items = new List<ParkingLotOwnerProfileSummaryDto>();
            foreach (var owner in owners)
            {
                var ownerIncludingUser = await _parkingLotOwnerProfileRepository.FindIncludingUserReadOnly(owner.UserId);
                if(ownerIncludingUser == null)
                {
                    continue;
                }
                items.Add(new ParkingLotOwnerProfileSummaryDto(ownerIncludingUser));
            }
            return new PageResult<ParkingLotOwnerProfileSummaryDto>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = pageRequest.PageNumber,
                PageSize = pageRequest.PageSize,
            };
        }
    }
}
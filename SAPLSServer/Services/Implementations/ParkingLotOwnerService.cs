using SAPLSServer.Constants;
using SAPLSServer.DTOs.Base;
using SAPLSServer.DTOs.Concrete;
using SAPLSServer.DTOs.Concrete.UserDtos;
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

        public async Task Create(CreateParkingLotOwnerProfileRequest request, string createPerformerId)
        {
            // Check for unique ParkingLotId
            bool ownerIdExists = await _parkingLotOwnerProfileRepository
                .ExistsAsync(o => o.ParkingLotOwnerId == request.ParkingLotOwnerId);
            if (ownerIdExists)
                throw new InvalidInformationException(MessageKeys.PARKING_LOT_OWNER_ID_ALREADY_EXISTS);

            var userId = await _userService.Create(request);

            var ownerProfile = new ParkingLotOwnerProfile
            {
                UserId = userId,
                ParkingLotOwnerId = request.ParkingLotOwnerId,
                ApiKey = request.ApiKey,
                ClientKey = request.ClientKey,
                CheckSumKey = request.CheckSumKey,
                CreatedBy = createPerformerId,
                UpdatedBy = createPerformerId,
            };
            await _parkingLotOwnerProfileRepository.AddAsync(ownerProfile);
            await _parkingLotOwnerProfileRepository.SaveChangesAsync();
        }

        public async Task Update(UpdateParkingLotOwnerProfileRequest request, string updatePerformerId)
        {
            var ownerProfile = await _parkingLotOwnerProfileRepository.FindIncludingUser(request.Id);
            if (ownerProfile == null)
                throw new InvalidInformationException(MessageKeys.PARKING_LOT_OWNER_PROFILE_NOT_FOUND);

            ownerProfile.ParkingLotOwnerId = request.ParkingLotOwnerId;
            ownerProfile.ApiKey = request.ApiKey;
            ownerProfile.ClientKey = request.ClientKey;
            ownerProfile.CheckSumKey = request.CheckSumKey;
            ownerProfile.UpdatedBy = updatePerformerId;
            ownerProfile.User.UpdatedAt = DateTime.UtcNow;
            _parkingLotOwnerProfileRepository.Update(ownerProfile);
            await _parkingLotOwnerProfileRepository.SaveChangesAsync();
        }

        public async Task<ParkingLotOwnerProfileDetailsDto?> GetParkingLotOwnerProfileDetails(
            GetDetailsRequest request)
        {
            var ownerProfile = await _parkingLotOwnerProfileRepository.FindIncludingUserReadOnly(request.Id);
            if (ownerProfile == null)
                return null;
            return new ParkingLotOwnerProfileDetailsDto(ownerProfile);
        }

        public async Task<PageResult<ParkingLotOwnerProfileSummaryDto>> GetParkingLotOwnerProfilesPage(
            PageRequest pageRequest, GetParkingLotOwnerListRequest request)
        {
            var criteriaList = new List<Expression<Func<ParkingLotOwnerProfile, bool>>>();
            
            // Ch? th�m ?i?u ki?n khi gi� tr? t?n t?i
            if (!string.IsNullOrEmpty(request.Status))
                criteriaList.Add(plo => plo.User.Status == request.Status);
                
            if (!string.IsNullOrEmpty(request.SearchCriteria))
                criteriaList.Add(plo => plo.User.FullName.Contains(request.SearchCriteria) ||
                                       plo.User.Email.Contains(request.SearchCriteria) ||
                                       plo.User.Phone.Contains(request.SearchCriteria));
            
            var totalCount = await _parkingLotOwnerProfileRepository.CountAsync(
                criteriaList.Count > 0 ? criteriaList.ToArray() : null);
                
            var owners = await _parkingLotOwnerProfileRepository.GetPageAsync(
                pageRequest.PageNumber, pageRequest.PageSize, 
                criteriaList.Count > 0 ? criteriaList.ToArray() : null, 
                null, request.Order == OrderType.Asc.ToString());
                
            var items = new List<ParkingLotOwnerProfileSummaryDto>();
            foreach (var owner in owners)
            {
                var ownerIncludingUser = await _parkingLotOwnerProfileRepository
                    .FindIncludingUserReadOnly(owner.UserId);
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

        public async Task<List<ParkingLotOwnerProfileSummaryDto>> GetAllParkingLotOwners(GetParkingLotOwnerListRequest request)
        {
            var criteriaList = new List<Expression<Func<ParkingLotOwnerProfile, bool>>>();
            
            // Ch? th�m ?i?u ki?n khi gi� tr? t?n t?i
            if (!string.IsNullOrEmpty(request.Status))
                criteriaList.Add(plo => plo.User.Status == request.Status);
                
            if (!string.IsNullOrEmpty(request.SearchCriteria))
                criteriaList.Add(plo => plo.User.FullName.Contains(request.SearchCriteria) ||
                                       plo.User.Email.Contains(request.SearchCriteria) ||
                                       plo.User.Phone.Contains(request.SearchCriteria));
            
            // L?y t?t c? owners m� kh�ng ph�n trang
            var owners = await _parkingLotOwnerProfileRepository.GetAllAsync(
                criteriaList.Count > 0 ? criteriaList.ToArray() : null, 
                null, 
                request.Order == OrderType.Asc.ToString());
                
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
            
            return items;
        }

        public Task<ParkingLotOwnerProfileDetailsDto?> GetByParkingLotOwnerId(string ownerId) {
            throw new NotImplementedException();
        }

        public Task<ParkingLotOwnerProfileDetailsDto?> GetByUserId(string userId) {
            throw new NotImplementedException();
        }

        public Task<List<ParkingLotOwnerProfileSummaryDto>> GetParkingLotOwnerProfiles(GetParkingLotOwnerListRequest request) {
            throw new NotImplementedException();
        }

        public Task<bool> IsParkingLotOwnerValid(string userId) {
            throw new NotImplementedException();
        }
    }
}
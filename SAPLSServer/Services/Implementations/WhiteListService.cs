using SAPLSServer.DTOs.Concrete;
using SAPLSServer.DTOs.PaginationDto;
using SAPLSServer.Exceptions;
using SAPLSServer.Models;
using SAPLSServer.Repositories.Interfaces;
using SAPLSServer.Services.Interfaces;
using System.Linq.Expressions;

namespace SAPLSServer.Services.Implementations
{
    public class WhiteListService : IWhitelistService
    {
        private readonly IWhiteListRepository _whiteListRepository;
        private readonly IClientProfileRepository _clientProfileRepository;
        private readonly IUserRepository _userRepository;

        public WhiteListService(IWhiteListRepository whiteListRepository, IClientProfileRepository clientProfileRepository, IUserRepository userRepository)
        {
            _whiteListRepository = whiteListRepository;
            _clientProfileRepository = clientProfileRepository;
            _userRepository = userRepository;
        }

        public async Task AddAttendantToWhitelist(AddAttendantToWhiteListRequest request)
        {
            // Check if already exists
            bool exists = await _whiteListRepository.ExistsAsync(wl =>
                wl.ParkingLotId == request.ParkingLotId && wl.ClientId == request.ClientId);
            if (exists)
                throw new InvalidInformationException("Attendant is already in the whitelist.");

            var entity = new WhiteList
            {
                ParkingLotId = request.ParkingLotId,
                ClientId = request.ClientId,
                AddedAt = DateTime.UtcNow,
                ExpireAt = request.ExpireAt
            };
            await _whiteListRepository.AddAsync(entity);
            await _whiteListRepository.SaveChangesAsync();
        }

        public async Task<WhiteListAttendantDto?> GetWhitelistAttendantDetails(GetWhiteListAttendantRequest request)
        {
            var criterias = new Expression<Func<WhiteList, bool>>[]
            {
                wl => wl.ParkingLotId == request.ParkingLotId && wl.ClientId == request.ClientId
            };
            var entity = await _whiteListRepository.Find(criterias);
            if (entity == null)
                return null;
            entity.Client = await _clientProfileRepository.Find(entity.ClientId) ?? new ClientProfile();
            if (entity.Client != null)
                entity.Client.User = await _userRepository.Find(entity.Client.UserId) ?? new User();
            return new WhiteListAttendantDto(entity);
        }

        public async Task<PageResult<WhiteListAttendantDto>> GetWhitelistAttendantsPage(PageRequest request, GetWhiteListAttendantListRequest listRequest)
        {
            var criterias = new Expression<Func<WhiteList, bool>>[]
            {
                wl => wl.ParkingLotId == listRequest.ParkingLotId,
                wl => string.IsNullOrEmpty(listRequest.SearchCriteria) ||
                      (wl.Client != null && (
                        wl.Client.User.FullName.Contains(listRequest.SearchCriteria) ||
                        wl.Client.User.Email.Contains(listRequest.SearchCriteria)
                      ))
            };
            var totalCount = await _whiteListRepository.CountAsync(criterias);
            var items = (await _whiteListRepository.GetPageAsync(
                request.PageNumber, request.PageSize, criterias))
                .ToList();
            foreach (var wl in items)
            {
                wl.Client = await _clientProfileRepository.Find(wl.ClientId) ?? new ClientProfile();
                if (wl.Client != null)
                    wl.Client.User = await _userRepository.Find(wl.Client.UserId) ?? new User();
            }
            var dtos = items.Select(wl => new WhiteListAttendantDto(wl)).ToList();

            return new PageResult<WhiteListAttendantDto>
            {
                Items = dtos,
                TotalCount = totalCount,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize
            };
        }

        public async Task RemoveAttendantFromWhitelist(RemoveAttendantFromWhiteListRequest request)
        {
            var criterias = new Expression<Func<WhiteList, bool>>[]
            {
                wl => wl.ParkingLotId == request.ParkingLotId && wl.ClientId == request.ClientId
            };
            var entity = await _whiteListRepository.Find(criterias);
            if (entity == null)
                throw new InvalidInformationException("Whitelist attendant not found.");

            _whiteListRepository.Remove(entity);
            await _whiteListRepository.SaveChangesAsync();
        }

        public async Task UpdateWhitelistAttendantExpireDate(UpdateWhiteListAttendantExpireDateRequest request)
        {
            var criterias = new Expression<Func<WhiteList, bool>>[]
            {
                wl => wl.ParkingLotId == request.ParkingLotId && wl.ClientId == request.ClientId
            };
            var entity = await _whiteListRepository.Find(criterias);
            if (entity == null)
                throw new InvalidInformationException("Whitelist attendant not found.");

            entity.ExpireAt = request.ExpiredDate;
            _whiteListRepository.Update(entity);
            await _whiteListRepository.SaveChangesAsync();
        }
    }
}

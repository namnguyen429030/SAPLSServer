using SAPLSServer.DTOs.Base;
using SAPLSServer.Services.Interfaces;
using SAPLSServer.Models;
using SAPLSServer.Exceptions;
using SAPLSServer.Repositories.Interfaces;
using SAPLSServer.Constants;
using System.Linq.Expressions;
using SAPLSServer.DTOs.PaginationDto;
using SAPLSServer.DTOs.Concrete.ParkingLotDtos;

namespace SAPLSServer.Services.Implementations
{
    public class ParkingLotService : IParkingLotService
    {
        private readonly IParkingLotRepository _parkingLotRepository;
        private readonly ISubscriptionService _subscriptionService;
        public ParkingLotService(IParkingLotRepository parkingLotRepository, 
            ISubscriptionService subscriptionService)
        {
            _parkingLotRepository = parkingLotRepository;
            _subscriptionService = subscriptionService;
        }

        public async Task CreateParkingLot(CreateParkingLotRequest request, string performerAdminId)
        {
            var entity = new ParkingLot
            {
                Id = Guid.NewGuid().ToString(),
                Name = request.Name,
                Description = request.Description,
                Address = request.Address,
                TotalParkingSlot = request.TotalParkingSlot,
                ParkingLotOwnerId = request.ParkingLotOwnerId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                SubscriptionId = request.SubscriptionId,
                ExpiredAt = DateTime.UtcNow.AddMilliseconds(await _subscriptionService
                .GetDurationOfSubscription(request.SubscriptionId)),
                CreatedBy = performerAdminId,
                UpdatedBy = performerAdminId,
            };
            await _parkingLotRepository.AddAsync(entity);
            await _parkingLotRepository.SaveChangesAsync();
        }

        public async Task UpdateParkingLotBasicInformation(UpdateParkingLotBasicInformationRequest request, 
                                                string performerId)
        {
            var entity = await _parkingLotRepository.Find(request.Id);
            if (entity == null)
                throw new InvalidInformationException(MessageKeys.PARKING_LOT_NOT_FOUND);
            if(performerId != entity.ParkingLotOwnerId)
                throw new UnauthorizedAccessException(MessageKeys.UNAUTHORIZED_ACCESS);

            entity.Name = request.Name;
            entity.Description = request.Description;
            entity.TotalParkingSlot = request.TotalParkingSlot;
            entity.Settings = request.Settings;
            entity.Status = request.Status;
            entity.UpdatedBy = performerId;
            entity.UpdatedAt = DateTime.UtcNow;
            _parkingLotRepository.Update(entity);
            await _parkingLotRepository.SaveChangesAsync();
        }

        public async Task UpdateParkingLotAddress(UpdateParkingLotAddressRequest request, 
            string performerAdminId)
        {
            var entity = await _parkingLotRepository.Find(request.Id);
            if (entity == null)
                throw new InvalidInformationException(MessageKeys.PARKING_LOT_NOT_FOUND);

            entity.Address = request.Address;
            entity.UpdatedBy = performerAdminId;
            entity.UpdatedAt = DateTime.UtcNow;
            _parkingLotRepository.Update(entity);
            await _parkingLotRepository.SaveChangesAsync();
        }

        public async Task<ParkingLotDetailsDto?> GetParkingLotDetails(string parkingLotId)
        {
            var parkingLot = await _parkingLotRepository.FindIncludingParkingLotOwnerReadOnly(parkingLotId);
            if (parkingLot == null)
                return null;
            return new ParkingLotDetailsDto(parkingLot);
        }

        public async Task<PageResult<ParkingLotSummaryDto>> GetParkingLotsPage(PageRequest pageRequest, 
            GetParkingLotListRequest request)
        {
            // Kh?i t?o danh s�ch criteria r?ng
            var criteriaList = new List<Expression<Func<ParkingLot, bool>>>();
            
            // Ch? th�m ?i?u ki?n ParkingLotOwnerId khi c� gi� tr?
            if (!string.IsNullOrEmpty(request.ParkingLotOwnerId))
            {
                criteriaList.Add(pl => pl.ParkingLotOwnerId == request.ParkingLotOwnerId);
            }
            
            // Ch? th�m ?i?u ki?n Status khi c� gi� tr?
            if (request.Status != null)
            {
                criteriaList.Add(pl => pl.Status == request.Status);
            }
            
            // Ch? th�m ?i?u ki?n t�m ki?m khi SearchCriteria c� gi� tr?
            if (!string.IsNullOrEmpty(request.SearchCriteria))
            {
                criteriaList.Add(pl => pl.Name.Contains(request.SearchCriteria) ||
                                     pl.Address.Contains(request.SearchCriteria) ||
                                     pl.Id.Contains(request.SearchCriteria));
            }
            
            // Chuy?n danh s�ch sang m?ng ?? truy?n v�o c�c ph??ng th?c repository
            var criterias = criteriaList.ToArray();
            
            // ??m t?ng s? b?n ghi ph� h?p
            var totalCount = await _parkingLotRepository.CountAsync(criterias.Length > 0 ? criterias : null);
            
            // L?y d? li?u trang hi?n t?i
            var parkingLots = await _parkingLotRepository.GetPageAsync(
                pageRequest.PageNumber,
                pageRequest.PageSize,
                criterias.Length > 0 ? criterias : null,
                null,
                request.Order == OrderType.Asc.ToString()
            );

            // Chuy?n ??i th�nh DTO v� tr? v? k?t qu? ph�n trang
            var items = parkingLots.Select(pl => new ParkingLotSummaryDto(pl)).ToList();
            return new PageResult<ParkingLotSummaryDto>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = pageRequest.PageNumber,
                PageSize = pageRequest.PageSize,
            };
        }
        public async Task<List<ParkingLotSummaryDto>> GetParkingLots(GetParkingLotListRequest request)
        {
            // Kh?i t?o danh s�ch criteria r?ng
            var criteriaList = new List<Expression<Func<ParkingLot, bool>>>();
            
            // Ch? th�m ?i?u ki?n ParkingLotOwnerId khi c� gi� tr?
            if (!string.IsNullOrEmpty(request.ParkingLotOwnerId))
            {
                criteriaList.Add(pl => pl.ParkingLotOwnerId == request.ParkingLotOwnerId);
            }
            
            // Ch? th�m ?i?u ki?n Status khi c� gi� tr?
            if (request.Status != null)
            {
                criteriaList.Add(pl => pl.Status == request.Status);
            }
            
            // Ch? th�m ?i?u ki?n t�m ki?m khi SearchCriteria c� gi� tr?
            if (!string.IsNullOrEmpty(request.SearchCriteria))
            {
                criteriaList.Add(pl => pl.Name.Contains(request.SearchCriteria) ||
                                     pl.Address.Contains(request.SearchCriteria) ||
                                     pl.Id.Contains(request.SearchCriteria));
            }
            
            // Chuy?n danh s�ch sang m?ng ?? truy?n v�o GetAllAsync
            var criterias = criteriaList.ToArray();
            
            // L?y d? li?u t? repository
            var parkingLots = await _parkingLotRepository.GetAllAsync(
                criterias.Length > 0 ? criterias : null, 
                null,
                request.Order == OrderType.Asc.ToString());

            return parkingLots.Select(pl => new ParkingLotSummaryDto(pl)).ToList();
        }

        public async Task DeleteParkingLot(DeleteRequest request)
        {
            var entity = await _parkingLotRepository.Find(request.Id);
            if (entity == null)
                throw new InvalidInformationException(MessageKeys.PARKING_LOT_NOT_FOUND);

            _parkingLotRepository.Remove(entity);
            await _parkingLotRepository.SaveChangesAsync();
        }
        public async Task<bool> IsParkingLotOwner(string parkingLotId, string userId)
        {
            var parkingLot = await _parkingLotRepository.Find(parkingLotId);
            if (parkingLot == null)
                throw new InvalidInformationException(MessageKeys.PARKING_LOT_NOT_FOUND);
            return parkingLot.ParkingLotOwnerId == userId;
        }
        public async Task<bool> IsParkingLotExpired(string parkingLotId)
        {
            var parkingLot = await _parkingLotRepository.Find(parkingLotId);
            if (parkingLot == null)
                throw new InvalidInformationException(MessageKeys.PARKING_LOT_NOT_FOUND);
            return parkingLot.ExpiredAt < DateTime.UtcNow;
        }

        public async Task UpdateParkingLotSubscription(UpdateParkingLotSubscriptionRequest request, 
                                                        string performerId)
        {
            var subscriptionDuration = await _subscriptionService.GetDurationOfSubscription(request.SubscriptionId);
            var parkingLot = await _parkingLotRepository.Find(request.Id);
            if (parkingLot == null)
                throw new InvalidInformationException(MessageKeys.PARKING_LOT_NOT_FOUND);
            if(parkingLot.ParkingLotOwnerId != performerId)
                throw new UnauthorizedAccessException(MessageKeys.UNAUTHORIZED_ACCESS);

            parkingLot.SubscriptionId = request.SubscriptionId;
            parkingLot.ExpiredAt = DateTime.UtcNow.AddMilliseconds(subscriptionDuration);
            parkingLot.UpdatedAt = DateTime.UtcNow;
            parkingLot.UpdatedBy = performerId;
            _parkingLotRepository.Update(parkingLot);
            await _parkingLotRepository.SaveChangesAsync();

        }
    }
}
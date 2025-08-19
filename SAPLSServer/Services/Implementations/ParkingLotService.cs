using SAPLSServer.DTOs.Base;
using SAPLSServer.Services.Interfaces;
using SAPLSServer.Models;
using SAPLSServer.Exceptions;
using SAPLSServer.Repositories.Interfaces;
using SAPLSServer.Constants;
using System.Linq.Expressions;
using SAPLSServer.DTOs.PaginationDto;
using SAPLSServer.DTOs.Concrete.ParkingLotDto;

namespace SAPLSServer.Services.Implementations
{
    public class ParkingLotService : IParkingLotService
    {
        private readonly IParkingLotRepository _parkingLotRepository;

        public ParkingLotService(IParkingLotRepository parkingLotRepository)
        {
            _parkingLotRepository = parkingLotRepository;
        }

        public async Task CreateParkingLot(CreateParkingLotRequest dto)
        {
            var entity = new ParkingLot
            {
                Id = Guid.NewGuid().ToString(),
                Name = dto.Name,
                Description = dto.Description,
                Address = dto.Address,
                TotalParkingSlot = dto.TotalParkingSlot,
                ParkingLotOwnerId = dto.ParkingLotOwnerId,
            };
            await _parkingLotRepository.AddAsync(entity);
            await _parkingLotRepository.SaveChangesAsync();
        }

        public async Task UpdateParkingLotBasicInformation(UpdateParkingLotBasicInformationRequest request)
        {
            var entity = await _parkingLotRepository.Find(request.Id);
            if (entity == null)
                throw new InvalidInformationException(MessageKeys.PARKING_LOT_NOT_FOUND);

            entity.Name = request.Name;
            entity.Description = request.Description;
            entity.TotalParkingSlot = request.TotalParkingSlot;
            entity.Settings = request.Settings;
            entity.Status = request.Status;
            _parkingLotRepository.Update(entity);
            await _parkingLotRepository.SaveChangesAsync();
        }

        public async Task UpdateParkingLotAddress(UpdateParkingLotAddressRequest request)
        {
            var entity = await _parkingLotRepository.Find(request.Id);
            if (entity == null)
                throw new InvalidInformationException(MessageKeys.PARKING_LOT_NOT_FOUND);

            entity.Address = request.Address;
            _parkingLotRepository.Update(entity);
            await _parkingLotRepository.SaveChangesAsync();
        }

        public async Task<ParkingLotDetailsDto?> GetParkingLotDetails(GetDetailsRequest request)
        {
            var parkingLot = await _parkingLotRepository.FindIncludingParkingLotOwnerReadOnly(request.Id);
            if (parkingLot == null)
                return null;
            return new ParkingLotDetailsDto(parkingLot);
        }

        public async Task<PageResult<ParkingLotSummaryDto>> GetParkingLotsPage(PageRequest pageRequest, GetParkingLotListRequest request)
        {
            // Kh?i t?o danh sách criteria r?ng
            var criteriaList = new List<Expression<Func<ParkingLot, bool>>>();
            
            // Ch? thêm ?i?u ki?n ParkingLotOwnerId khi có giá tr?
            if (!string.IsNullOrEmpty(request.ParkingLotOwnerId))
            {
                criteriaList.Add(pl => pl.ParkingLotOwnerId == request.ParkingLotOwnerId);
            }
            
            // Ch? thêm ?i?u ki?n Status khi có giá tr?
            if (request.Status != null)
            {
                criteriaList.Add(pl => pl.Status == request.Status);
            }
            
            // Ch? thêm ?i?u ki?n tìm ki?m khi SearchCriteria có giá tr?
            if (!string.IsNullOrEmpty(request.SearchCriteria))
            {
                criteriaList.Add(pl => pl.Name.Contains(request.SearchCriteria) ||
                                     pl.Address.Contains(request.SearchCriteria) ||
                                     pl.Id.Contains(request.SearchCriteria));
            }
            
            // Chuy?n danh sách sang m?ng ?? truy?n vào các ph??ng th?c repository
            var criterias = criteriaList.ToArray();
            
            // ??m t?ng s? b?n ghi phù h?p
            var totalCount = await _parkingLotRepository.CountAsync(criterias.Length > 0 ? criterias : null);
            
            // L?y d? li?u trang hi?n t?i
            var parkingLots = await _parkingLotRepository.GetPageAsync(
                pageRequest.PageNumber,
                pageRequest.PageSize,
                criterias.Length > 0 ? criterias : null,
                null,
                request.Order == OrderType.Asc.ToString()
            );

            // Chuy?n ??i thành DTO và tr? v? k?t qu? phân trang
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
            // Kh?i t?o danh sách criteria r?ng
            var criteriaList = new List<Expression<Func<ParkingLot, bool>>>();
            
            // Ch? thêm ?i?u ki?n ParkingLotOwnerId khi có giá tr?
            if (!string.IsNullOrEmpty(request.ParkingLotOwnerId))
            {
                criteriaList.Add(pl => pl.ParkingLotOwnerId == request.ParkingLotOwnerId);
            }
            
            // Ch? thêm ?i?u ki?n Status khi có giá tr?
            if (request.Status != null)
            {
                criteriaList.Add(pl => pl.Status == request.Status);
            }
            
            // Ch? thêm ?i?u ki?n tìm ki?m khi SearchCriteria có giá tr?
            if (!string.IsNullOrEmpty(request.SearchCriteria))
            {
                criteriaList.Add(pl => pl.Name.Contains(request.SearchCriteria) ||
                                     pl.Address.Contains(request.SearchCriteria) ||
                                     pl.Id.Contains(request.SearchCriteria));
            }
            
            // Chuy?n danh sách sang m?ng ?? truy?n vào GetAllAsync
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
    }
}
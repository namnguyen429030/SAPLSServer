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
            var criterias = new Expression<Func<ParkingLot, bool>>[] 
            {
                p1 => p1.ParkingLotOwnerId == request.ParkingLotOwnerId,
                pl => request.Status != null && pl.Status == request.Status,
                pl => !string.IsNullOrEmpty(request.SearchCriteria) && (
                        pl.Name.Contains(request.SearchCriteria) || 
                        pl.Address.Contains(request.SearchCriteria) ||
                        pl.Id.Contains(request.SearchCriteria)
                    )
            };
            var totalCount = await _parkingLotRepository.CountAsync(criterias);
            var parkingLots = await _parkingLotRepository.GetPageAsync(
                pageRequest.PageNumber,
                pageRequest.PageSize,
                criterias,
                null,
                request.Order == OrderType.Asc.ToString()
            );

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
            var criterias = new Expression<Func<ParkingLot, bool>>[]
                        {
                p1 => p1.ParkingLotOwnerId == request.ParkingLotOwnerId,
                pl => request.Status != null && pl.Status == request.Status,
                pl => !string.IsNullOrEmpty(request.SearchCriteria) && (
                        pl.Name.Contains(request.SearchCriteria) ||
                        pl.Address.Contains(request.SearchCriteria) ||
                        pl.Id.Contains(request.SearchCriteria)
                    )
                        };
            var parkingLots = await _parkingLotRepository.GetAllAsync(criterias, null,request.Order == OrderType.Asc.ToString());

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
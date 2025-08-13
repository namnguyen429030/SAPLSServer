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

        public ParkingLotService(IParkingLotRepository parkingLotRepository)
        {
            _parkingLotRepository = parkingLotRepository;
        }

        public async Task CreateParkingLot(CreateParkingLotRequest request)
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
                ApiKey = request.ApiKey,
                ClientKey = request.ClientKey,
                CheckSumKey = request.CheckSumKey,
                SubscriptionId = request.SubscriptionId,
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

        public async Task<ParkingLotDetailsDto?> GetParkingLotDetails(string parkingLotId)
        {
            var parkingLot = await _parkingLotRepository.FindIncludingParkingLotOwnerReadOnly(parkingLotId);
            if (parkingLot == null)
                return null;
            return new ParkingLotDetailsDto(parkingLot);
        }

        public async Task<PageResult<ParkingLotSummaryDto>> GetParkingLotsPage(PageRequest pageRequest, GetParkingLotListRequest request)
        {
            var criterias = new List<Expression<Func<ParkingLot, bool>>>();
            if (!string.IsNullOrWhiteSpace(request.ParkingLotOwnerId))
            {
                criterias.Add(p1 => p1.ParkingLotOwnerId == request.ParkingLotOwnerId);
            }
            criterias.Add(pl => pl.Name.Contains(request.SearchCriteria ?? string.Empty) ||
                        pl.Address.Contains(request.SearchCriteria ?? string.Empty) ||
                        pl.Id.Contains(request.SearchCriteria ?? string.Empty));
            var criteriasArray = criterias.ToArray();
            var totalCount = await _parkingLotRepository.CountAsync(criteriasArray);
            var parkingLots = await _parkingLotRepository.GetPageAsync(
                pageRequest.PageNumber,
                pageRequest.PageSize,
                criteriasArray,
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
            var criterias = new List<Expression<Func<ParkingLot, bool>>>();
            if (!string.IsNullOrWhiteSpace(request.ParkingLotOwnerId))
            {
                criterias.Add(p1 => p1.ParkingLotOwnerId == request.ParkingLotOwnerId);
            }
            criterias.Add(pl => pl.Name.Contains(request.SearchCriteria ?? string.Empty) ||
                        pl.Address.Contains(request.SearchCriteria ?? string.Empty) ||
                        pl.Id.Contains(request.SearchCriteria ?? string.Empty));
            var criteriasArray = criterias.ToArray();
            var parkingLots = await _parkingLotRepository.GetAllAsync(criteriasArray, null,request.Order == OrderType.Asc.ToString());

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
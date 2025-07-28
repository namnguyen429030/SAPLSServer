using SAPLSServer.DTOs.Base;
using SAPLSServer.DTOs.Concrete;
using SAPLSServer.Services.Interfaces;
using SAPLSServer.Models;
using SAPLSServer.Exceptions;
using SAPLSServer.Repositories.Interfaces;
using SAPLSServer.Constants;
using System.Linq.Expressions;

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
                TotalParkingSlot = dto.TotalParkingSlot
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
            var entity = await _parkingLotRepository.Find(request.Id);

            if (entity == null)
                return null;

            return new ParkingLotDetailsDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Address = entity.Address,
                Description = entity.Description,
                TotalParkingSlot = entity.TotalParkingSlot
            };
        }

        public async Task<PageResult<ParkingLotSummaryDto>> GetParkingLotsPage(PageRequest pageRequest, GetListRequest request)
        {
            Expression<Func<ParkingLot, bool>>? filter = null;
            if (!string.IsNullOrEmpty(request.SearchCriteria))
            {
                filter = x => x.Name.Contains(request.SearchCriteria) || x.Address.Contains(request.SearchCriteria);
            }

            var filters = filter != null ? new[] { filter } : null;
            var items = await _parkingLotRepository.GetPageAsync(
                pageRequest.PageNumber,
                pageRequest.PageSize,
                filters,
                x => x.Name,
                true
            );
            var totalCount = await _parkingLotRepository.CountAsync(filter);

            return new PageResult<ParkingLotSummaryDto>
            {
                Items = items.Select(x => new ParkingLotSummaryDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Address = x.Address
                }).ToList(),
                TotalCount = totalCount,
                PageNumber = pageRequest.PageNumber,
                PageSize = pageRequest.PageSize,
            };
        }

        public async Task<PageResult<ParkingLotSummaryDto>> GetParkingLotsByOwnerPage(string ownerId, PageRequest pageRequest, GetListRequest request)
        {
            Expression<Func<ParkingLot, bool>> ownerFilter = x => x.ParkingLotOwnerId == ownerId;
            Expression<Func<ParkingLot, bool>>? searchFilter = null;
            if (!string.IsNullOrEmpty(request.SearchCriteria))
            {
                searchFilter = x => x.Name.Contains(request.SearchCriteria) || x.Address.Contains(request.SearchCriteria);
            }

            var filters = searchFilter != null
                ? new[] { ownerFilter, searchFilter }
                : new[] { ownerFilter };

            var items = await _parkingLotRepository.GetPageAsync(
                pageRequest.PageNumber,
                pageRequest.PageSize,
                filters,
                x => x.Name,
                true
            );
            var totalCount = await _parkingLotRepository.CountAsync(
                x => x.ParkingLotOwnerId == ownerId &&
                     (string.IsNullOrEmpty(request.SearchCriteria) ||
                      x.Name.Contains(request.SearchCriteria) ||
                      x.Address.Contains(request.SearchCriteria))
            );

            return new PageResult<ParkingLotSummaryDto>
            {
                Items = items.Select(x => new ParkingLotSummaryDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Address = x.Address
                }).ToList(),
                TotalCount = totalCount,
                PageNumber = pageRequest.PageNumber,
                PageSize = pageRequest.PageSize,
            };
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
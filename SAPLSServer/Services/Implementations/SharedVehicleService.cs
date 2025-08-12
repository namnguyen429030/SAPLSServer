using SAPLSServer.Constants;
using SAPLSServer.DTOs.Base;
using SAPLSServer.DTOs.Concrete.SharedVehicleDto;
using SAPLSServer.DTOs.PaginationDto;
using SAPLSServer.Exceptions;
using SAPLSServer.Models;
using SAPLSServer.Repositories.Interfaces;
using SAPLSServer.Services.Interfaces;
using System.Linq.Expressions;

namespace SAPLSServer.Services.Implementations
{
    public class SharedVehicleService : ISharedVehicleService
    {
        private readonly ISharedVehicleRepository _sharedVehicleRepository;
        private readonly IVehicleRepository _vehicleRepository;
        public SharedVehicleService(ISharedVehicleRepository sharedVehicleRepository, 
            IVehicleRepository vehicleRepository)
        {
            _sharedVehicleRepository = sharedVehicleRepository;
            _vehicleRepository = vehicleRepository;
        }
        public async Task CreateSharedVehicle(CreateSharedVehicleRequest request)
        {
            var vehicle = await _vehicleRepository.Find(request.VehicleId);
            if(vehicle == null)
                throw new InvalidInformationException(MessageKeys.VEHICLE_NOT_FOUND);
            if (vehicle.SharingStatus == VehicleSharingStatus.Shared.ToString())
            {
                throw new InvalidInformationException(MessageKeys.VEHICLE_ALREADY_SHARED);
            }
            if(vehicle.OwnerId != request.OwnerId)
            {
                throw new InvalidInformationException(MessageKeys.UNAUTHORIZED_ACCESS);
            }
            var sharedVehicle = new SharedVehicle
            {
                Id = Guid.NewGuid().ToString(),
                VehicleId = request.VehicleId,
                AccessDuration = request.AccessDuration,
                InviteAt = DateTime.UtcNow,
                Note = request.Note,
                SharedPersonId = request.SharedPersonId,
            };
            await _sharedVehicleRepository.AddAsync(sharedVehicle);
            var updateRequest = new UpdateVehicleSharingStatusRequest
            {
                Id = request.VehicleId,
                SharingStatus = VehicleSharingStatus.Pending.ToString()
            };
            await UpdateVehicleSharingStatus(updateRequest);
        }

        public async Task<SharedVehicleDetailsDto?> GetSharedVehicleDetails(GetDetailsRequest request)
        {
            var sharedVehicle = await _sharedVehicleRepository.FindIncludingVehicleAndOwner(request.Id);
            if (sharedVehicle != null)
            {
                return new SharedVehicleDetailsDto(sharedVehicle);
            }
            return null;
        }
        public async Task<PageResult<SharedVehicleSummaryDto>> GetSharedVehiclesPage(PageRequest pageRequest, GetSharedVehicleList request)
        {
            var criteriaList = new List<Expression<Func<SharedVehicle, bool>>>();
            
            // Chỉ thêm điều kiện khi giá trị tồn tại
            if (!string.IsNullOrEmpty(request.SharedPersonId))
                criteriaList.Add(x => x.SharedPersonId == request.SharedPersonId);
            
            var totalCount = await _sharedVehicleRepository.CountAsync(
                criteriaList.Count > 0 ? criteriaList.ToArray() : null);
                
            var sharedVehicles = await _sharedVehicleRepository.GetPageAsync(
                pageRequest.PageNumber,
                pageRequest.PageSize,
                criteriaList.Count > 0 ? criteriaList.ToArray() : null
            );

            var items = new List<SharedVehicleSummaryDto>();
            foreach(var sharedVehicle in sharedVehicles)
            {
                var sharedVehicleWithDependencies = await _sharedVehicleRepository.FindIncludingVehicleAndOwnerReadOnly(sharedVehicle.VehicleId);
                if(sharedVehicleWithDependencies == null)
                    continue;
                items.Add(new SharedVehicleSummaryDto(sharedVehicleWithDependencies));
            }

            return new PageResult<SharedVehicleSummaryDto>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = pageRequest.PageNumber,
                PageSize = pageRequest.PageSize
            };
        }
        public async Task UpdateVehicleSharingStatus(UpdateVehicleSharingStatusRequest request)
        {
            var vehicle = await _vehicleRepository.Find(request.Id);
            if (vehicle == null)
                throw new InvalidInformationException(MessageKeys.VEHICLE_NOT_FOUND);
            if (Enum.TryParse<VehicleSharingStatus>(request.SharingStatus, out var sharingStatus))
            {
                vehicle.SharingStatus = sharingStatus.ToString();
            }
            else
            {
                throw new InvalidInformationException(MessageKeys.INVALID_VEHICLE_SHARING_STATUS);
            }
            vehicle.UpdatedAt = DateTime.UtcNow;
            _vehicleRepository.Update(vehicle);
            await _vehicleRepository.SaveChangesAsync();
        }
    }
}

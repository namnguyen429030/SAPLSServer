using SAPLSServer.Constants;
using SAPLSServer.DTOs.Base;
using SAPLSServer.DTOs.Concrete.VehicleDto;
using SAPLSServer.DTOs.PaginationDto;
using SAPLSServer.Exceptions;
using SAPLSServer.Models;
using SAPLSServer.Repositories.Interfaces;
using SAPLSServer.Services.Interfaces;
using System.Linq.Expressions;

namespace SAPLSServer.Services.Implementations
{
    public class VehicleService : IVehicleService
    {
        private readonly IVehicleRepository _vehicleRepository;
        private readonly IClientProfileRepository _clientProfileRepository;
        private readonly IUserRepository _userRepository;

        public VehicleService(
            IVehicleRepository vehicleRepository,
            IClientProfileRepository clientProfileRepository,
            IUserRepository userRepository)
        {
            _vehicleRepository = vehicleRepository;
            _clientProfileRepository = clientProfileRepository;
            _userRepository = userRepository;
        }

        public async Task CreateVehicle(CreateVehicleRequest request)
        {
            var existingVehicle = await _vehicleRepository.ExistsAsync(v => v.LicensePlate == request.LicensePlate);
            if (existingVehicle)
                throw new InvalidInformationException(MessageKeys.VEHICLE_ALREADY_EXISTS);
            var vehicle = new Vehicle
            {
                Id = Guid.NewGuid().ToString(),
                LicensePlate = request.LicensePlate,
                Brand = request.Brand,
                Model = request.Model,
                EngineNumber = request.EngineNumber,
                ChassisNumber = request.ChassisNumber,
                Color = request.Color,
                OwnerVehicleFullName = request.OwnerVehicleFullName,
                Status = VehicleStatus.Active.ToString(),
                SharingStatus = VehicleSharingStatus.Available.ToString(),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                OwnerId = request.OwnerId
            };
            await _vehicleRepository.AddAsync(vehicle);
            await _vehicleRepository.SaveChangesAsync();
        }

        public async Task UpdateVehicle(UpdateVehicleRequest request)
        {
            var vehicle = await _vehicleRepository.Find(request.Id);
            if (vehicle == null)
                throw new InvalidInformationException(MessageKeys.VEHICLE_NOT_FOUND);
            vehicle.LicensePlate = request.LicensePlate;
            vehicle.Brand = request.Brand;
            vehicle.Model = request.Model;
            vehicle.EngineNumber = request.EngineNumber;
            vehicle.ChassisNumber = request.ChassisNumber;
            vehicle.Color = request.Color;
            vehicle.OwnerVehicleFullName = request.OwnerVehicleFullName;
            vehicle.UpdatedAt = DateTime.UtcNow;
            _vehicleRepository.Update(vehicle);
            await _vehicleRepository.SaveChangesAsync();
        }

        public async Task UpdateVehicleStatus(UpdateVehicleStatusRequest request)
        {
            var vehicle = await _vehicleRepository.Find(request.Id);
            if (vehicle == null)
                throw new InvalidInformationException(MessageKeys.VEHICLE_NOT_FOUND);
            if (Enum.TryParse<VehicleStatus>(request.Status, out var status))
            {
                vehicle.Status = status.ToString();
            }
            else
            {
                throw new InvalidInformationException(MessageKeys.INVALID_VEHICLE_STATUS);
            }
            vehicle.UpdatedAt = DateTime.UtcNow;
            _vehicleRepository.Update(vehicle);
            await _vehicleRepository.SaveChangesAsync();
        }

        public async Task<VehicleDetailsDto?> GetVehicleDetails(GetDetailsRequest request)
        {
            var vehicle = await _vehicleRepository.Find(request.Id);
            if (vehicle == null)
                return null;
            vehicle.Owner = await _clientProfileRepository.Find(vehicle.OwnerId) ?? new ClientProfile();
            if (vehicle.Owner != null)
                vehicle.Owner.User = await _userRepository.Find(vehicle.Owner.UserId) ?? new User();
            return new VehicleDetailsDto(vehicle);
        }

        public async Task<PageResult<VehicleSummaryDto>> GetVehiclesPage(PageRequest pageRequest, GetVehicleListRequest request)
        {
            var criterias = new Expression<Func<Vehicle, bool>>[]
            {
                v => v.OwnerId == request.OwnerId,
                v => string.IsNullOrEmpty(request.SharingStatus) || v.SharingStatus == request.SharingStatus
            };
            var totalCount = await _vehicleRepository.CountAsync(criterias);
            var vehicles = await _vehicleRepository.GetPageAsync(
                pageRequest.PageNumber, pageRequest.PageSize, criterias);
            foreach (var v in vehicles)
            {
                v.Owner = await _clientProfileRepository.Find(v.OwnerId) ?? new ClientProfile();
                if (v.Owner != null)
                    v.Owner.User = await _userRepository.Find(v.Owner.UserId) ?? new User();
            }
            var items = vehicles.Select(v => new VehicleSummaryDto(v)).ToList();
            return new PageResult<VehicleSummaryDto>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = pageRequest.PageNumber,
                PageSize = pageRequest.PageSize
            };
        }

        public async Task DeleteVehicle(DeleteRequest request)
        {
            var vehicle = await _vehicleRepository.Find(request.Id);
            if (vehicle == null)
                throw new InvalidInformationException("Vehicle not found.");
            _vehicleRepository.Remove(vehicle);
            await _vehicleRepository.SaveChangesAsync();
        }
    }
}

using SAPLSServer.Constants;
using SAPLSServer.DTOs.Base;
using SAPLSServer.DTOs.Concrete.ParkingSessionDto;
using SAPLSServer.DTOs.PaginationDto;
using SAPLSServer.Exceptions;
using SAPLSServer.Models;
using SAPLSServer.Repositories.Interfaces;
using SAPLSServer.Services.Interfaces;
using System.Linq.Expressions;

namespace SAPLSServer.Services.Implementations
{
    public class ParkingSessionService : IParkingSessionService
    {
        private readonly IParkingSessionRepository _parkingSessionRepository;

        public ParkingSessionService(IParkingSessionRepository parkingSessionRepository)
        {
            _parkingSessionRepository = parkingSessionRepository;
        }

        public async Task CreateParkingSession(CreateParkingSessionRequest request)
        {
            if(string.IsNullOrEmpty(request.VehicleId))
                throw new InvalidInformationException(MessageKeys.VEHICLE_ID_REQUIRED);
            if(string.IsNullOrEmpty(request.ParkingLotId))
                throw new InvalidInformationException(MessageKeys.PARKING_LOT_ID_REQUIRED);
            var session = new ParkingSession
            {
                Id = Guid.NewGuid().ToString(),
                VehicleId = request.VehicleId,
                ParkingLotId = request.ParkingLotId,
                EntryDateTime = DateTime.UtcNow,
                //EntryFrontCaptureUrl = request.EntryFrontCaptureUrl,
                //EntryBackCaptureUrl = request.EntryBackCaptureUrl,
                Status = ParkingSessionStatus.Parking.ToString(),
                PaymentStatus = "Unpaid",
                PaymentMethod = "",
                Cost = 0
            };
            await _parkingSessionRepository.AddAsync(session);
            await _parkingSessionRepository.SaveChangesAsync();
        }

        public async Task UpdateParkingSessionCheckOutDateTime(UpdateParkingSessionCheckOutDateTimeRequest request)
        {
            var session = await _parkingSessionRepository.Find(request.Id);
            if (session == null)
                throw new InvalidInformationException("Parking session not found.");

            session.CheckOutDateTime = DateTime.UtcNow;
            _parkingSessionRepository.Update(session);
            await _parkingSessionRepository.SaveChangesAsync();
        }

        public async Task UpdateParkingSessionExit(UpdateParkingSessionExitRequest request)
        {
            var session = await _parkingSessionRepository.Find(request.Id);
            if (session == null)
                throw new InvalidInformationException("Parking session not found.");

            session.ExitDateTime = DateTime.UtcNow;
            session.ExitFrontCaptureUrl = request.ExitFrontCaptureUrl;
            session.ExitBackCaptureUrl = request.ExitBackCaptureUrl;
            session.Status = "Completed";
            _parkingSessionRepository.Update(session);
            await _parkingSessionRepository.SaveChangesAsync();
        }

        public async Task<ParkingSessionDetailsForClientDto?> GetParkingSessionDetailsForClient(GetDetailsRequest request)
        {
            var session = await _parkingSessionRepository.FindIncludingVehicleAndParkingLotReadOnly(request.Id);
            if (session == null)
                return null;
            return new ParkingSessionDetailsForClientDto(session);
        }

        public async Task<ParkingSessionDetailsForParkingLotDto?> GetParkingSessionDetailsForParkingLot(GetDetailsRequest request)
        {
            var session = await _parkingSessionRepository.FindIncludingVehicleReadOnly(request.Id);
            if (session == null)
                return null;
            return new ParkingSessionDetailsForParkingLotDto(session);
        }

        public async Task<PageResult<ParkingSessionSummaryForClientDto>> GetParkingSessionsForClientPage(PageRequest pageRequest, GetParkingSessionListByClientIdRequest request)
        {
            var criterias = new List<Expression<Func<ParkingSession, bool>>>
            {
                ps => !string.IsNullOrEmpty(request.ClientId) && ps.VehicleId == request.ClientId,
                ps => !string.IsNullOrEmpty(request.Status) && ps.Status == request.Status,
            };

            var totalCount = await _parkingSessionRepository.CountAsync(criterias.ToArray());
            var sessions = await _parkingSessionRepository.GetPageAsync(
                pageRequest.PageNumber, pageRequest.PageSize, criterias.ToArray());

            var items = new List<ParkingSessionSummaryForClientDto>();
            foreach(var session in sessions)
            {
                var sessionIncludingDependencies = await _parkingSessionRepository
                    .FindIncludingVehicleReadOnly(session.Id);
                if(sessionIncludingDependencies == null)
                {
                    continue; // Skip if session not found
                }
                items.Add(new ParkingSessionSummaryForClientDto(sessionIncludingDependencies));
            }
            return new PageResult<ParkingSessionSummaryForClientDto>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = pageRequest.PageNumber,
                PageSize = pageRequest.PageSize
            };
        }

        public async Task<PageResult<ParkingSessionSummaryForParkingLotDto>> GetParkingSessionsForParkingLotPage(PageRequest pageRequest, GetParkingSessionListByClientIdRequest request)
        {
            var criterias = new List<Expression<Func<ParkingSession, bool>>>
            {
                ps => !string.IsNullOrEmpty(request.Status) && ps.Status == request.Status
            };

            var totalCount = await _parkingSessionRepository.CountAsync(criterias.ToArray());
            var sessions = await _parkingSessionRepository.GetPageAsync(
                pageRequest.PageNumber, pageRequest.PageSize, criterias.ToArray());

            var items = new List<ParkingSessionSummaryForParkingLotDto>();
            foreach(var session in sessions)
            {
                var sessionIncludingDependencies = await _parkingSessionRepository
                    .FindIncludingVehicleAndParkingLot(session.VehicleId);
                if(sessionIncludingDependencies == null)
                    continue; // Skip if session not found
                items.Add(new ParkingSessionSummaryForParkingLotDto(sessionIncludingDependencies));
            }
            return new PageResult<ParkingSessionSummaryForParkingLotDto>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = pageRequest.PageNumber,
                PageSize = pageRequest.PageSize
            };
        }
    }
}
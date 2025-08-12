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
            var session = await _parkingSessionRepository.FindIncludingVehicleAndParkingLotReadOnly(request.Id);
            if (session == null)
                return null;
            return new ParkingSessionDetailsForParkingLotDto(session);
        }

        public async Task<PageResult<ParkingSessionSummaryForClientDto>> GetParkingSessionsForClientPage(PageRequest pageRequest, GetParkingSessionListByClientIdRequest request)
        {
            var criteriaList = new List<Expression<Func<ParkingSession, bool>>>();
            
            // Ch? thêm ?i?u ki?n khi giá tr? t?n t?i
            if (!string.IsNullOrEmpty(request.ClientId))
                criteriaList.Add(ps => ps.DriverId == request.ClientId);
                
            if (!string.IsNullOrEmpty(request.Status))
                criteriaList.Add(ps => ps.Status == request.Status);
            
            var totalCount = await _parkingSessionRepository.CountAsync(
                criteriaList.Count > 0 ? criteriaList.ToArray() : null);
                
            var sessions = await _parkingSessionRepository.GetPageAsync(
                pageRequest.PageNumber, pageRequest.PageSize,
                criteriaList.Count > 0 ? criteriaList.ToArray() : null);

            var items = new List<ParkingSessionSummaryForClientDto>();
            foreach(var session in sessions)
            {
                var sessionIncludingDependencies = await _parkingSessionRepository
                    .FindIncludingVehicleAndParkingLotReadOnly(session.Id);
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

        public async Task<List<ParkingSessionSummaryForClientDto>> GetParkingSessionsForClient(GetParkingSessionListByClientIdRequest request)
        {
            var criteriaList = new List<Expression<Func<ParkingSession, bool>>>();

            // Ch? thêm ?i?u ki?n khi giá tr? t?n t?i
            if (!string.IsNullOrEmpty(request.ClientId))
                criteriaList.Add(ps => ps.DriverId == request.ClientId);

            if (!string.IsNullOrEmpty(request.Status))
                criteriaList.Add(ps => ps.Status == request.Status);
                
            // Thêm các ?i?u ki?n ngày tháng n?u có
            if (request.StartEntryDate.HasValue)
                criteriaList.Add(ps => ps.EntryDateTime.Date >= request.StartEntryDate.Value.ToDateTime(TimeOnly.MinValue));
                
            if (request.EndEntryDate.HasValue)
                criteriaList.Add(ps => ps.EntryDateTime.Date <= request.EndEntryDate.Value.ToDateTime(TimeOnly.MaxValue));
                
            if (request.StartExitDate.HasValue)
                criteriaList.Add(ps => ps.ExitDateTime != null && ps.ExitDateTime.Value.Date >= request.StartExitDate.Value.ToDateTime(TimeOnly.MinValue));
                
            if (request.EndExitDate.HasValue)
                criteriaList.Add(ps => ps.ExitDateTime != null && ps.ExitDateTime.Value.Date <= request.EndExitDate.Value.ToDateTime(TimeOnly.MaxValue));

            // L?y t?t c? sessions mà không phân trang
            var sessions = await _parkingSessionRepository.GetAllAsync(
                criteriaList.Count > 0 ? criteriaList.ToArray() : null, 
                null, 
                true); // M?c ??nh s?p x?p t?ng d?n

            var items = new List<ParkingSessionSummaryForClientDto>();
            foreach (var session in sessions)
            {
                var sessionIncludingDependencies = await _parkingSessionRepository
                    .FindIncludingVehicleAndParkingLotReadOnly(session.Id);
                if (sessionIncludingDependencies == null)
                {
                    continue; // Skip if session not found
                }
                items.Add(new ParkingSessionSummaryForClientDto(sessionIncludingDependencies));
            }
            
            return items;
        }

        public async Task<PageResult<ParkingSessionSummaryForParkingLotDto>> GetParkingSessionsForParkingLotPage(PageRequest pageRequest, GetParkingSessionListByParkingLotIdRequest request)
        {
            var criteriaList = new List<Expression<Func<ParkingSession, bool>>>();
            
            // Ch? thêm ?i?u ki?n khi giá tr? t?n t?i
            if (!string.IsNullOrEmpty(request.ParkingLotId))
                criteriaList.Add(ps => ps.ParkingLotId == request.ParkingLotId);
                
            if (!string.IsNullOrEmpty(request.Status))
                criteriaList.Add(ps => ps.Status == request.Status);
            
            var totalCount = await _parkingSessionRepository.CountAsync(
                criteriaList.Count > 0 ? criteriaList.ToArray() : null);
                
            var sessions = await _parkingSessionRepository.GetPageAsync(
                pageRequest.PageNumber, pageRequest.PageSize,
                criteriaList.Count > 0 ? criteriaList.ToArray() : null);

            var items = new List<ParkingSessionSummaryForParkingLotDto>();
            foreach(var session in sessions)
            {
                var sessionIncludingDependencies = await _parkingSessionRepository
                    .FindIncludingVehicleAndParkingLotReadOnly(session.Id);
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
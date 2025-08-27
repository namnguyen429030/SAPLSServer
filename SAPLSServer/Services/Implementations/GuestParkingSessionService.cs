using SAPLSServer.Constants;
using SAPLSServer.DTOs.Concrete.GuestParkingSessionDtos;
using SAPLSServer.DTOs.PaginationDto;
using SAPLSServer.Models;
using SAPLSServer.Repositories.Interfaces;
using SAPLSServer.Services.Interfaces;
using SAPLSServer.Exceptions;
using SAPLSServer.DTOs.Concrete.FileUploadDtos;
using System.Linq.Expressions;
using SAPLSServer.DTOs.Concrete.ParkingSessionDtos;

namespace SAPLSServer.Services.Implementations
{
    public class GuestParkingSessionService : IGuestParkingSessionService
    {
        private readonly IGuestParkingSessionRepository _guestParkingSessionRepository;
        private readonly IParkingLotService _parkingLotService;
        private readonly IParkingFeeScheduleService _parkingFeeScheduleService;
        private readonly IFileService _fileService;

        public GuestParkingSessionService(
            IGuestParkingSessionRepository guestParkingSessionRepository,
            IParkingLotService parkingLotService,
            IFileService fileService,
            IParkingFeeScheduleService parkingFeeScheduleService)
        {
            _guestParkingSessionRepository = guestParkingSessionRepository;
            _parkingLotService = parkingLotService;
            _fileService = fileService;
            _parkingFeeScheduleService = parkingFeeScheduleService;
        }

        private Expression<Func<GuestParkingSession, object>> GetSortByExpression(string? sortBy)
        {
            if (string.IsNullOrWhiteSpace(sortBy))
                return s => s.EntryDateTime;

            switch (sortBy.Trim().ToLower())
            {
                case "entrydatetime":
                    return s => s.EntryDateTime;
                case "exitdatetime":
                    return s => s.ExitDateTime ?? DateTime.MinValue;
                case "status":
                    return s => s.Status;
                case "cost":
                    return s => s.Cost;
                default:
                    return s => s.EntryDateTime;
            }
        }

        private bool IsAscending(string? order)
        {
            return string.Equals(order, OrderType.Asc.ToString(), StringComparison.OrdinalIgnoreCase);
        }

        public async Task<GuestParkingSessionDetailsForParkingLotDto?> GetSessionDetailsForParkingLot(string sessionId)
        {
            var session = await _guestParkingSessionRepository.Find(sessionId);
            if (session == null)
                return null;
            return new GuestParkingSessionDetailsForParkingLotDto(session);
        }

        public async Task<List<GuestParkingSessionSummaryForParkingLotDto>> GetSessionsByParkingLot(
            GetGuestParkingSessionListByParkingLotIdRequest request)
        {
            var criterias = BuildSessionCriterias(request, s => s.ParkingLotId == request.ParkingLotId);
            var sortBy = GetSortByExpression(request.SortBy);
            var ascending = IsAscending(request.Order);

            var sessions = await _guestParkingSessionRepository.GetAllAsync(criterias.ToArray(), sortBy, ascending);
            var result = new List<GuestParkingSessionSummaryForParkingLotDto>();
            foreach (var session in sessions)
            {
                result.Add(new GuestParkingSessionSummaryForParkingLotDto(session));
            }
            return result;
        }

        public async Task<PageResult<GuestParkingSessionSummaryForParkingLotDto>> GetPageByParkingLot(
            PageRequest pageRequest, GetGuestParkingSessionListByParkingLotIdRequest listRequest)
        {
            var criterias = BuildSessionCriterias(listRequest, s => s.ParkingLotId == listRequest.ParkingLotId);
            var totalCount = await _guestParkingSessionRepository.CountAsync(criterias.ToArray());
            var sortBy = GetSortByExpression(listRequest.SortBy);
            var ascending = IsAscending(listRequest.Order);

            var page = await _guestParkingSessionRepository.GetPageAsync(
                pageRequest.PageNumber, pageRequest.PageSize, criterias.ToArray(), sortBy, ascending);

            var items = new List<GuestParkingSessionSummaryForParkingLotDto>();
            foreach (var session in page)
            {
                items.Add(new GuestParkingSessionSummaryForParkingLotDto(session));
            }
            return new PageResult<GuestParkingSessionSummaryForParkingLotDto>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = pageRequest.PageNumber,
                PageSize = pageRequest.PageSize,
            };
        }

        public async Task CheckIn(CheckInParkingSessionRequest request, string staffId)
        {
            if (!await _parkingLotService.IsParkingLotValid(request.ParkingLotId))
                throw new InvalidInformationException(MessageKeys.PARKING_LOT_NOT_FOUND);

            var frontCaptureUploadRequest = new FileUploadRequest
            {
                File = request.EntryFrontCapture!,
                Container = "parking-session-captures",
                SubFolder = $"guest-{request.VehicleLicensePlate}",
                GenerateUniqueFileName = true,
                Metadata = new Dictionary<string, string>
                {
                    { "LicensePlate", request.VehicleLicensePlate },
                    { "CaptureType", "EntryFront" }
                }
            };
            var frontCaptureResult = await _fileService.UploadFileAsync(frontCaptureUploadRequest);

            var backCaptureUploadRequest = new FileUploadRequest
            {
                File = request.EntryBackCapture!,
                Container = "parking-session-captures",
                SubFolder = $"guest-{request.VehicleLicensePlate}",
                GenerateUniqueFileName = true,
                Metadata = new Dictionary<string, string>
                {
                    { "LicensePlate", request.VehicleLicensePlate },
                    { "CaptureType", "EntryBack" }
                }
            };
            var backCaptureResult = await _fileService.UploadFileAsync(backCaptureUploadRequest);

            var guestSession = new GuestParkingSession
            {
                Id = Guid.NewGuid().ToString(),
                VehicleLicensePlate = request.VehicleLicensePlate,
                ParkingLotId = request.ParkingLotId,
                EntryDateTime = DateTime.UtcNow,
                EntryFrontCaptureUrl = frontCaptureResult.CloudUrl,
                EntryBackCaptureUrl = backCaptureResult.CloudUrl,
                Status = GuestParkingSessionStatus.Parking.ToString(),
                PaymentStatus = ParkingSessionPayStatus.NotPaid.ToString(),
                PaymentMethod = PaymentMethod.Cash.ToString(),
                VehicleType = request.VehicleType,
                Cost = 0,
                CheckInStaffId = staffId,
                ParkingFeeSchedule = await _parkingFeeScheduleService.GetParkingLotCurrentFeeSchedule(request.ParkingLotId),
            };
            await _guestParkingSessionRepository.AddAsync(guestSession);
            await _guestParkingSessionRepository.SaveChangesAsync();
        }

        public async Task Finish(FinishParkingSessionRequest request, string staffId)
        {
            var criterias = new Expression<Func<GuestParkingSession, bool>>[]
            {
                s => s.VehicleLicensePlate == request.VehicleLicensePlate &&
                     s.ParkingLotId == request.ParkingLotId &&
                     s.Status == GuestParkingSessionStatus.Parking.ToString()
            };
            var sessions = await _guestParkingSessionRepository.GetAllAsync(criterias);
            var session = sessions.OrderByDescending(s => s.EntryDateTime).FirstOrDefault();
            if (session == null)
                throw new InvalidInformationException(MessageKeys.PARKING_SESSION_NOT_FOUND);

            var frontCaptureUploadRequest = new FileUploadRequest
            {
                File = request.ExitFrontCapture!,
                Container = "parking-session-captures",
                SubFolder = $"guest-{request.VehicleLicensePlate}",
                GenerateUniqueFileName = true,
                Metadata = new Dictionary<string, string>
                {
                    { "LicensePlate", request.VehicleLicensePlate },
                    { "CaptureType", "ExitFront" }
                }
            };
            var frontCaptureResult = await _fileService.UploadFileAsync(frontCaptureUploadRequest);

            var backCaptureUploadRequest = new FileUploadRequest
            {
                File = request.ExitBackCapture!,
                Container = "parking-session-captures",
                SubFolder = $"guest-{request.VehicleLicensePlate}",
                GenerateUniqueFileName = true,
                Metadata = new Dictionary<string, string>
                {
                    { "LicensePlate", request.VehicleLicensePlate },
                    { "CaptureType", "ExitBack" }
                }
            };
            var backCaptureResult = await _fileService.UploadFileAsync(backCaptureUploadRequest);

            session.Status = GuestParkingSessionStatus.Finished.ToString();
            session.ExitFrontCaptureUrl = frontCaptureResult.CloudUrl;
            session.ExitBackCaptureUrl = backCaptureResult.CloudUrl;
            session.CheckOutStaffId = staffId;
            session.ExitDateTime = DateTime.UtcNow;
            _guestParkingSessionRepository.Update(session);
            await _guestParkingSessionRepository.SaveChangesAsync();
        }

        private List<Expression<Func<GuestParkingSession, bool>>> BuildSessionCriterias(
            GetGuestParkingSessionListByParkingLotIdRequest request,
            Expression<Func<GuestParkingSession, bool>>? extra = null)
        {
            var criterias = new List<Expression<Func<GuestParkingSession, bool>>>();
            if (extra != null)
                criterias.Add(extra);
            if (!string.IsNullOrWhiteSpace(request.Status))
                criterias.Add(s => s.Status == request.Status);
            if (request.StartEntryDate.HasValue)
                criterias.Add(s => s.EntryDateTime.Date
                >= request.StartEntryDate.Value.ToDateTime(TimeOnly.MinValue).Date);
            if (request.EndEntryDate.HasValue)
                criterias.Add(s => s.EntryDateTime.Date
                <= request.EndEntryDate.Value.ToDateTime(TimeOnly.MaxValue).Date);
            if (request.StartExitDate.HasValue)
                criterias.Add(s => s.ExitDateTime.HasValue && s.ExitDateTime.Value.Date
                >= request.StartExitDate.Value.ToDateTime(TimeOnly.MinValue).Date);
            if (request.EndExitDate.HasValue)
                criterias.Add(s => s.ExitDateTime.HasValue && s.ExitDateTime.Value.Date
                <= request.EndExitDate.Value.ToDateTime(TimeOnly.MaxValue).Date);
            if (!string.IsNullOrWhiteSpace(request.SearchCriteria))
                criterias.Add(s => s.VehicleLicensePlate.Contains(request.SearchCriteria));
            return criterias;
        }

        public async Task<ParkingSessionDetailsForParkingLotDto?> GetByLicensePlateNumber(string licennsePlateNumber, string parkingLotId)
        {
            var criterias = new Expression<Func<GuestParkingSession, bool>>[]
{
                s => s.VehicleLicensePlate == licennsePlateNumber &&
                     s.ParkingLotId == parkingLotId &&
                     s.Status == GuestParkingSessionStatus.Parking.ToString()
};
            var sessions = await _guestParkingSessionRepository.GetAllAsync(criterias);
            var session = sessions.OrderByDescending(s => s.EntryDateTime).FirstOrDefault();
            if(session == null)
            {
                return null;
            }
            var parkingSession = new ParkingSession()
            {
                Id = session.Id,
                VehicleId = string.Empty, // GuestParkingSession doesn't have VehicleId
                DriverId = string.Empty, // GuestParkingSession doesn't have DriverId
                CheckInStaffId = session.CheckInStaffId,
                CheckOutStaffId = session.CheckOutStaffId,
                ParkingLotId = session.ParkingLotId,
                EntryDateTime = session.EntryDateTime,
                ExitDateTime = session.ExitDateTime,
                CheckOutDateTime = session.CheckOutDateTime,
                EntryFrontCaptureUrl = session.EntryFrontCaptureUrl,
                EntryBackCaptureUrl = session.EntryBackCaptureUrl,
                ExitFrontCaptureUrl = session.ExitFrontCaptureUrl,
                ExitBackCaptureUrl = session.ExitBackCaptureUrl,
                TransactionId = session.TransactionId != null ? int.Parse(session.TransactionId) : null,
                TransactionCount = null, // GuestParkingSession doesn't have TransactionCount
                PaymentInformation = null, // GuestParkingSession doesn't have PaymentInformation
                PaymentMethod = session.PaymentMethod,
                Cost = session.Cost,
                Status = session.Status,
                PaymentStatus = session.PaymentStatus,
                ParkingFeeSchedule = session.ParkingFeeSchedule

            };
            return new ParkingSessionDetailsForParkingLotDto(parkingSession);
        }
    }
}
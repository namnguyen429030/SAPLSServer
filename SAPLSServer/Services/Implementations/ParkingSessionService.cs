using SAPLSServer.Constants;
using SAPLSServer.DTOs.Concrete.FileUploadDtos;
using SAPLSServer.DTOs.Concrete.GuestParkingSessionDtos;
using SAPLSServer.DTOs.Concrete.ParkingSessionDtos;
using SAPLSServer.DTOs.Concrete.PaymentDtos;
using SAPLSServer.DTOs.PaginationDto;
using SAPLSServer.Exceptions;
using SAPLSServer.Helpers;
using SAPLSServer.Models;
using SAPLSServer.Repositories.Interfaces;
using SAPLSServer.Services.Interfaces;
using System.Linq.Expressions;
using System.Text.Json;

namespace SAPLSServer.Services.Implementations
{
    public class ParkingSessionService : IParkingSessionService
    {
        private readonly IParkingSessionRepository _parkingSessionRepository;
        private readonly IGuestParkingSessionService _guestParkingSessionService;
        private readonly IParkingLotService _parkingLotService;
        private readonly IPaymentService _paymentService;
        private readonly IParkingFeeScheduleService _parkingFeeScheduleService;
        private readonly IVehicleService _vehicleService;
        private readonly IFileService _fileService;

        public ParkingSessionService(
            IParkingSessionRepository parkingSessionRepository,
            IGuestParkingSessionService guestParkingSessionService,
            IParkingLotService parkingLotService,
            IPaymentService paymentService,
            IClientService clientService,
            IVehicleService vehicleService,
            ISharedVehicleService sharedVehicleService,
            IFileService fileService,
            IParkingFeeScheduleService parkingFeeScheduleService)
        {
            _parkingSessionRepository = parkingSessionRepository;
            _guestParkingSessionService = guestParkingSessionService;
            _vehicleService = vehicleService;
            _paymentService = paymentService;
            _parkingLotService = parkingLotService;
            _fileService = fileService;
            _parkingFeeScheduleService = parkingFeeScheduleService;
        }

        public async Task<ParkingSessionDetailsForClientDto?> GetSessionDetailsForClient(string sessionId)
        {
            var session = await _parkingSessionRepository.FindIncludingVehicleAndParkingLotReadOnly(sessionId);
            if (session == null)
                return null;
            if (session.Status != ParkingSessionStatus.CheckedOut.ToString())
                session.Cost = await CalculateSessionFee(session);
            var sessionDto = new ParkingSessionDetailsForClientDto(session);
            if (!string.IsNullOrWhiteSpace(session.PaymentInformation))
            {
                sessionDto.PaymentInformation =
                    JsonSerializer.Deserialize<PaymentResponseDto>(session.PaymentInformation);
            }
            return sessionDto;
        }

        public async Task<ParkingSessionDetailsForParkingLotDto?> GetSessionDetailsForParkingLot(string sessionId)
        {
            var session = await _parkingSessionRepository.FindIncludingVehicleAndOwnerReadOnly(sessionId);
            if (session == null)
                return null;
            if (session.Status != ParkingSessionStatus.CheckedOut.ToString())
                session.Cost = await CalculateSessionFee(session);
            return new ParkingSessionDetailsForParkingLotDto(session);
        }

        public async Task<List<ParkingSessionSummaryForClientDto>> GetSessionsByClient(
            GetParkingSessionListByClientIdRequest request)
        {
            var criterias = BuildSessionCriterias(request);
            var sessions = await _parkingSessionRepository.GetAllAsync(criterias.ToArray());
            var result = new List<ParkingSessionSummaryForClientDto>();
            foreach (var session in sessions)
            {
                var sessionIncludingVehicle = await _parkingSessionRepository
                    .FindIncludingVehicleAndParkingLotReadOnly(session.Id);
                if (sessionIncludingVehicle == null)
                {
                    continue;
                }
                if (sessionIncludingVehicle.Status != ParkingSessionStatus.CheckedOut.ToString())
                    sessionIncludingVehicle.Cost = await CalculateSessionFee(sessionIncludingVehicle);
                result.Add(new ParkingSessionSummaryForClientDto(sessionIncludingVehicle));
            }
            return result;
        }

        public async Task<List<ParkingSessionSummaryForParkingLotDto>> GetSessionsByParkingLot(
            GetParkingSessionListByParkingLotIdRequest request)
        {
            var criterias = BuildSessionCriterias(request, s => s.ParkingLotId == request.ParkingLotId);
            var sessions = await _parkingSessionRepository.GetAllAsync(criterias.ToArray());
            var result = new List<ParkingSessionSummaryForParkingLotDto>();
            foreach (var session in sessions)
            {
                var sessionIncludingVehicle = await _parkingSessionRepository
                    .FindIncludingVehicleReadOnly(session.Id);
                if (sessionIncludingVehicle == null)
                {
                    continue;
                }
                if (sessionIncludingVehicle.Status != ParkingSessionStatus.CheckedOut.ToString())
                    sessionIncludingVehicle.Cost = await CalculateSessionFee(session);
                result.Add(new ParkingSessionSummaryForParkingLotDto(sessionIncludingVehicle));
            }
            return result;
        }

        public async Task CheckIn(CheckInParkingSessionRequest request, string staffId)
        {
            if (!await _parkingLotService.IsParkingLotValid(request.ParkingLotId))
                throw new InvalidInformationException(MessageKeys.PARKING_LOT_NOT_FOUND);
            var vehicle = await _vehicleService.GetByLicensePlate(request.VehicleLicensePlate);
            if (vehicle != null)
            {
                var frontCaptureUploadRequest = new FileUploadRequest
                {
                    File = request.EntryFrontCapture!,
                    Container = "parking-session-captures",
                    SubFolder = $"vehicle-{request.VehicleLicensePlate}",
                    GenerateUniqueFileName = true,
                    Metadata = new Dictionary<string, string>
                        {
                            { "LicenPlate", request.VehicleLicensePlate },
                            { "CaptureType", "EntryFront" }
                        }
                };
                var frontCaptureResult = await _fileService.UploadFileAsync(frontCaptureUploadRequest);

                var backCaptureUploadRequest = new FileUploadRequest
                {
                    File = request.EntryBackCapture!,
                    Container = "parking-session-captures",
                    SubFolder = $"vehicle-{request.VehicleLicensePlate}",
                    GenerateUniqueFileName = true,
                    Metadata = new Dictionary<string, string>
                        {
                            { "LicenPlate", request.VehicleLicensePlate },
                            { "CaptureType", "EntryBack" }
                        }
                };
                var backCaptureResult = await _fileService.UploadFileAsync(backCaptureUploadRequest);

                string driverId = vehicle.OwnerId;
                if (vehicle.SharingStatus == VehicleSharingStatus.Shared.ToString())
                {
                    driverId = await _vehicleService.GetCurrentHolderId(vehicle.Id) ?? string.Empty;
                }

                var session = new ParkingSession
                {
                    Id = Guid.NewGuid().ToString(),
                    VehicleId = vehicle.Id,
                    ParkingLotId = request.ParkingLotId,
                    EntryDateTime = DateTime.UtcNow,
                    EntryFrontCaptureUrl = frontCaptureResult.CloudUrl,
                    EntryBackCaptureUrl = backCaptureResult.CloudUrl,
                    Status = ParkingSessionStatus.Parking.ToString(),
                    PaymentStatus = ParkingSessionPayStatus.NotPaid.ToString(),
                    Cost = 0,
                    TransactionCount = 0,
                    DriverId = driverId,
                    CheckInStaffId = staffId,
                    ParkingFeeSchedule = await _parkingFeeScheduleService
                        .GetParkingLotCurrentFeeSchedule(request.ParkingLotId),
                };

                await _parkingSessionRepository.AddAsync(session);
                await _parkingSessionRepository.SaveChangesAsync();
            }
            else
            {
                // Delegate guest sessionDto check-in to the guest service
                await _guestParkingSessionService.CheckIn(request, staffId);
            }
        }
        public async Task CheckOut(CheckOutParkingSessionRequest request, string userId)
        {
            var session = await _parkingSessionRepository.FindIncludingVehicle(request.SessionId);
            if (session == null)
                throw new InvalidInformationException(MessageKeys.PARKING_SESSION_NOT_FOUND);
            if (userId != session.DriverId)
                throw new UnauthorizedAccessException(MessageKeys.UNAUTHORIZED_ACCESS);
            if (session.Status != ParkingSessionStatus.Parking.ToString())
                throw new InvalidInformationException(MessageKeys.PARKING_SESSION_ALREADY_CHECKED_OUT);

            session.Status = ParkingSessionStatus.CheckedOut.ToString();
            session.CheckOutDateTime = DateTime.UtcNow;
            session.Cost = await CalculateSessionFee(session);
            session.PaymentMethod = request.PaymentMethod;

            if (request.PaymentMethod == PaymentMethod.Bank.ToString())
            {
                int transactionId = await _parkingSessionRepository.CountTransactions() + 1;
                session.TransactionId = transactionId;
                session.TransactionCount++;
                var apiKey = await _parkingLotService.GetParkingLotApiKey(session.ParkingLotId);
                var clientKey = await _parkingLotService.GetParkingLotClientKey(session.ParkingLotId);
                var checkSumKey = await _parkingLotService.GetParkingLotCheckSumKey(session.ParkingLotId);
                string data = $"amount={(int)session.Cost}&cancelUrl={""}&description={session.Id}&orderCode={transactionId}&returnUrl={""}";
                var signature = _paymentService.GenerateSignature(data, checkSumKey);
                // Prepare payment request
                var paymentRequest = new PaymentRequestDto
                {
                    OrderCode = transactionId,
                    Amount = (int)session.Cost,
                    Description = $"SESS{transactionId}",
                    CancelUrl = "",
                    ReturnUrl = "",
                    Signature = signature,
                    BuyerName = session.Driver?.User?.FullName,
                    BuyerEmail = session.Driver?.User?.Email,
                    BuyerPhone = session.Driver?.User?.Phone,
                    BuyerAddress = string.Empty,
                    ExpiredAt = (int)DateTime.UtcNow.AddMinutes(15).ToUniversalTime().Subtract(DateTime.UnixEpoch).TotalSeconds,
                    Items = new List<DTOs.Concrete.PaymentDtos.PaymentItemDto>
                    {
                        new DTOs.Concrete.PaymentDtos.PaymentItemDto
                        {
                            Name = "Parking Fee",
                            Quantity = 1,
                            Price = (int)session.Cost
                        }
                    },
                };

                // Send payment request
                var response = await _paymentService.SendPaymentRequest(paymentRequest, clientKey, apiKey, checkSumKey);
                if (response == null)
                    throw new InvalidOperationException(MessageKeys.PAYOS_SERVICE_UNAVAILABLE);
                var responseData = JsonSerializer.Serialize(response);
                session.PaymentStatus = ParkingSessionPayStatus.Pending.ToString();
                session.PaymentInformation = responseData;

            }
            _parkingSessionRepository.Update(session);
            await _parkingSessionRepository.SaveChangesAsync();
        }

        public async Task Finish(FinishParkingSessionRequest request, string staffId)
        {
            var vehicle = await _vehicleService.GetByLicensePlate(request.VehicleLicensePlate);
            if (vehicle != null)
            {
                var session = await _parkingSessionRepository.FindLatest(request.VehicleLicensePlate, request.ParkingLotId);
                if (session == null)
                    throw new InvalidInformationException(MessageKeys.PARKING_SESSION_NOT_FOUND);
                if (session.PaymentStatus == ParkingSessionPayStatus.NotPaid.ToString())
                {
                    throw new ParkingSessionException(MessageKeys.PARKING_SESSION_NOT_PAID);
                }
                var frontCaptureUploadRequest = new FileUploadRequest
                {
                    File = request.ExitFrontCapture!,
                    Container = "parking-session-captures",
                    SubFolder = $"vehicle-{request.VehicleLicensePlate}",
                    GenerateUniqueFileName = true,
                    Metadata = new Dictionary<string, string>
                    {
                        { "VehicleId", session.VehicleId },
                        { "CaptureType", "ExitFront" }
                    }
                };
                var frontCaptureResult = await _fileService.UploadFileAsync(frontCaptureUploadRequest);

                var backCaptureUploadRequest = new FileUploadRequest
                {
                    File = request.ExitBackCapture!,
                    Container = "parking-session-captures",
                    SubFolder = $"vehicle-{request.VehicleLicensePlate}",
                    GenerateUniqueFileName = true,
                    Metadata = new Dictionary<string, string>
                    {
                        { "VehicleId", session.VehicleId },
                        { "CaptureType", "ExitBack" }
                    }
                };
                var backCaptureResult = await _fileService.UploadFileAsync(backCaptureUploadRequest);

                session.Status = ParkingSessionStatus.Finished.ToString();
                session.ExitFrontCaptureUrl = frontCaptureResult.CloudUrl;
                session.ExitBackCaptureUrl = backCaptureResult.CloudUrl;
                session.CheckOutStaffId = staffId;
                session.ExitDateTime = DateTime.UtcNow;
                _parkingSessionRepository.Update(session);
                await _parkingSessionRepository.SaveChangesAsync();
            }
            else
            {
                // Delegate guest sessionDto finish to the guest service
                await _guestParkingSessionService.Finish(request, staffId);
            }
        }

        public async Task<List<ParkingSessionSummaryForClientDto>> GetOwnedSessions(
            GetOwnedParkingSessionListRequest request, string clientId)
        {
            var criterias = BuildSessionCriterias(request, s => s.DriverId == clientId);
            var sortBy = GetSortByExpression(request.SortBy);
            var orderBy = IsAscending(request.Order);
            var sessions = await _parkingSessionRepository.GetAllAsync(criterias.ToArray(), sortBy, orderBy);
            var result = new List<ParkingSessionSummaryForClientDto>();
            foreach (var session in sessions)
            {
                var sessionIncludingVehicleAndParkingLot = await _parkingSessionRepository
                    .FindIncludingVehicleAndParkingLot(session.Id);
                if (sessionIncludingVehicleAndParkingLot == null)
                {
                    continue;
                }
                if (sessionIncludingVehicleAndParkingLot.Status != ParkingSessionStatus.CheckedOut.ToString())
                    sessionIncludingVehicleAndParkingLot.Cost = await
                        CalculateSessionFee(sessionIncludingVehicleAndParkingLot);
                result.Add(new ParkingSessionSummaryForClientDto(sessionIncludingVehicleAndParkingLot));
            }
            return result;
        }

        public async Task<PageResult<ParkingSessionSummaryForClientDto>> GetPageByClient(PageRequest pageRequest,
            GetParkingSessionListByClientIdRequest listRequest)
        {
            var criterias = BuildSessionCriterias(listRequest);
            var sortBy = GetSortByExpression(listRequest.SortBy);
            var orderBy = IsAscending(listRequest.Order);
            var totalCount = await _parkingSessionRepository.CountAsync(criterias.ToArray());
            var page = await _parkingSessionRepository.GetPageAsync(pageRequest.PageNumber, pageRequest.PageSize,
                criterias.ToArray(), null, true);
            var items = new List<ParkingSessionSummaryForClientDto>();
            foreach (var session in page)
            {
                var sessionIncludingVehicleAndParkingLot = await _parkingSessionRepository
                    .FindIncludingVehicleAndParkingLotReadOnly(session.Id);
                if (sessionIncludingVehicleAndParkingLot == null)
                {
                    continue;
                }
                if (sessionIncludingVehicleAndParkingLot.Status != ParkingSessionStatus.CheckedOut.ToString())
                    sessionIncludingVehicleAndParkingLot.Cost = await CalculateSessionFee(sessionIncludingVehicleAndParkingLot);
                items.Add(new ParkingSessionSummaryForClientDto(sessionIncludingVehicleAndParkingLot));
            }
            return new PageResult<ParkingSessionSummaryForClientDto>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = pageRequest.PageNumber,
                PageSize = pageRequest.PageSize,
            };
        }

        public async Task<PageResult<ParkingSessionSummaryForParkingLotDto>> GetPageByParkingLot(PageRequest pageRequest,
            GetParkingSessionListByParkingLotIdRequest listRequest)
        {
            var criterias = BuildSessionCriterias(listRequest, s => s.ParkingLotId == listRequest.ParkingLotId);
            var sortBy = GetSortByExpression(listRequest.SortBy);
            var orderBy = IsAscending(listRequest.Order);
            var totalCount = await _parkingSessionRepository.CountAsync(criterias.ToArray());
            var page = await _parkingSessionRepository.GetPageAsync(pageRequest.PageNumber, pageRequest.PageSize,
                criterias.ToArray(), null, true);
            var items = new List<ParkingSessionSummaryForParkingLotDto>();
            foreach (var session in page)
            {
                var sessionIncludingAll = await _parkingSessionRepository
                    .FindIncludingVehicleReadOnly(session.Id);
                if (sessionIncludingAll == null)
                {
                    continue;
                }
                if (sessionIncludingAll.Status != ParkingSessionStatus.CheckedOut.ToString())
                    sessionIncludingAll.Cost = await CalculateSessionFee(sessionIncludingAll);
                items.Add(new ParkingSessionSummaryForParkingLotDto(sessionIncludingAll));
            }
            return new PageResult<ParkingSessionSummaryForParkingLotDto>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = pageRequest.PageNumber,
                PageSize = pageRequest.PageSize,
            };
        }

        public async Task<PageResult<ParkingSessionSummaryForClientDto>> GetPageByOwnedSessions(
             PageRequest pageRequest, GetOwnedParkingSessionListRequest listRequest, string clientId)
        {
            var criterias = BuildSessionCriterias(listRequest, s => s.DriverId == clientId);
            var sortBy = GetSortByExpression(listRequest.SortBy);
            var orderBy = IsAscending(listRequest.Order);
            var totalCount = await _parkingSessionRepository.CountAsync(criterias.ToArray());
            var page = await _parkingSessionRepository.GetPageAsync(
                pageRequest.PageNumber, pageRequest.PageSize, criterias.ToArray(), null, true);
            var items = new List<ParkingSessionSummaryForClientDto>();
            foreach (var session in page)
            {

                if (session.Status != ParkingSessionStatus.CheckedOut.ToString())
                    session.Cost = await CalculateSessionFee(session);
                items.Add(new ParkingSessionSummaryForClientDto(session));
            }
            return new PageResult<ParkingSessionSummaryForClientDto>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = pageRequest.PageNumber,
                PageSize = pageRequest.PageSize,
            };
        }

        private List<Expression<Func<ParkingSession, bool>>> BuildSessionCriterias(GetOwnedParkingSessionListRequest request,
            Expression<Func<ParkingSession, bool>>? extra = null)
        {
            var criterias = new List<Expression<Func<ParkingSession, bool>>>();
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
                criterias.Add(s => s.VehicleId.Contains(request.SearchCriteria)
                || s.Vehicle.LicensePlate.Contains(request.SearchCriteria)
                || s.Driver.User.FullName.Contains(request.SearchCriteria));
            return criterias;
        }


        private Expression<Func<ParkingSession, object>> GetSortByExpression(string? sortBy)
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

        private async Task<decimal> CalculateSessionFee(ParkingSession session)
        {
            Enum.TryParse<VehicleType>(session.Vehicle.VehicleType, out var parsedVehicleType);
            return await _parkingFeeScheduleService.CalculateParkingSessionFee(
                session.ParkingFeeSchedule,
                session.EntryDateTime,
                DateTime.UtcNow,
                parsedVehicleType
            );
        }

        public async Task<int?> GetSessionTransactionId(string sessionId)
        {
            var session = await _parkingSessionRepository.Find(sessionId);
            if (session == null)
            {
                return null;
            }
            return session.TransactionId;
        }

        public async Task<PaymentResponseDto?> GetSessionPaymentInfo(string sessionId)
        {
            var session = await _parkingSessionRepository.Find(sessionId);
            if (session == null)
            {
                throw new InvalidInformationException(MessageKeys.PARKING_SESSION_NOT_FOUND);
            }
            if (string.IsNullOrWhiteSpace(session.PaymentInformation))
            {
                return null;
            }
            return JsonSerializer.Deserialize<PaymentResponseDto>(session.PaymentInformation);
        }

        public async Task ConfirmTransaction(PaymentWebHookRequest request)
        {
            var session = await _parkingSessionRepository.Find([p => p.TransactionId == request.Data.OrderCode]);
            if (session != null)
            {
                var signature = _paymentService.GenerateSignature(PayOutDataToStringConverter.ConvertToSignatureString(request.Data),
                    await _parkingLotService.GetParkingLotCheckSumKey(session.ParkingLotId));
                if (signature == request.Signature)
                {
                    session.PaymentStatus = ParkingSessionPayStatus.Paid.ToString();
                    _parkingSessionRepository.Update(session);
                    await _parkingSessionRepository.SaveChangesAsync();
                }

            }

        }

        public async Task<ParkingSessionDetailsForParkingLotDto> GetByLicensePlateNumber(string licennsePlateNumber, string parkingLotId)
        {
            var session = await _parkingSessionRepository.FindLatest(licennsePlateNumber, parkingLotId);
            if(session == null)
            {
                throw new InvalidInformationException(MessageKeys.PARKING_SESSION_NOT_FOUND);
            }
            return new ParkingSessionDetailsForParkingLotDto(session);
        }
    }
}
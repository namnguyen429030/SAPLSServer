using SAPLSServer.Constants;
using SAPLSServer.DTOs.Base;
using SAPLSServer.DTOs.Concrete.ParkingLotDtos;
using SAPLSServer.DTOs.Concrete.PaymentDtos;
using SAPLSServer.DTOs.Concrete.SubscriptionDtos;
using SAPLSServer.DTOs.PaginationDto;
using SAPLSServer.Exceptions;
using SAPLSServer.Helpers;
using SAPLSServer.Models;
using SAPLSServer.Repositories.Implementations;
using SAPLSServer.Repositories.Interfaces;
using SAPLSServer.Services.Interfaces;
using System.Linq.Expressions;
using System.Text.Json;

namespace SAPLSServer.Services.Implementations
{
    public class ParkingLotService : IParkingLotService
    {
        private readonly IParkingLotRepository _parkingLotRepository;
        private readonly ISubscriptionService _subscriptionService;
        private readonly IStaffService _staffService;
        private readonly IPaymentService _paymentService;
        private readonly IPaymentSettings _paymentSettings;
        private readonly IParkingLotOwnerService _parkingLotOwnerService;
        private readonly ILogger<ParkingLotService> _logger;
        public ParkingLotService(IParkingLotRepository parkingLotRepository,
            ISubscriptionService subscriptionService,
            IStaffService staffService,
            IPaymentService paymentService,
            IPaymentSettings paymentSettings,
            IParkingLotOwnerService parkingLotOwnerService,
            ILogger<ParkingLotService> logger)
        {
            _paymentSettings = paymentSettings;
            _parkingLotRepository = parkingLotRepository;
            _subscriptionService = subscriptionService;
            _staffService = staffService;
            _paymentService = paymentService;
            _parkingLotOwnerService = parkingLotOwnerService;
            _logger = logger;
        }

        public async Task CreateParkingLot(CreateParkingLotRequest request, string performerAdminId)
        {
            var parkignLotOwnerUid = await _parkingLotOwnerService.GetByParkingLotOwnerId(request.ParkingLotOwnerId);
            if(parkignLotOwnerUid == null)
                throw new InvalidInformationException(MessageKeys.PARKING_LOT_OWNER_NOT_FOUND);
            var entity = new ParkingLot
            {
                Id = Guid.NewGuid().ToString(),
                Name = request.Name,
                Description = request.Description,
                Address = request.Address,
                TotalParkingSlot = request.TotalParkingSlot,
                ParkingLotOwnerId = parkignLotOwnerUid.Id,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                SubscriptionId = request.SubscriptionId,
                ExpiredAt = DateTime.UtcNow.AddMilliseconds(await _subscriptionService
                .GetDurationOfSubscription(request.SubscriptionId)),
                CreatedBy = performerAdminId,
                UpdatedBy = performerAdminId,
                Settings = JsonSerializer.Serialize(new ParkingLotSettings()),
            };
            await _parkingLotRepository.AddAsync(entity);
            await _parkingLotRepository.SaveChangesAsync();
        }

        public async Task UpdateParkingLotBasicInformation(UpdateParkingLotBasicInformationRequest request,
                                                string performerId)
        {
            var entity = await _parkingLotRepository.Find(request.Id);
            if (entity == null)
                throw new InvalidInformationException(MessageKeys.PARKING_LOT_NOT_FOUND);
            if (performerId != entity.ParkingLotOwnerId)
                throw new UnauthorizedAccessException(MessageKeys.UNAUTHORIZED_ACCESS);

            entity.Name = request.Name;
            entity.Description = request.Description;
            entity.TotalParkingSlot = request.TotalParkingSlot;
            entity.Settings = request.Settings;
            entity.Status = request.Status;
            entity.Settings = request.Settings;
            entity.UpdatedAt = DateTime.UtcNow;
            _parkingLotRepository.Update(entity);
            await _parkingLotRepository.SaveChangesAsync();
        }

        public async Task UpdateParkingLotAddress(UpdateParkingLotAddressRequest request,
            string performerAdminId)
        {
            var entity = await _parkingLotRepository.Find(request.Id);
            if (entity == null)
                throw new InvalidInformationException(MessageKeys.PARKING_LOT_NOT_FOUND);

            entity.Address = request.Address;
            entity.UpdatedBy = performerAdminId;
            entity.UpdatedAt = DateTime.UtcNow;
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
        public async Task<ParkingLotDetailsForOwner?> GetParkingLotDetailsForOwner(string parkingLotId)
        {
            var parkingLot = await _parkingLotRepository.FindIncludingParkingLotOwnerReadOnly(parkingLotId);
            if (parkingLot == null)
                return null;
            return new ParkingLotDetailsForOwner(parkingLot);
        }
        public async Task<PageResult<ParkingLotSummaryDto>> GetParkingLotsPage(PageRequest pageRequest,
            GetParkingLotListRequest request)
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
            var parkingLots = await _parkingLotRepository.GetAllAsync(criteriasArray, null, request.Order == OrderType.Asc.ToString());

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
        public async Task<bool> IsParkingLotOwner(string parkingLotId, string userId)
        {
            var parkingLot = await _parkingLotRepository.Find(parkingLotId);
            if (parkingLot == null)
                throw new InvalidInformationException(MessageKeys.PARKING_LOT_NOT_FOUND);
            return parkingLot.ParkingLotOwnerId == userId;
        }
        public async Task<bool> IsParkingLotExpired(string parkingLotId)
        {
            var parkingLot = await _parkingLotRepository.Find(parkingLotId);
            if (parkingLot == null)
                throw new InvalidInformationException(MessageKeys.PARKING_LOT_NOT_FOUND);
            return parkingLot.ExpiredAt < DateTime.UtcNow;
        }

        public async Task<int> UpdateParkingLotSubscription(UpdateParkingLotSubscriptionRequest request,
                                                        string performerId)
        {
            var subscriptionDuration = await _subscriptionService.GetDurationOfSubscription(request.SubscriptionId);
            var cost = await _subscriptionService.GetCostOfSubscription(request.SubscriptionId);
            var parkingLot = await _parkingLotRepository.Find(request.Id);
            if (parkingLot == null)
                throw new InvalidInformationException(MessageKeys.PARKING_LOT_NOT_FOUND);
            if (parkingLot.ParkingLotOwnerId != performerId)
                throw new UnauthorizedAccessException(MessageKeys.UNAUTHORIZED_ACCESS);

            int transactionId = new Random().Next(0, int.MaxValue);
            parkingLot.SubscriptionTransactionId = transactionId;
            var apiKey =  _paymentSettings.PaymentGatewayApiKey;
            var clientKey = _paymentSettings.PaymentGatewayClientKey;
            var checkSumKey = _paymentSettings.PaymentGatewayCheckSumKey;
            _logger.LogInformation($"ApiKey: {apiKey}, ClientKey: {clientKey}, CheckSumKey: {checkSumKey}");

            string data = $"amount={(int)cost}&cancelUrl={""}&description=SUB{transactionId}" +
                $"&orderCode={transactionId}&returnUrl={""}";
            var signature = _paymentService.GenerateSignature(data, checkSumKey);
            // Prepare payment request
            var paymentRequest = new PaymentRequestDto
            {
                OrderCode = transactionId,
                Amount = (int)cost,
                Description = $"SUB{transactionId}",
                CancelUrl = "",
                ReturnUrl = "",
                Signature = signature,
                BuyerName = "",
                BuyerEmail = "",
                BuyerPhone = "",
                BuyerAddress = string.Empty,
                ExpiredAt = (int)DateTime.UtcNow.AddMinutes(15).ToUniversalTime().Subtract(DateTime.UnixEpoch).TotalSeconds,
                Items = new List<DTOs.Concrete.PaymentDtos.PaymentItemDto>
                    {
                        new DTOs.Concrete.PaymentDtos.PaymentItemDto
                        {
                            Name = "Parking lot Subscription",
                            Quantity = 1,
                            Price = (int)cost
                        }
                    },
            };
            var paymentResponse = await _paymentService.SendPaymentRequest(paymentRequest, clientKey, apiKey, checkSumKey);
            parkingLot.TempSubscriptionId = request.SubscriptionId;
            parkingLot.SubscriptionTransactionInformation = JsonSerializer.Serialize(paymentResponse);
            //parkingLot.ExpiredAt = DateTime.UtcNow.AddMilliseconds(subscriptionDuration);
            //parkingLot.UpdatedAt = DateTime.UtcNow;
            //parkingLot.UpdatedBy = performerId;
            _parkingLotRepository.Update(parkingLot);
            await _parkingLotRepository.SaveChangesAsync();
            return paymentRequest.OrderCode;
        }

        public Task<bool> IsParkingLotValid(string parkingLotId)
        {
            return _parkingLotRepository.ExistsAsync(pl => pl.Id == parkingLotId);
        }

        public async Task<bool> IsParkingLotStaff(string parkingLotId, string userId)
        {
            return await _staffService.GetParkingLotId(userId) == parkingLotId;
        }

        public async Task<string> GetParkingLotApiKey(string parkingLotId)
        {
            var parkingLot = await _parkingLotRepository.FindIncludingParkingLotOwnerReadOnly(parkingLotId);
            if (parkingLot == null)
                throw new InvalidInformationException(MessageKeys.PARKING_LOT_NOT_FOUND);
            if (parkingLot.ParkingLotOwner == null || string.IsNullOrEmpty(parkingLot.ParkingLotOwner.ApiKey))
                throw new InvalidInformationException(MessageKeys.PARKING_LOT_API_KEY_NOT_FOUND);
            return parkingLot.ParkingLotOwner.ApiKey;
        }

        public async Task<string> GetParkingLotClientKey(string parkingLotId)
        {
            var parkingLot = await _parkingLotRepository.FindIncludingParkingLotOwnerReadOnly(parkingLotId);
            if (parkingLot == null)
                throw new InvalidInformationException(MessageKeys.PARKING_LOT_NOT_FOUND);
            if (parkingLot.ParkingLotOwner == null || string.IsNullOrEmpty(parkingLot.ParkingLotOwner.ClientKey))
                throw new InvalidInformationException(MessageKeys.PARKING_LOT_CLIENT_KEY_NOT_FOUND);
            return parkingLot.ParkingLotOwner.ClientKey;
        }

        public async Task<string> GetParkingLotCheckSumKey(string parkingLotId)
        {
            var parkingLot = await _parkingLotRepository.FindIncludingParkingLotOwnerReadOnly(parkingLotId);
            if (parkingLot == null)
                throw new InvalidInformationException(MessageKeys.PARKING_LOT_NOT_FOUND);
            if (parkingLot.ParkingLotOwner == null || string.IsNullOrEmpty(parkingLot.ParkingLotOwner.CheckSumKey))
                throw new InvalidInformationException(MessageKeys.PARKING_LOT_CHECKSUM_KEY_NOT_FOUND);
            return parkingLot.ParkingLotOwner.CheckSumKey;
        }

        public async Task ConfirmTransaction(PaymentWebHookRequest request)
        {
            var parkingLot = await _parkingLotRepository.Find([plo => plo.SubscriptionTransactionId == request.Data.OrderCode]);
            if (parkingLot != null)
            {
                var signature = _paymentService.GenerateSignature(PayOutDataToStringConverter.ConvertToSignatureString(request.Data),
                                    _paymentSettings.PaymentGatewayCheckSumKey);
                if (signature == request.Signature)
                {
                    if (parkingLot.TempSubscriptionId != null)
                    {
                        parkingLot.SubscriptionId = parkingLot.TempSubscriptionId;
                        parkingLot.ExpiredAt = DateTime.UtcNow.AddMilliseconds(
                            await _subscriptionService.GetDurationOfSubscription(parkingLot.TempSubscriptionId));
                        _parkingLotRepository.Update(parkingLot);
                        await _parkingLotRepository.SaveChangesAsync();
                    }
                }
            }
        }

        public async Task<SubscriptionDetailsDto?> GetSubscriptionByParkingLotId(string parkingLotId)
        {
            var parkingLot = await _parkingLotRepository.Find(parkingLotId);
            if (parkingLot == null)
                throw new InvalidInformationException(MessageKeys.PARKING_LOT_NOT_FOUND);
            return await _subscriptionService.GetDetailsAsync(parkingLot.SubscriptionId);
        }

        public async Task<PaymentResponseDto?> GetLatestPaymentByParkingLotId(string parkingLotId)
        {
            var parkingLot = await _parkingLotRepository.Find(parkingLotId);
            if (parkingLot == null)
                throw new InvalidInformationException(MessageKeys.PARKING_LOT_NOT_FOUND);
            if (string.IsNullOrWhiteSpace(parkingLot.SubscriptionTransactionInformation))
            {
                throw new InvalidInformationException(MessageKeys.PARKING_LOT_SUBSCSRIPTION_PAYMENT_INFO_NOT_FOUND);
            }
            var paymentInfo = JsonSerializer.Deserialize<PaymentResponseDto>(parkingLot.SubscriptionTransactionInformation);
            if (paymentInfo == null)
                throw new InvalidInformationException(MessageKeys.PARKING_LOT_SUBSCSRIPTION_PAYMENT_INFO_NOT_FOUND);
            return paymentInfo;
        }

        public async Task<bool> IsParkingLotUsingWhiteList(string parkingLotId)
        {
            var parkingLot = await _parkingLotRepository.Find(parkingLotId);
            if (parkingLot == null)
                throw new InvalidInformationException(MessageKeys.PARKING_LOT_NOT_FOUND);
            var settings = JsonSerializer.Deserialize<ParkingLotSettings>(parkingLot.Settings);
            return settings?.UseWhiteList ?? false;
        }
    }
}
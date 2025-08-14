using SAPLSServer.Constants;
using SAPLSServer.DTOs.Base;
using SAPLSServer.DTOs.Concrete.PaymentSourceDto;
using SAPLSServer.DTOs.PaginationDto;
using SAPLSServer.Exceptions;
using SAPLSServer.Models;
using SAPLSServer.Repositories.Interfaces;
using SAPLSServer.Services.Interfaces;
using System.Linq.Expressions;

namespace SAPLSServer.Services.Implementations
{
    public class PaymentSourceService : IPaymentSourceService
    {
        private readonly IPaymentSourceRepository _paymentSourceRepository;

        public PaymentSourceService(IPaymentSourceRepository paymentSourceRepository)
        {
            _paymentSourceRepository = paymentSourceRepository;
        }

        public async Task CreatePaymentSource(CreatePaymenSourceRequest request)
        {
            if (string.IsNullOrEmpty(request.BankName))
                throw new InvalidInformationException(MessageKeys.BANK_NAME_REQUIRED);
            
            if (string.IsNullOrEmpty(request.AccountName))
                throw new InvalidInformationException(MessageKeys.ACCOUNT_NAME_REQUIRED);
            
            if (string.IsNullOrEmpty(request.AccountNumber))
                throw new InvalidInformationException(MessageKeys.ACCOUNT_NUMBER_REQUIRED);
            
            if (string.IsNullOrEmpty(request.ParkingLotOwnerId))
                throw new InvalidInformationException(MessageKeys.PARKING_LOT_OWNER_ID_REQUIRED);

            var paymentSource = new PaymentSource
            {
                Id = Guid.NewGuid().ToString(),
                BankName = request.BankName,
                AccountName = request.AccountName,
                AccountNumber = request.AccountNumber,
                ParkingLotOwnerId = request.ParkingLotOwnerId,
                Status = PaymentSourceStatus.Active.ToString(),
                UpdatedAt = DateTime.UtcNow
            };

            await _paymentSourceRepository.AddAsync(paymentSource);
            await _paymentSourceRepository.SaveChangesAsync();
        }

        public async Task UpdatePaymentSource(UpdatePaymentSourceRequest request)
        {
            // Sử dụng phương thức FindById mới thay vì Find
            var paymentSource = await _paymentSourceRepository.FindById(request.Id);
            if (paymentSource == null)
                throw new InvalidInformationException(MessageKeys.PAYMENT_SOURCE_NOT_FOUND);

            paymentSource.BankName = request.BankName;
            paymentSource.AccountName = request.AccountName;
            paymentSource.AccountNumber = request.AccountNumber;
            paymentSource.ParkingLotOwnerId = request.ParkingLotOwnerId;
            paymentSource.UpdatedAt = DateTime.UtcNow;
            
            // Kiểm tra trạng thái hợp lệ - chỉ chấp nhận Active hoặc Inactive
            if (request.Status == PaymentSourceStatus.Active.ToString() || request.Status == PaymentSourceStatus.Inactive.ToString())
            {
                paymentSource.Status = request.Status;
            }
            else
            {
                throw new InvalidInformationException(MessageKeys.INVALID_PAYMENT_SOURCE_STATUS);
            }

            _paymentSourceRepository.Update(paymentSource);
            await _paymentSourceRepository.SaveChangesAsync();
        }

        public async Task DeletePaymentSource(DeleteRequest request)
        {
            // Sử dụng phương thức FindById mới thay vì Find
            var paymentSource = await _paymentSourceRepository.FindById(request.Id);
            if (paymentSource == null)
                throw new InvalidInformationException(MessageKeys.PAYMENT_SOURCE_NOT_FOUND);

            _paymentSourceRepository.Remove(paymentSource);
            await _paymentSourceRepository.SaveChangesAsync();
        }

        public async Task<GetPaymentSourceDto?> GetPaymentSourceDetails(GetDetailsRequest request)
        {
            // Sử dụng phương thức FindById mới thay vì Find
            var paymentSource = await _paymentSourceRepository.FindById(request.Id);
            if (paymentSource == null)
                return null;

            return new GetPaymentSourceDto(paymentSource);
        }

        public async Task<PageResult<GetPaymentSourceDto>> GetPaymentSourcesPage(PageRequest pageRequest, GetListRequest request)
        {
            var criteriaList = new List<Expression<Func<PaymentSource, bool>>>();
            
            // Chỉ thêm điều kiện khi giá trị tồn tại
            if (!string.IsNullOrEmpty(request.SearchCriteria))
                criteriaList.Add(ps => ps.BankName.Contains(request.SearchCriteria) || 
                                       ps.AccountName.Contains(request.SearchCriteria) || 
                                       ps.AccountNumber.Contains(request.SearchCriteria) ||
                                       ps.ParkingLotOwnerId.Contains(request.SearchCriteria));
            
            var totalCount = await _paymentSourceRepository.CountAsync(
                criteriaList.Count > 0 ? criteriaList.ToArray() : null);
                
            var paymentSources = await _paymentSourceRepository.GetPageAsync(
                pageRequest.PageNumber, 
                pageRequest.PageSize, 
                criteriaList.Count > 0 ? criteriaList.ToArray() : null, 
                null, 
                request.Order == OrderType.Asc.ToString());

            var items = paymentSources.Select(ps => new GetPaymentSourceDto(ps)).ToList();
            
            return new PageResult<GetPaymentSourceDto>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = pageRequest.PageNumber,
                PageSize = pageRequest.PageSize
            };
        }

        public async Task<IEnumerable<GetPaymentSourceDto>> GetPaymentSourcesByOwner(string ownerId)
        {
            if (string.IsNullOrEmpty(ownerId))
                throw new InvalidInformationException(MessageKeys.PARKING_LOT_OWNER_ID_REQUIRED);

            try
            {
                // Sử dụng phương thức mới tạo trong repository
                var paymentSources = await _paymentSourceRepository.GetPaymentSourcesByOwnerIdAsync(ownerId);
                return paymentSources.Select(ps => new GetPaymentSourceDto(ps)).ToList();
            }
            catch (Exception ex)
            {
                // Log lỗi để debug
                Console.WriteLine($"Error in GetPaymentSourcesByOwner: {ex.Message}");
                throw; // Re-throw exception để client biết có lỗi xảy ra
            }
        }

        public async Task<PageResult<GetPaymentSourceDto>> GetPaymentSourcesByOwnerPage(string ownerId, PageRequest pageRequest, GetListRequest request)
        {
            if (string.IsNullOrEmpty(ownerId))
                throw new InvalidInformationException(MessageKeys.PARKING_LOT_OWNER_ID_REQUIRED);

            var criteriaList = new List<Expression<Func<PaymentSource, bool>>>
            {
                ps => ps.ParkingLotOwnerId == ownerId
            };
            
            // Chỉ thêm điều kiện khi giá trị tồn tại
            if (!string.IsNullOrEmpty(request.SearchCriteria))
                criteriaList.Add(ps => ps.BankName.Contains(request.SearchCriteria) || 
                                       ps.AccountName.Contains(request.SearchCriteria) || 
                                       ps.AccountNumber.Contains(request.SearchCriteria));
            
            var totalCount = await _paymentSourceRepository.CountAsync(criteriaList.ToArray());
            
            var paymentSources = await _paymentSourceRepository.GetPageAsync(
                pageRequest.PageNumber, 
                pageRequest.PageSize, 
                criteriaList.ToArray(), 
                null, 
                request.Order == OrderType.Asc.ToString());

            var items = paymentSources.Select(ps => new GetPaymentSourceDto(ps)).ToList();
            
            return new PageResult<GetPaymentSourceDto>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = pageRequest.PageNumber,
                PageSize = pageRequest.PageSize
            };
        }

        public async Task<PageResult<GetPaymentSourceDto>> GetPaymentSourcesByStatusPage(string status, PageRequest pageRequest, GetListRequest request)
        {
            if (string.IsNullOrEmpty(status))
                throw new InvalidInformationException(MessageKeys.STATUS_REQUIRED);

            // Kiểm tra trạng thái hợp lệ - chỉ chấp nhận Active hoặc Inactive
            if (status != PaymentSourceStatus.Active.ToString() && status != PaymentSourceStatus.Inactive.ToString())
                throw new InvalidInformationException(MessageKeys.INVALID_PAYMENT_SOURCE_STATUS);

            var criteriaList = new List<Expression<Func<PaymentSource, bool>>>
            {
                ps => ps.Status == status
            };
            
            // Chỉ thêm điều kiện khi giá trị tồn tại
            if (!string.IsNullOrEmpty(request.SearchCriteria))
                criteriaList.Add(ps => ps.BankName.Contains(request.SearchCriteria) || 
                                       ps.AccountName.Contains(request.SearchCriteria) || 
                                       ps.AccountNumber.Contains(request.SearchCriteria) ||
                                       ps.ParkingLotOwnerId.Contains(request.SearchCriteria));
            
            var totalCount = await _paymentSourceRepository.CountAsync(criteriaList.ToArray());
            
            var paymentSources = await _paymentSourceRepository.GetPageAsync(
                pageRequest.PageNumber, 
                pageRequest.PageSize, 
                criteriaList.ToArray(), 
                null, 
                request.Order == OrderType.Asc.ToString());

            var items = paymentSources.Select(ps => new GetPaymentSourceDto(ps)).ToList();
            
            return new PageResult<GetPaymentSourceDto>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = pageRequest.PageNumber,
                PageSize = pageRequest.PageSize
            };
        }
    }
}
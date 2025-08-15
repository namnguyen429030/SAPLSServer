using System.ComponentModel.DataAnnotations;
using SAPLSServer.Constants;
using SAPLSServer.DTOs.Concrete.ShiftDiaryDtos;
using SAPLSServer.DTOs.PaginationDto;
using SAPLSServer.Exceptions;
using SAPLSServer.Models;
using SAPLSServer.Repositories.Interfaces;
using SAPLSServer.Services.Interfaces;

namespace SAPLSServer.Services.Implementations
{
    public class ShiftDiaryService : IShiftDiaryService
    {
        private readonly IShiftDiaryRepository _shiftDiaryRepository;
        private readonly IParkingLotService _parkingLotService;

        public ShiftDiaryService(IShiftDiaryRepository shiftDiaryRepository, IParkingLotService parkingLotService)
        {
            _shiftDiaryRepository = shiftDiaryRepository;
            _parkingLotService = parkingLotService;
        }

        public async Task<ShiftDiaryDetailsDto> CreateAsync(CreateShiftDiaryRequest request, string senderId)
        {
            if(!await _parkingLotService.IsParkingLotValid(request.ParkingLotId))
            {
                throw new InvalidInformationException(MessageKeys.PARKING_LOT_NOT_FOUND);
            }
            if(!await _parkingLotService.IsParkingLotStaff(request.ParkingLotId, senderId))
            {
                throw new UnauthorizedAccessException(MessageKeys.UNAUTHORIZED_ACCESS);
            }
            var diary = new ShiftDiary
            {
                Id = Guid.NewGuid().ToString(),
                Header = request.Header,
                Body = request.Body,
                ParkingLotId = request.ParkingLotId,
                SenderId = senderId,
                CreatedAt = DateTime.UtcNow
            };
            await _shiftDiaryRepository.AddAsync(diary);
            await _shiftDiaryRepository.SaveChangesAsync();
            var created = await _shiftDiaryRepository.FindIncludingSenderReadOnly(diary.Id)
                         ?? throw new InvalidInformationException("Failed to create shift diary.");
            return new ShiftDiaryDetailsDto(created);
        }

        public async Task<List<ShiftDiarySummaryDto>> GetListAsync(GetShiftDiaryListRequest request)
        {
            var filters = new List<System.Linq.Expressions.Expression<Func<ShiftDiary, bool>>>();
            if (!string.IsNullOrEmpty(request.ParkingLotId))
                filters.Add(d => d.ParkingLotId == request.ParkingLotId);
            if (request.UploadedDate.HasValue)
                filters.Add(d => d.CreatedAt.Date == request.UploadedDate.Value.ToDateTime(TimeOnly.MinValue).Date);

            var diaries = await _shiftDiaryRepository.GetAllAsync(filters.ToArray());
            return diaries.Select(d => new ShiftDiarySummaryDto(d)).ToList();
        }

        public async Task<PageResult<ShiftDiarySummaryDto>> GetPageAsync(PageRequest pageRequest, GetShiftDiaryListRequest request)
        {            
            var filters = new List<System.Linq.Expressions.Expression<Func<ShiftDiary, bool>>>();
            if (!string.IsNullOrEmpty(request.ParkingLotId))
                filters.Add(d => d.ParkingLotId == request.ParkingLotId);
            if (request.UploadedDate.HasValue)
                filters.Add(d => d.CreatedAt.Date == request.UploadedDate.Value.ToDateTime(TimeOnly.MinValue).Date);

            var diaries = await _shiftDiaryRepository.GetPageAsync(
                pageRequest.PageNumber,
                pageRequest.PageSize,
                filters.ToArray(),
                null,
                false
            );
            var total = await _shiftDiaryRepository.CountAsync(filters.ToArray());
            return new PageResult<ShiftDiarySummaryDto>
            {
                Items = diaries.Select(d => new ShiftDiarySummaryDto(d)).ToList(),
                TotalCount = total,
                PageNumber = pageRequest.PageNumber,
                PageSize = pageRequest.PageSize
            };
        }

        public async Task<ShiftDiaryDetailsDto?> GetDetailsAsync(string id)
        {
            var diary = await _shiftDiaryRepository.FindIncludingSenderReadOnly(id);
            return diary == null ? null : new ShiftDiaryDetailsDto(diary);
        }
    }
}
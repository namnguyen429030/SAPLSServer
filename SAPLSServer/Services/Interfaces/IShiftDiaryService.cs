using SAPLSServer.DTOs.Concrete.Pagination;
using SAPLSServer.DTOs.Concrete.ShiftDiary;

namespace SAPLSServer.Services.Interfaces
{
    public interface IShiftDiaryService
    {
        /// <summary>
        /// Creates a new shift diary with the provided details.
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task CreateShiftDiary(CreateShiftDiaryRequest dto);
        /// <summary>
        /// Retrieves the details of a shift diary by its unique identifier (ID).
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ShiftDiaryDetailsDto?> GetShiftDiaryDetails(string id);
        /// <summary>
        /// Retrieves a paginated list of shift diaries with optional search criteria.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<PageResult<ShiftDiarySummaryDto>> GetShiftDiariesPage(PageRequest request);
        /// <summary>
        /// Retrieves a paginated list of shift diaries for a specific staff member, identified by their unique identifier (staffId).
        /// </summary>
        /// <param name="staffId"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<PageResult<ShiftDiarySummaryDto>> GetShiftDiariesByStaffPage(string staffId, PageRequest request);
        Task<PageResult<ShiftDiarySummaryDto>> GetShiftDiariesByParkingLotPage(string parkingLotId, PageRequest request);
        Task<PageResult<ShiftDiarySummaryDto>> GetShiftDiariesByDateRangePage(DateTime startDate, DateTime endDate, PageRequest request);
    }
}


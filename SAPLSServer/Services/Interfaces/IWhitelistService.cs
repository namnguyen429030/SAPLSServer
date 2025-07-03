using SAPLSServer.DTOs.Concrete.Pagination;
using SAPLSServer.DTOs.Concrete.Whitelist;

namespace SAPLSServer.Services.Interfaces
{
    public interface IWhitelistService
    {
        Task AddAttendantToWhitelist(AddAttendantToWhitelistRequest dto);
        Task UpdateWhitelistAttendantExpireDate(UpdateWhitelistAttendantExpireDateRequest dto);
        Task<WhitelistAttendantDto?> GetWhitelistAttendantDetails(string id);
        Task<PageResult<WhitelistAttendantDto>> GetWhitelistAttendantsPage(PageRequest request);
        Task<PageResult<WhitelistAttendantDto>> GetWhitelistAttendantsByEmailPage(string email, PageRequest request);
        Task<PageResult<WhitelistAttendantDto>> GetActiveWhitelistAttendantsPage(PageRequest request);
        Task<PageResult<WhitelistAttendantDto>> GetExpiredWhitelistAttendantsPage(PageRequest request);
        Task<PageResult<WhitelistAttendantDto>> GetWhitelistAttendantsByParkingLotPage(string parkingLotId, PageRequest request);
        Task RemoveAttendantFromWhitelist(RemoveAttendantFromWhitelistRequest dto);
    }
}


using SAPLSServer.DTOs.Concrete.Pagination;
using SAPLSServer.DTOs.Concrete.User;

namespace SAPLSServer.Services.Interfaces
{
    public interface IStaffProfileService
    {
        /// <summary>
        /// Creates a new staff profile with the provided details.
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task CreateStaff(CreateStaffProfileRequest dto);
        /// <summary>
        /// Updates an existing staff profile with the provided details.
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task UpdateStaff(UpdateStaffProfileRequest dto);
        /// <summary>
        /// Retrieves the details of a staff profile by its unique identifier (ID).
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<StaffProfileDetailsDto?> GetStaffProfileDetails(string id);
        /// <summary>
        /// Retrieves a paginated list of staff profiles with optional search criteria.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<PageResult<StaffProfileDetailsDto>> GetStaffsPageByParkingLot(PageRequest request);
    }
}

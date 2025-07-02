using SAPLSServer.DTOs.Base;
using SAPLSServer.DTOs.Concrete.Pagination;
using SAPLSServer.DTOs.Concrete.User;

namespace SAPLSServer.Services.Interfaces
{
    public interface IParkingLotOwnerProfileService
    {
        /// <summary>
        /// Creates a new parking lot owner profile.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task CreateParkingLotOwner(CreateParkingLotOwnerProfileRequest request);
        /// <summary>
        /// Updates an existing parking lot owner profile.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task UpdateParkingLotOwner(UpdateParkingLotOwnerProfileRequest request);
        /// <summary>
        /// Get parking lot owner profile's details by UID.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<ParkingLotOwnerProfileDetailsDto?> GetParkingLotOwnerProfileDetails(GetDetailsRequest request);
        /// <summary>
        /// Get a paginated list of parking lot owner profiles.
        /// </summary>
        /// <param name="pageRequest"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<PageResult<ParkingLotOwnerProfileDetailsDto>> GetParkingLotOwnersPage(PageRequest pageRequest, GetListRequest request);
    }
}

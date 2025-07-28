using SAPLSServer.DTOs.Base;
using SAPLSServer.DTOs.Concrete;

namespace SAPLSServer.Services.Interfaces
{
    public interface IParkingLotService
    {
        /// <summary>
        /// Creates a new parking lot with the provided details.
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task CreateParkingLot(CreateParkingLotRequest dto);
        /// <summary>
        /// Updates the basic information of an existing parking lot.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task UpdateParkingLotBasicInformation(UpdateParkingLotBasicInformationRequest request);
        /// <summary>
        /// Updates the address of an existing parking lot.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task UpdateParkingLotAddress(UpdateParkingLotAddressRequest request);
        /// <summary>
        /// Retrieves the details of a parking lot by its unique identifier (ID).
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<ParkingLotDetailsDto?> GetParkingLotDetails(GetDetailsRequest request);
        /// <summary>
        /// Retrieves a paginated list of parking lots based on the provided page request and filter criteria.
        /// </summary>
        /// <param name="pageRequest"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<PageResult<ParkingLotSummaryDto>> GetParkingLotsPage(PageRequest pageRequest, GetListRequest request);
        /// <summary>
        /// Retrieves a paginated list of parking lots owned by a specific owner, identified by their unique identifier (ID).
        /// </summary>
        /// <param name="ownerId"></param>
        /// <param name="pageRequest"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<PageResult<ParkingLotSummaryDto>> GetParkingLotsByOwnerPage(string ownerId, PageRequest pageRequest, GetListRequest request);
        /// <summary>
        /// Deletes a parking lot by its unique identifier (ID).
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task DeleteParkingLot(DeleteRequest request);
    }
}


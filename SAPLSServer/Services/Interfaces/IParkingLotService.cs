using SAPLSServer.DTOs.Base;
using SAPLSServer.DTOs.Concrete.ParkingLotDto;
using SAPLSServer.DTOs.PaginationDto;

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
        /// Retrieves a paginated list of owned parking lots based on the provided page request and filter criteria.
        /// </summary>
        /// <param name="pageRequest"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<PageResult<ParkingLotSummaryDto>> GetParkingLotsPage(PageRequest pageRequest, 
            GetParkingLotListRequest request);
        /// <summary>
        /// Retrieves a own list of owned parking lots based on the filter criteria.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<List<ParkingLotSummaryDto>> GetParkingLots(GetParkingLotListRequest request);
        /// <summary>
        /// Deletes a parking lot by its unique identifier (ID).
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task DeleteParkingLot(DeleteRequest request);
    }
}


using SAPLSServer.DTOs.Base;
using SAPLSServer.DTOs.Concrete.ParkingSessionDto;
using SAPLSServer.DTOs.PaginationDto;

namespace SAPLSServer.Services.Interfaces
{
    public interface IParkingSessionService
    {
        /// <summary>
        /// Creates a new parking session with the provided details.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task CreateParkingSession(CreateParkingSessionRequest request);
        /// <summary>
        /// Updates the check-out date and time of an existing parking session.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task UpdateParkingSessionCheckOutDateTime(UpdateParkingSessionCheckOutDateTimeRequest request);
        /// <summary>
        /// Updates the exit details of an existing parking session, including the exit date and time.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task UpdateParkingSessionExit(UpdateParkingSessionExitRequest request);
        /// <summary>
        /// Retrieves the details of a parking session for a client by its unique identifier (ID).
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<ParkingSessionDetailsForClientDto?> GetParkingSessionDetailsForClient(GetDetailsRequest request);
        /// <summary>
        /// Retrieves the details of a parking session for a parking lot by its unique identifier (ID).
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<ParkingSessionDetailsForParkingLotDto?> GetParkingSessionDetailsForParkingLot(GetDetailsRequest request);
        /// <summary>
        /// Retrieves a paginated list of parking sessions for a specific client with optional search criteria.
        /// </summary>
        /// <param name="pageRequest"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<PageResult<ParkingSessionSummaryForClientDto>> GetParkingSessionsForClientPage(PageRequest pageRequest, GetParkingSessionListByClientIdRequest request);
        /// <summary>
        /// Retrieves all parking sessions for a specific client with optional search criteria.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<List<ParkingSessionSummaryForClientDto>> GetParkingSessionsForClient(GetParkingSessionListByClientIdRequest request);
        /// <summary>
        /// Retrieves a paginated list of parking sessions for a specific parking lot with optional search criteria.
        /// </summary>
        /// <param name="pageRequest"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<PageResult<ParkingSessionSummaryForParkingLotDto>> GetParkingSessionsForParkingLotPage(PageRequest pageRequest, GetParkingSessionListByParkingLotIdRequest request);
    }
}


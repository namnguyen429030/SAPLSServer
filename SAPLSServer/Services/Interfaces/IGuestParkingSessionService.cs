using SAPLSServer.DTOs.Concrete.GuestParkingSessionDtos;
using SAPLSServer.DTOs.Concrete.ParkingSessionDtos;
using SAPLSServer.DTOs.PaginationDto;

namespace SAPLSServer.Services.Interfaces
{
    /// <summary>
    /// Provides operations for managing guest parking sessions, including check-in, finishing, and retrieval for parking lot use.
    /// </summary>
    public interface IGuestParkingSessionService
    {
        /// <summary>
        /// Gets detailed information about a guest parking session for a specific parking lot.
        /// </summary>
        /// <param name="sessionId">The ID of the guest parking session.</param>
        /// <returns>Guest parking session details for the parking lot, or null if not found.</returns>
        Task<GuestParkingSessionDetailsForParkingLotDto?> GetSessionDetailsForParkingLot(string sessionId);

        /// <summary>
        /// Retrieves a list of guest parking session summaries for a parking lot based on the specified request.
        /// </summary>
        /// <param name="request">The request containing parking lot and filter information.</param>
        /// <returns>List of guest parking session summaries for the parking lot.</returns>
        Task<List<GuestParkingSessionSummaryForParkingLotDto>> GetSessionsByParkingLot(
            GetGuestParkingSessionListByParkingLotIdRequest request);

        /// <summary>
        /// Checks in a vehicle and creates a new guest parking session.
        /// Handles file upload for entry captures and associates the session with the correct staff.
        /// </summary>
        /// <param name="request">The check-in request details, including entry capture files.</param>
        /// <param name="staffId">The ID of the staff performing the check-in.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task CheckIn(CheckInParkingSessionRequest request, string staffId);

        /// <summary>
        /// Finishes the corresponding guest parking session, uploads exit capture files, and updates session status.
        /// </summary>
        /// <param name="request">The finish request details, including exit capture files.</param>
        /// <param name="staffId">The ID of the staff performing the finish operation.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task Finish(FinishParkingSessionRequest request, string staffId);

        /// <summary>
        /// Retrieves a paged list of guest parking session summaries for a parking lot.
        /// </summary>
        /// <param name="request">The page request containing parking lot and paging information.</param>
        /// <param name="listRequest">The filter and search criteria for the parking lot sessions.</param>
        /// <returns>Paged result of guest parking session summaries for the parking lot.</returns>
        Task<PageResult<GuestParkingSessionSummaryForParkingLotDto>> GetPageByParkingLot(PageRequest request,
            GetGuestParkingSessionListByParkingLotIdRequest listRequest);
    }
}
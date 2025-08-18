using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SAPLSServer.Constants;
using SAPLSServer.DTOs.Concrete.ParkingSessionDtos;
using SAPLSServer.DTOs.PaginationDto;
using SAPLSServer.Exceptions;
using SAPLSServer.Services.Interfaces;
using System.Security.Claims;

namespace SAPLSServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ParkingSessionController : ControllerBase
    {
        private readonly IParkingSessionService _parkingSessionService;

        public ParkingSessionController(IParkingSessionService parkingSessionService)
        {
            _parkingSessionService = parkingSessionService;
        }

        /// <summary>
        /// Gets detailed information about a parking session for a client.
        /// </summary>
        /// <param name="sessionId">The ID of the parking session.</param>
        /// <returns>Parking session details for the client.</returns>
        [HttpGet("client/{sessionId}")]
        public async Task<IActionResult> GetSessionDetailsForClient(string sessionId)
        {
            try
            {
                var result = await _parkingSessionService.GetSessionDetailsForClient(sessionId);
                if (result == null)
                    return NotFound(new { error = MessageKeys.PARKING_SESSION_NOT_FOUND });

                return Ok(new
                {
                    message = MessageKeys.GET_PARKING_SESSION_DETAILS_SUCCESSFULLY,
                    data = result
                });
            }
            catch (InvalidInformationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (UnauthorizedAccessException ex)
            {
                return StatusCode(403, new { error = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { error = MessageKeys.UNEXPECTED_ERROR });
            }
        }

        /// <summary>
        /// Gets detailed information about a parking session for a parking lot.
        /// </summary>
        /// <param name="sessionId">The ID of the parking session.</param>
        /// <returns>Parking session details for the parking lot.</returns>
        [HttpGet("lot/{sessionId}")]
        public async Task<IActionResult> GetSessionDetailsForParkingLot(string sessionId)
        {
            try
            {
                var result = await _parkingSessionService.GetSessionDetailsForParkingLot(sessionId);
                if (result == null)
                    return NotFound(new { error = MessageKeys.PARKING_SESSION_NOT_FOUND });

                return Ok(new
                {
                    message = MessageKeys.GET_PARKING_SESSION_DETAILS_SUCCESSFULLY,
                    data = result
                });
            }
            catch (InvalidInformationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (UnauthorizedAccessException ex)
            {
                return StatusCode(403, new { error = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { error = MessageKeys.UNEXPECTED_ERROR });
            }
        }

        /// <summary>
        /// Retrieves parking session summaries for a specific client.
        /// </summary>
        /// <param name="request">The request containing client and filter information.</param>
        /// <returns>List of parking session summaries for the client.</returns>
        [HttpGet("by-client")]
        public async Task<IActionResult> GetSessionsByClient([FromQuery] GetParkingSessionListByClientIdRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var result = await _parkingSessionService.GetSessionsByClient(request);
                return Ok(new
                {
                    message = MessageKeys.GET_PARKING_SESSIONS_PAGE_SUCCESSFULLY,
                    data = result
                });
            }
            catch (InvalidInformationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { error = MessageKeys.UNEXPECTED_ERROR });
            }
        }

        /// <summary>
        /// Retrieves parking session summaries for a specific parking lot.
        /// </summary>
        /// <param name="request">The request containing parking lot and filter information.</param>
        /// <returns>List of parking session summaries for the parking lot.</returns>
        [HttpPost("by-lot")]
        public async Task<IActionResult> GetSessionsByParkingLot([FromBody] GetParkingSessionListByParkingLotIdRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var result = await _parkingSessionService.GetSessionsByParkingLot(request);
                return Ok(new
                {
                    message = MessageKeys.GET_PARKING_SESSIONS_PAGE_SUCCESSFULLY,
                    data = result
                });
            }
            catch (InvalidInformationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { error = MessageKeys.UNEXPECTED_ERROR });
            }
        }

        /// <summary>
        /// Retrieves parking sessions owned by a specific client.
        /// </summary>
        /// <param name="request">The request containing ownership and filter information.</param>
        /// <param name="clientId">The ID of the client who owns the sessions.</param>
        /// <returns>List of owned parking session summaries.</returns>
        [HttpGet("owned/{clientId}")]
        public async Task<IActionResult> GetOwnedSessions([FromQuery] GetOwnedParkingSessionListRequest request, string clientId)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var result = await _parkingSessionService.GetOwnedSessions(request, clientId);
                return Ok(new
                {
                    message = MessageKeys.GET_PARKING_SESSIONS_PAGE_SUCCESSFULLY,
                    data = result
                });
            }
            catch (InvalidInformationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { error = MessageKeys.UNEXPECTED_ERROR });
            }
        }

        /// <summary>
        /// Retrieves a paginated list of parking sessions for a specific client.
        /// </summary>
        /// <param name="pageRequest">The pagination request.</param>
        /// <param name="listRequest">The filter criteria.</param>
        /// <returns>Paged result of parking session summaries.</returns>
        [HttpPost("page/by-client")]
        public async Task<IActionResult> GetPageByClient([FromBody] PageRequest pageRequest, [FromQuery] GetParkingSessionListByClientIdRequest listRequest)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var result = await _parkingSessionService.GetPageByClient(pageRequest, listRequest);
                return Ok(new
                {
                    message = MessageKeys.GET_PARKING_SESSIONS_PAGE_SUCCESSFULLY,
                    data = result
                });
            }
            catch (InvalidInformationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { error = MessageKeys.UNEXPECTED_ERROR });
            }
        }

        /// <summary>
        /// Retrieves a paginated list of parking sessions for a specific parking lot.
        /// </summary>
        /// <param name="pageRequest">The pagination request.</param>
        /// <param name="listRequest">The filter criteria.</param>
        /// <returns>Paged result of parking session summaries.</returns>
        [HttpPost("page/by-lot")]
        public async Task<IActionResult> GetPageByParkingLot([FromBody] PageRequest pageRequest, [FromQuery] GetParkingSessionListByParkingLotIdRequest listRequest)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var result = await _parkingSessionService.GetPageByParkingLot(pageRequest, listRequest);
                return Ok(new
                {
                    message = MessageKeys.GET_PARKING_SESSIONS_PAGE_SUCCESSFULLY,
                    data = result
                });
            }
            catch (InvalidInformationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { error = MessageKeys.UNEXPECTED_ERROR });
            }
        }

        /// <summary>
        /// Retrieves a paginated list of parking sessions owned by a specific client.
        /// </summary>
        /// <param name="pageRequest">The pagination request.</param>
        /// <param name="listRequest">The filter criteria.</param>
        /// <param name="clientId">The ID of the client who owns the sessions.</param>
        /// <returns>Paged result of owned parking session summaries.</returns>
        [HttpPost("page/owned/{clientId}")]
        public async Task<IActionResult> GetPageByOwnedSessions([FromBody] PageRequest pageRequest, [FromQuery] GetOwnedParkingSessionListRequest listRequest, string clientId)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var result = await _parkingSessionService.GetPageByOwnedSessions(pageRequest, listRequest, clientId);
                return Ok(new
                {
                    message = MessageKeys.GET_PARKING_SESSIONS_PAGE_SUCCESSFULLY,
                    data = result
                });
            }
            catch (InvalidInformationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { error = MessageKeys.UNEXPECTED_ERROR });
            }
        }

        /// <summary>
        /// Checks in a vehicle and creates a new parking session.
        /// </summary>
        /// <param name="request">The check-in request details, including entry capture files.</param>
        /// <returns>Success response.</returns>
        [HttpPost("check-in")]
        public async Task<IActionResult> CheckIn([FromForm] CheckInParkingSessionRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var staffId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(staffId))
                    return Unauthorized(new { error = MessageKeys.STAFF_PROFILE_ID_REQUIRED });

                await _parkingSessionService.CheckIn(request, staffId);
                return Ok(new { message = MessageKeys.PARKING_SESSION_CREATED_SUCCESSFULLY });
            }
            catch (InvalidInformationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (UnauthorizedAccessException ex)
            {
                return StatusCode(403, new { error = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { error = MessageKeys.UNEXPECTED_ERROR });
            }
        }

        /// <summary>
        /// Checks out a vehicle and updates the parking session.
        /// </summary>
        /// <param name="request">The check-out request details.</param>
        /// <returns>Success response.</returns>
        [HttpPost("check-out")]
        public async Task<IActionResult> CheckOut([FromBody] CheckOutParkingSessionRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId))
                    return Unauthorized(new { error = MessageKeys.USER_ID_REQUIRED });

                await _parkingSessionService.CheckOut(request, userId);
                return Ok(new { message = MessageKeys.PARKING_SESSION_CHECKOUT_UPDATED_SUCCESSFULLY });
            }
            catch (InvalidInformationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (UnauthorizedAccessException ex)
            {
                return StatusCode(403, new { error = ex.Message });
            }
            catch (ParkingSessionException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { error = MessageKeys.UNEXPECTED_ERROR });
            }
        }

        /// <summary>
        /// Finishes a parking session by uploading exit captures.
        /// </summary>
        /// <param name="request">The finish request details, including exit capture files.</param>
        /// <returns>Success response.</returns>
        [HttpPost("finish")]
        public async Task<IActionResult> Finish([FromForm] FinishParkingSessionRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var staffId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(staffId))
                    return Unauthorized(new { error = MessageKeys.STAFF_PROFILE_ID_REQUIRED });

                await _parkingSessionService.Finish(request, staffId);
                return Ok(new { message = MessageKeys.PARKING_SESSION_EXIT_UPDATED_SUCCESSFULLY });
            }
            catch (InvalidInformationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (UnauthorizedAccessException ex)
            {
                return StatusCode(403, new { error = ex.Message });
            }
            catch (ParkingSessionException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { error = MessageKeys.UNEXPECTED_ERROR });
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sessionId"></param>
        /// <returns></returns>
        [HttpGet("transaction-id/{id}")]
        public async Task<IActionResult> GetSessionByTransactionId(string sessionId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(sessionId))
                    return BadRequest(new { error = MessageKeys.PARKING_SESSION_ID_REQUIRED });
                var result = await _parkingSessionService.GetSessionTransactionId(sessionId);
                if (result == null)
                    return NotFound(new { error = MessageKeys.TRANSACTION_ID_NOT_FOUND });
                return Ok(new
                {
                    message = MessageKeys.GET_PARKING_SESSION_DETAILS_SUCCESSFULLY,
                    data = result
                });
            }
            catch (Exception)
            {
                return StatusCode(500, new { error = MessageKeys.UNEXPECTED_ERROR });
            }
        }
    }
}
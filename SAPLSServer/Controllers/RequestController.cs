using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SAPLSServer.Constants;
using SAPLSServer.DTOs.Concrete.RequestDtos;
using SAPLSServer.DTOs.PaginationDto;
using SAPLSServer.Exceptions;
using SAPLSServer.Services.Interfaces;
using System.Security.Claims;

namespace SAPLSServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class RequestController : ControllerBase
    {
        private readonly IRequestService _requestService;

        public RequestController(IRequestService requestService)
        {
            _requestService = requestService;
        }

        /// <summary>
        /// Creates a new request with optional file attachments.
        /// </summary>
        /// <param name="request">The request creation details, including optional attachments.</param>
        /// <returns>Success response.</returns>
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateRequest([FromForm] CreateRequestRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var senderId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(senderId))
                    return Unauthorized(new { error = MessageKeys.USER_ID_REQUIRED });

                await _requestService.Create(request, senderId);
                return Ok(new { message = MessageKeys.REQUEST_CREATED_SUCCESSFULLY });
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
        /// Gets detailed information about a request by its ID.
        /// </summary>
        /// <param name="id">The ID of the request.</param>
        /// <returns>Request details.</returns>
        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetRequestById(string id)
        {
            try
            {
                var result = await _requestService.GetById(id);
                if (result == null)
                    return NotFound(new { error = MessageKeys.REQUEST_NOT_FOUND });

                return Ok(new 
                { 
                    message = MessageKeys.GET_REQUEST_DETAILS_SUCCESSFULLY, 
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
        /// Retrieves a list of requests based on specified criteria.
        /// </summary>
        /// <param name="request">The filter criteria for requests.</param>
        /// <returns>List of request summaries.</returns>
        [HttpGet("list")]
        [Authorize]
        public async Task<IActionResult> GetRequestList([FromQuery] GetRequestListByUserIdRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var result = await _requestService.GetList(request);
                return Ok(new 
                { 
                    message = MessageKeys.GET_REQUESTS_PAGE_SUCCESSFULLY, 
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
        /// Retrieves a paginated list of requests based on specified criteria.
        /// </summary>
        /// <param name="pageRequest">The pagination request.</param>
        /// <param name="listRequest">The filter criteria for requests.</param>
        /// <returns>Paged result of request summaries.</returns>
        [HttpGet("page")]
        [Authorize]
        public async Task<IActionResult> GetRequestPage([FromQuery] PageRequest pageRequest, [FromQuery] GetRequestListByUserIdRequest listRequest)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var result = await _requestService.GetPage(pageRequest, listRequest);
                return Ok(new 
                { 
                    message = MessageKeys.GET_REQUESTS_PAGE_SUCCESSFULLY, 
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
        /// Updates the status of a request and processes associated data if resolved.
        /// </summary>
        /// <param name="request">The request status update details.</param>
        /// <returns>Success response.</returns>
        [HttpPut("status")]
        [Authorize(Policy = Accessibility.ADMIN_ACCESS)]
        public async Task<IActionResult> UpdateRequestStatus([FromBody] UpdateRequestRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var adminId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(adminId))
                    return Unauthorized(new { error = MessageKeys.ADMIN_PROFILE_ID_REQUIRED });

                await _requestService.UpdateRequestStatus(request, adminId);
                return Ok(new { message = MessageKeys.REQUEST_UPDATED_SUCCESSFULLY });
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
        /// Gets requests submitted by the current user.
        /// </summary>
        /// <param name="status">Optional status filter.</param>
        /// <param name="startDate">Optional start date filter.</param>
        /// <param name="endDate">Optional end date filter.</param>
        /// <param name="searchCriteria">Optional search criteria.</param>
        /// <returns>List of user's request summaries.</returns>
        [HttpGet("my-requests")]
        [Authorize]
        public async Task<IActionResult> GetMyRequests(
            [FromQuery] string? status = null,
            [FromQuery] DateOnly? startDate = null,
            [FromQuery] DateOnly? endDate = null,
            [FromQuery] string? searchCriteria = null)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId))
                    return Unauthorized(new { error = MessageKeys.USER_ID_REQUIRED });

                var request = new GetRequestListByUserIdRequest
                {
                    UserId = userId,
                    Status = status,
                    StartDate = startDate,
                    EndDate = endDate,
                    SearchCriteria = searchCriteria
                };

                var result = await _requestService.GetList(request);
                return Ok(new 
                { 
                    message = MessageKeys.GET_REQUESTS_PAGE_SUCCESSFULLY, 
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
        /// Gets paginated requests submitted by the current user.
        /// </summary>
        /// <param name="pageRequest">The pagination request.</param>
        /// <param name="status">Optional status filter.</param>
        /// <param name="startDate">Optional start date filter.</param>
        /// <param name="endDate">Optional end date filter.</param>
        /// <param name="searchCriteria">Optional search criteria.</param>
        /// <returns>Paged result of user's request summaries.</returns>
        [HttpGet("my-requests/page")]
        [Authorize]
        public async Task<IActionResult> GetMyRequestsPage(
            [FromQuery] PageRequest pageRequest,
            [FromQuery] string? status = null,
            [FromQuery] DateOnly? startDate = null,
            [FromQuery] DateOnly? endDate = null,
            [FromQuery] string? searchCriteria = null)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId))
                    return Unauthorized(new { error = MessageKeys.USER_ID_REQUIRED });

                var listRequest = new GetRequestListByUserIdRequest
                {
                    UserId = userId,
                    Status = status,
                    StartDate = startDate,
                    EndDate = endDate,
                    SearchCriteria = searchCriteria
                };

                var result = await _requestService.GetPage(pageRequest, listRequest);
                return Ok(new 
                { 
                    message = MessageKeys.GET_REQUESTS_PAGE_SUCCESSFULLY, 
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
        /// Gets all requests for admin review (Admin only).
        /// </summary>
        /// <param name="userId">Optional user ID filter.</param>
        /// <param name="status">Optional status filter.</param>
        /// <param name="startDate">Optional start date filter.</param>
        /// <param name="endDate">Optional end date filter.</param>
        /// <param name="searchCriteria">Optional search criteria.</param>
        /// <returns>List of all request summaries.</returns>
        [HttpGet("admin/all")]
        [Authorize(Policy = Accessibility.ADMIN_ACCESS)]
        public async Task<IActionResult> GetAllRequestsForAdmin(
            [FromQuery] string? userId = null,
            [FromQuery] string? status = null,
            [FromQuery] DateOnly? startDate = null,
            [FromQuery] DateOnly? endDate = null,
            [FromQuery] string? searchCriteria = null)
        {
            try
            {
                var request = new GetRequestListByUserIdRequest
                {
                    UserId = userId ?? string.Empty, // If no userId provided, get all requests
                    Status = status,
                    StartDate = startDate,
                    EndDate = endDate,
                    SearchCriteria = searchCriteria
                };

                var result = await _requestService.GetList(request);
                return Ok(new 
                { 
                    message = MessageKeys.GET_REQUESTS_PAGE_SUCCESSFULLY, 
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
        /// Gets paginated requests for admin review (Admin only).
        /// </summary>
        /// <param name="pageRequest">The pagination request.</param>
        /// <param name="userId">Optional user ID filter.</param>
        /// <param name="status">Optional status filter.</param>
        /// <param name="startDate">Optional start date filter.</param>
        /// <param name="endDate">Optional end date filter.</param>
        /// <param name="searchCriteria">Optional search criteria.</param>
        /// <returns>Paged result of all request summaries.</returns>
        [HttpGet("admin/page")]
        [Authorize(Policy = Accessibility.ADMIN_ACCESS)]
        public async Task<IActionResult> GetAllRequestsPageForAdmin(
            [FromQuery] PageRequest pageRequest,
            [FromQuery] string? userId = null,
            [FromQuery] string? status = null,
            [FromQuery] DateOnly? startDate = null,
            [FromQuery] DateOnly? endDate = null,
            [FromQuery] string? searchCriteria = null)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var listRequest = new GetRequestListByUserIdRequest
                {
                    UserId = userId ?? string.Empty, // If no userId provided, get all requests
                    Status = status,
                    StartDate = startDate,
                    EndDate = endDate,
                    SearchCriteria = searchCriteria
                };

                var result = await _requestService.GetPage(pageRequest, listRequest);
                return Ok(new 
                { 
                    message = MessageKeys.GET_REQUESTS_PAGE_SUCCESSFULLY, 
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
    }
}
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SAPLSServer.Constants;
using SAPLSServer.DTOs.Base;
using SAPLSServer.DTOs.Concrete.IncidenceReportDtos;
using SAPLSServer.DTOs.PaginationDto;
using SAPLSServer.Exceptions;
using SAPLSServer.Services.Interfaces;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SAPLSServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Policy = Accessibility.PARKING_LOT_OWNER_OR_STAFF_ACCESS)]
    public class IncidentReportController : ControllerBase
    {
        private readonly IIncidentReportService _incidentReportService;

        public IncidentReportController(IIncidentReportService incidentReportService)
        {
            _incidentReportService = incidentReportService;
        }

        /// <summary>
        /// Creates a new incident report (Staff only).
        /// </summary>
        [HttpPost]
        [Authorize(Policy = Accessibility.STAFF_ACCESS)]
        public async Task<IActionResult> Create([FromBody] CreateIncidentReportRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var reporterId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
            await _incidentReportService.CreateIncidentReport(request, reporterId);
            return Ok(new { message = MessageKeys.SHIFT_DIARY_CREATED_SUCCESSFULLY });
        }

        /// <summary>
        /// Updates the status of an incident report (Parking Lot Owner only).
        /// </summary>
        [HttpPut("status")]
        [Authorize(Policy = Accessibility.PARKING_LOT_OWNER_ACCESS)]
        public async Task<IActionResult> UpdateStatus([FromBody] UpdateIncidentReportStatusRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var performerId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
            await _incidentReportService.UpdateIncidentReportStatus(request, performerId);
            return Ok(new { message = MessageKeys.SHARING_STATUS_UPDATED_SUCCESSFULLY });
        }

        /// <summary>
        /// Gets the details of an incident report by ID.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDetails(string id)
        {
            var result = await _incidentReportService.GetIncidentReportDetails(new GetDetailsRequest { Id = id });
            if (result == null)
                return NotFound(new { message = MessageKeys.INCIDENT_REPORT_NOT_FOUND });
            return Ok(result);
        }

        /// <summary>
        /// Gets a paginated list of incident reports.
        /// </summary>
        [HttpGet("page")]
        public async Task<IActionResult> GetPage([FromQuery] PageRequest pageRequest, [FromQuery] GetIncidenReportListRequest request)
        {
            var result = await _incidentReportService.GetIncidentReportsPage(pageRequest, request);
            return Ok(result);
        }
    }
}
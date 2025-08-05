using Microsoft.AspNetCore.Mvc;
using SAPLSServer.DTOs.Base;
using SAPLSServer.DTOs.Concrete.IncidentReportDto;
using SAPLSServer.DTOs.PaginationDto;
using SAPLSServer.Services.Interfaces;

namespace SAPLSServer.Controllers
{
    /// <summary>
    /// Controller for managing incident report operations including creation, updates, and retrieval.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class IncidentReportController : ControllerBase
    {
        private readonly IIncidentReportService _incidentReportService;

        public IncidentReportController(IIncidentReportService incidentReportService)
        {
            _incidentReportService = incidentReportService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateIncidentReport([FromBody] CreateIncidentReportRequest request, [FromQuery] string reporterId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (string.IsNullOrEmpty(reporterId))
                return BadRequest("Reporter ID is required");

            await _incidentReportService.CreateIncidentReport(request, reporterId);
            return Ok(new { message = "Incident report created successfully" });
        }

        [HttpPut("status")]
        public async Task<IActionResult> UpdateIncidentReportStatus([FromBody] UpdateIncidentReportStatusRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _incidentReportService.UpdateIncidentReportStatus(request);
            return Ok(new { message = "Incident report status updated successfully" });
        }

        [HttpGet("details")]
        public async Task<IActionResult> GetIncidentReportDetails([FromQuery] GetDetailsRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _incidentReportService.GetIncidentReportDetails(request);
            if (result == null)
                return NotFound(new { message = "Incident report not found" });

            return Ok(result);
        }

        [HttpGet("page")]
        public async Task<IActionResult> GetIncidentReportsPage(
            [FromQuery] PageRequest pageRequest,
            [FromQuery] GetIncidenReportListRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _incidentReportService.GetIncidentReportsPage(pageRequest, request);
            return Ok(result);
        }
    }
}
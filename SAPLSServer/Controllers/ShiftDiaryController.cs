using Microsoft.AspNetCore.Mvc;
using SAPLSServer.DTOs.Concrete.ShiftDiaryDto;
using SAPLSServer.DTOs.PaginationDto;
using SAPLSServer.Services.Interfaces;

namespace SAPLSServer.Controllers
{
    /// <summary>
    /// Controller for managing shift diary operations including creation and retrieval.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class ShiftDiaryController : ControllerBase
    {
        private readonly IShiftDiaryService _shiftDiaryService;

        public ShiftDiaryController(IShiftDiaryService shiftDiaryService)
        {
            _shiftDiaryService = shiftDiaryService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateShiftDiary([FromBody] CreateShiftDiaryRequest request, [FromQuery] string senderId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (string.IsNullOrEmpty(senderId))
                return BadRequest("Sender ID is required");

            await _shiftDiaryService.CreateShiftDiary(request, senderId);
            return Ok(new { message = "Shift diary created successfully" });
        }

        [HttpGet("details/{id}")]
        public async Task<IActionResult> GetShiftDiaryDetails(string id)
        {
            if (string.IsNullOrEmpty(id))
                return BadRequest("Id is required");

            var result = await _shiftDiaryService.GetShiftDiaryDetails(id);
            if (result == null)
                return NotFound(new { message = "Shift diary not found" });

            return Ok(result);
        }

        [HttpGet("page")]
        public async Task<IActionResult> GetShiftDiariesPage([FromQuery] PageRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _shiftDiaryService.GetShiftDiariesPage(request);
            return Ok(result);
        }

        [HttpGet("staff/{staffId}/page")]
        public async Task<IActionResult> GetShiftDiariesByStaffPage(string staffId, [FromQuery] PageRequest request)
        {
            if (string.IsNullOrEmpty(staffId))
                return BadRequest("Staff ID is required");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _shiftDiaryService.GetShiftDiariesByStaffPage(staffId, request);
            return Ok(result);
        }

        [HttpGet("parking-lot/{parkingLotId}/page")]
        public async Task<IActionResult> GetShiftDiariesByParkingLotPage(string parkingLotId, [FromQuery] PageRequest request)
        {
            if (string.IsNullOrEmpty(parkingLotId))
                return BadRequest("Parking lot ID is required");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _shiftDiaryService.GetShiftDiariesByParkingLotPage(parkingLotId, request);
            return Ok(result);
        }

        [HttpGet("date-range/page")]
        public async Task<IActionResult> GetShiftDiariesByDateRangePage(
            [FromQuery] DateTime startDate,
            [FromQuery] DateTime endDate,
            [FromQuery] PageRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _shiftDiaryService.GetShiftDiariesByDateRangePage(startDate, endDate, request);
            return Ok(result);
        }
    }
}
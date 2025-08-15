using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SAPLSServer.Constants;
using SAPLSServer.DTOs.Concrete.ShiftDiaryDtos;
using SAPLSServer.DTOs.PaginationDto;
using SAPLSServer.Services.Interfaces;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SAPLSServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Policy = Accessibility.PARKING_LOT_OWNER_OR_STAFF_ACCESS)]
    public class ShiftDiaryController : ControllerBase
    {
        private readonly IShiftDiaryService _shiftDiaryService;

        public ShiftDiaryController(IShiftDiaryService shiftDiaryService)
        {
            _shiftDiaryService = shiftDiaryService;
        }

        /// <summary>
        /// Creates a new shift diary entry (Staff only).
        /// </summary>
        [HttpPost]
        [Authorize(Policy = Accessibility.STAFF_ACCESS)]
        public async Task<IActionResult> Create([FromBody] CreateShiftDiaryRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var senderId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
            var result = await _shiftDiaryService.CreateAsync(request, senderId);
            return Ok(result);
        }

        /// <summary>
        /// Gets the details of a shift diary by ID.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDetails(string id)
        {
            var result = await _shiftDiaryService.GetDetailsAsync(id);
            if (result == null)
                return NotFound(new { message = MessageKeys.SHIFT_DIARY_NOT_FOUND });
            return Ok(result);
        }

        /// <summary>
        /// Gets a list of shift diaries.
        /// </summary>
        [HttpGet("list")]
        public async Task<IActionResult> GetList([FromQuery] GetShiftDiaryListRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _shiftDiaryService.GetListAsync(request);
            return Ok(result);
        }

        /// <summary>
        /// Gets a paginated list of shift diaries.
        /// </summary>
        [HttpGet("page")]
        public async Task<IActionResult> GetPage([FromQuery] PageRequest pageRequest, [FromQuery] GetShiftDiaryListRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _shiftDiaryService.GetPageAsync(pageRequest, request);
            return Ok(result);
        }
    }
}
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SAPLSServer.Constants;
using SAPLSServer.DTOs.Concrete.ParkingLotShiftDtos;
using SAPLSServer.Services.Interfaces;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SAPLSServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Policy = Accessibility.PARKING_LOT_OWNER_ACCESS)]
    public class ParkingLotShiftController : ControllerBase
    {
        private readonly IParkingLotShiftService _shiftService;

        public ParkingLotShiftController(IParkingLotShiftService shiftService)
        {
            _shiftService = shiftService;
        }

        /// <summary>
        /// Gets all shifts for a specific parking lot.
        /// </summary>
        [HttpGet("by-parking-lot/{parkingLotId}")]
        public async Task<IActionResult> GetByParkingLot(string parkingLotId)
        {
            var result = await _shiftService.GetShiftsByParkingLotAsync(parkingLotId);
            return Ok(result);
        }

        /// <summary>
        /// Gets the details of a shift by ID.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var result = await _shiftService.GetShiftByIdAsync(id);
            if (result == null)
                return NotFound(new { message = MessageKeys.SHIFT_NOT_FOUND });
            return Ok(result);
        }

        /// <summary>
        /// Creates a new parking lot shift.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateParkingLotShiftRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var performerId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
            var result = await _shiftService.CreateShiftAsync(request, performerId);
            return Ok(result);
        }

        /// <summary>
        /// Updates an existing parking lot shift.
        /// </summary>
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateParkingLotShiftRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var performerId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
            var success = await _shiftService.UpdateShiftAsync(request, performerId);
            if (!success)
                return NotFound(new { message = MessageKeys.SHIFT_NOT_FOUND });
            return Ok(new { message = MessageKeys.SHIFT_UPDATED_SUCCESSFULLY });
        }

        /// <summary>
        /// Deletes a parking lot shift.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var performerId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
            var success = await _shiftService.DeleteShiftAsync(id, performerId);
            if (!success)
                return NotFound(new { message = MessageKeys.SHIFT_NOT_FOUND });
            return Ok(new { message = MessageKeys.SHIFT_DELETED_SUCCESSFULLY });
        }
    }
}
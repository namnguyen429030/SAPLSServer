using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SAPLSServer.Constants;
using SAPLSServer.DTOs.Concrete.ParkingFeeScheduleDtos;
using SAPLSServer.Services.Interfaces;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SAPLSServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ParkingFeeScheduleController : ControllerBase
    {
        private readonly IParkingFeeScheduleService _service;

        public ParkingFeeScheduleController(IParkingFeeScheduleService service)
        {
            _service = service;
        }

        /// <summary>
        /// Creates a new parking fee schedule. (Parking Lot Owner only)
        /// </summary>
        [HttpPost]
        [Authorize(Policy = Accessibility.PARKING_LOT_OWNER_ACCESS)]
        public async Task<IActionResult> Create([FromBody] CreateParkingFeeScheduleRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var performerId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
            var result = await _service.CreateAsync(request, performerId);
            return Ok(result);
        }

        /// <summary>
        /// Updates an existing parking fee schedule. (Parking Lot Owner only)
        /// </summary>
        [HttpPut]
        [Authorize(Policy = Accessibility.PARKING_LOT_OWNER_ACCESS)]
        public async Task<IActionResult> Update([FromBody] UpdateParkingFeeScheduleRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var performerId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
            var result = await _service.UpdateAsync(request, performerId);
            return Ok(result);
        }

        /// <summary>
        /// Gets the list of parking fee schedules for a parking lot.
        /// </summary>
        [HttpGet("by-parking-lot/{parkingLotId}")]
        public async Task<IActionResult> GetListByParkingLot(string parkingLotId)
        {
            var performerId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
            var result = await _service.GetListByParkingLotAsync(parkingLotId, performerId);
            return Ok(result);
        }

        /// <summary>
        /// Gets the details of a parking fee schedule by its ID.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var performerId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
            var result = await _service.GetByIdAsync(id, performerId);
            if (result == null)
                return NotFound(new { message = MessageKeys.PARKING_FEE_SCHEDULE_NOT_FOUND });
            return Ok(result);
        }
    }
}
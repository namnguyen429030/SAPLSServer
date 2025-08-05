using Microsoft.AspNetCore.Mvc;
using SAPLSServer.DTOs.Base;
using SAPLSServer.DTOs.Concrete.UserDto;
using SAPLSServer.DTOs.PaginationDto;
using SAPLSServer.Services.Interfaces;

namespace SAPLSServer.Controllers
{
    /// <summary>
    /// Controller for managing parking lot owner profile operations including creation, updates, and retrieval.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class ParkingLotOwnerController : ControllerBase
    {
        private readonly IParkingLotOwnerService _parkingLotOwnerService;

        public ParkingLotOwnerController(IParkingLotOwnerService parkingLotOwnerService)
        {
            _parkingLotOwnerService = parkingLotOwnerService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateParkingLotOwner([FromBody] CreateParkingLotOwnerProfileRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _parkingLotOwnerService.CreateParkingLotOwner(request);
            return Ok(new { message = "Parking lot owner profile created successfully" });
        }

        [HttpPut]
        public async Task<IActionResult> UpdateParkingLotOwner([FromBody] UpdateParkingLotOwnerProfileRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _parkingLotOwnerService.UpdateParkingLotOwner(request);
            return Ok(new { message = "Parking lot owner profile updated successfully" });
        }

        [HttpGet("details")]
        public async Task<IActionResult> GetParkingLotOwnerProfileDetails([FromQuery] GetDetailsRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _parkingLotOwnerService.GetParkingLotOwnerProfileDetails(request);
            if (result == null)
                return NotFound(new { message = "Parking lot owner profile not found" });

            return Ok(result);
        }

        [HttpGet("page")]
        public async Task<IActionResult> GetParkingLotOwnerProfilesPage(
            [FromQuery] PageRequest pageRequest,
            [FromQuery] GetParkingLotOwnerListRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _parkingLotOwnerService.GetParkingLotOwnerProfilesPage(pageRequest, request);
            return Ok(result);
        }
    }
}
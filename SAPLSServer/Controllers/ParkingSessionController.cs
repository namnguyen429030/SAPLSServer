using Microsoft.AspNetCore.Mvc;
using SAPLSServer.DTOs.Base;
using SAPLSServer.DTOs.Concrete.ParkingSessionDto;
using SAPLSServer.DTOs.PaginationDto;
using SAPLSServer.Services.Interfaces;

namespace SAPLSServer.Controllers
{
    /// <summary>
    /// Controller for managing parking session operations including creation, updates, and retrieval.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class ParkingSessionController : ControllerBase
    {
        private readonly IParkingSessionService _parkingSessionService;

        public ParkingSessionController(IParkingSessionService parkingSessionService)
        {
            _parkingSessionService = parkingSessionService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateParkingSession([FromBody] CreateParkingSessionRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _parkingSessionService.CreateParkingSession(request);
            return Ok(new { message = "Parking session created successfully" });
        }

        [HttpPut("checkout")]
        public async Task<IActionResult> UpdateParkingSessionCheckOutDateTime([FromBody] UpdateParkingSessionCheckOutDateTimeRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _parkingSessionService.UpdateParkingSessionCheckOutDateTime(request);
            return Ok(new { message = "Parking session check-out time updated successfully" });
        }

        [HttpPut("exit")]
        public async Task<IActionResult> UpdateParkingSessionExit([FromBody] UpdateParkingSessionExitRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _parkingSessionService.UpdateParkingSessionExit(request);
            return Ok(new { message = "Parking session exit updated successfully" });
        }

        [HttpGet("client/details")]
        public async Task<IActionResult> GetParkingSessionDetailsForClient([FromQuery] GetDetailsRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _parkingSessionService.GetParkingSessionDetailsForClient(request);
            if (result == null)
                return NotFound(new { message = "Parking session not found" });

            return Ok(result);
        }

        [HttpGet("parking-lot/details")]
        public async Task<IActionResult> GetParkingSessionDetailsForParkingLot([FromQuery] GetDetailsRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _parkingSessionService.GetParkingSessionDetailsForParkingLot(request);
            if (result == null)
                return NotFound(new { message = "Parking session not found" });

            return Ok(result);
        }

        [HttpGet("client/page")]
        public async Task<IActionResult> GetParkingSessionsForClientPage(
            [FromQuery] PageRequest pageRequest,
            [FromQuery] GetParkingSessionListByClientIdRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _parkingSessionService.GetParkingSessionsForClientPage(pageRequest, request);
            return Ok(result);
        }

        [HttpGet("client")]
        public async Task<IActionResult> GetParkingSessionsForClient([FromQuery] GetParkingSessionListByClientIdRequest request) {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _parkingSessionService.GetParkingSessionsForClient(request);
            return Ok(result);
        }

        [HttpGet("parking-lot")]
        public async Task<IActionResult> GetParkingSessionsForParkingLotPage(
            [FromQuery] PageRequest pageRequest,
            [FromQuery] GetParkingSessionListByParkingLotIdRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _parkingSessionService.GetParkingSessionsForParkingLotPage(pageRequest, request);
            return Ok(result);
        }
    }
}
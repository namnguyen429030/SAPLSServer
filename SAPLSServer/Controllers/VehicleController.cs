using Microsoft.AspNetCore.Mvc;
using SAPLSServer.DTOs.Base;
using SAPLSServer.DTOs.Concrete.VehicleDto;
using SAPLSServer.DTOs.PaginationDto;
using SAPLSServer.Services.Interfaces;

namespace SAPLSServer.Controllers
{
    /// <summary>
    /// Controller for managing vehicle operations including creation, updates, retrieval, and deletion.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class VehicleController : ControllerBase
    {
        private readonly IVehicleService _vehicleService;

        public VehicleController(IVehicleService vehicleService)
        {
            _vehicleService = vehicleService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateVehicle([FromBody] CreateVehicleRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _vehicleService.CreateVehicle(request);
            return Ok(new { message = "Vehicle created successfully" });
        }

        [HttpPut]
        public async Task<IActionResult> UpdateVehicle([FromBody] UpdateVehicleRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _vehicleService.UpdateVehicle(request);
            return Ok(new { message = "Vehicle updated successfully" });
        }

        [HttpPut("status")]
        public async Task<IActionResult> UpdateVehicleStatus([FromBody] UpdateVehicleStatusRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _vehicleService.UpdateVehicleStatus(request);
            return Ok(new { message = "Vehicle status updated successfully" });
        }

        [HttpGet("details")]
        public async Task<IActionResult> GetVehicleDetails([FromQuery] GetDetailsRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _vehicleService.GetVehicleDetails(request);
            if (result == null)
                return NotFound(new { message = "Vehicle not found" });

            return Ok(result);
        }

        [HttpGet("page")]
        public async Task<IActionResult> GetVehiclesPage(
            [FromQuery] PageRequest pageRequest,
            [FromQuery] GetVehicleListRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _vehicleService.GetVehiclesPage(pageRequest, request);
            return Ok(result);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteVehicle([FromQuery] DeleteRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _vehicleService.DeleteVehicle(request);
            return Ok(new { message = "Vehicle deleted successfully" });
        }
    }
}
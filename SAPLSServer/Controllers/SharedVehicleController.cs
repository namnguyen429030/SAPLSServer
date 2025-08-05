using Microsoft.AspNetCore.Mvc;
using SAPLSServer.DTOs.Base;
using SAPLSServer.DTOs.Concrete.SharedVehicleDto;
using SAPLSServer.DTOs.PaginationDto;
using SAPLSServer.Services.Interfaces;

namespace SAPLSServer.Controllers
{
    /// <summary>
    /// Controller for managing shared vehicle operations including creation, retrieval, and status updates.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class SharedVehicleController : ControllerBase
    {
        private readonly ISharedVehicleService _sharedVehicleService;

        /// <summary>
        /// Initializes a new instance of the SharedVehicleController.
        /// </summary>
        /// <param name="sharedVehicleService">The shared vehicle service dependency.</param>
        public SharedVehicleController(ISharedVehicleService sharedVehicleService)
        {
            _sharedVehicleService = sharedVehicleService;
        }

        /// <summary>
        /// Creates a new shared vehicle.
        /// </summary>
        /// <param name="request">The request containing shared vehicle details.</param>
        /// <returns>Ok result if successful.</returns>
        [HttpPost]
        public async Task<IActionResult> CreateSharedVehicle([FromBody] CreateSharedVehicleRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _sharedVehicleService.CreateSharedVehicle(request);
            return Ok(new { message = "Shared vehicle created successfully" });
        }

        /// <summary>
        /// Retrieves the details of a specific shared vehicle.
        /// </summary>
        /// <param name="request">The request containing shared vehicle identifier.</param>
        /// <returns>The shared vehicle details or NotFound if not exists.</returns>
        [HttpGet("details")]
        public async Task<IActionResult> GetSharedVehicleDetails([FromQuery] GetDetailsRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _sharedVehicleService.GetSharedVehicleDetails(request);
            if (result == null)
                return NotFound(new { message = "Shared vehicle not found" });

            return Ok(result);
        }

        /// <summary>
        /// Retrieves a paginated list of shared vehicles.
        /// </summary>
        /// <param name="pageRequest">Pagination parameters.</param>
        /// <param name="request">Filter criteria for shared vehicles.</param>
        /// <returns>Paginated list of shared vehicles.</returns>
        [HttpGet]
        public async Task<IActionResult> GetSharedVehiclesPage(
            [FromQuery] PageRequest pageRequest,
            [FromQuery] GetSharedVehicleList request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _sharedVehicleService.GetSharedVehiclesPage(pageRequest, request);
            return Ok(result);
        }

        /// <summary>
        /// Updates the sharing status of a vehicle.
        /// </summary>
        /// <param name="request">The request containing status update details.</param>
        /// <returns>Ok result if successful.</returns>
        [HttpPut("sharing-status")]
        public async Task<IActionResult> UpdateVehicleSharingStatus([FromBody] UpdateVehicleSharingStatusRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _sharedVehicleService.UpdateVehicleSharingStatus(request);
            return Ok(new { message = "Vehicle sharing status updated successfully" });
        }
    }
}
using Microsoft.AspNetCore.Mvc;
using SAPLSServer.DTOs.Base;
using SAPLSServer.DTOs.Concrete.ParkingLotDto;
using SAPLSServer.DTOs.PaginationDto;
using SAPLSServer.Services.Interfaces;

namespace SAPLSServer.Controllers
{
    /// <summary>
    /// Controller for managing parking lot operations including creation, updates, retrieval, and deletion.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class ParkingLotController : ControllerBase
    {
        private readonly IParkingLotService _parkingLotService;

        /// <summary>
        /// Initializes a new instance of the ParkingLotController.
        /// </summary>
        /// <param name="parkingLotService">The parking lot service dependency.</param>
        public ParkingLotController(IParkingLotService parkingLotService)
        {
            _parkingLotService = parkingLotService;
        }

        /// <summary>
        /// Creates a new parking lot.
        /// </summary>
        /// <param name="request">The request containing parking lot details.</param>
        /// <returns>Ok result if successful.</returns>
        [HttpPost]
        public async Task<IActionResult> CreateParkingLot([FromBody] CreateParkingLotRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _parkingLotService.CreateParkingLot(request);
            return Ok(new { message = "Parking lot created successfully" });
        }

        /// <summary>
        /// Updates the basic information of a parking lot.
        /// </summary>
        /// <param name="request">The request containing updated parking lot details.</param>
        /// <returns>Ok result if successful.</returns>
        [HttpPut("basic-information")]
        public async Task<IActionResult> UpdateParkingLotBasicInformation([FromBody] UpdateParkingLotBasicInformationRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _parkingLotService.UpdateParkingLotBasicInformation(request);
            return Ok(new { message = "Parking lot basic information updated successfully" });
        }

        /// <summary>
        /// Updates the address of a parking lot.
        /// </summary>
        /// <param name="request">The request containing updated address details.</param>
        /// <returns>Ok result if successful.</returns>
        [HttpPut("address")]
        public async Task<IActionResult> UpdateParkingLotAddress([FromBody] UpdateParkingLotAddressRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _parkingLotService.UpdateParkingLotAddress(request);
            return Ok(new { message = "Parking lot address updated successfully" });
        }

        /// <summary>
        /// Retrieves the details of a specific parking lot.
        /// </summary>
        /// <param name="request">The request containing parking lot identifier.</param>
        /// <returns>The parking lot details or NotFound if not exists.</returns>
        [HttpGet("details")]
        public async Task<IActionResult> GetParkingLotDetails([FromQuery] GetDetailsRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _parkingLotService.GetParkingLotDetails(request);
            if (result == null)
                return NotFound(new { message = "Parking lot not found" });

            return Ok(result);
        }

        /// <summary>
        /// Retrieves a paginated list of parking lots.
        /// </summary>
        /// <param name="pageRequest">Pagination parameters.</param>
        /// <param name="request">Filter criteria for parking lots.</param>
        /// <returns>Paginated list of parking lots.</returns>
        [HttpGet("page")]
        public async Task<IActionResult> GetParkingLotsPage(
            [FromQuery] PageRequest pageRequest,
            [FromQuery] GetParkingLotListRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _parkingLotService.GetParkingLotsPage(pageRequest, request);
            return Ok(result);
        }

        /// <summary>
        /// Retrieves a list of parking lots without pagination.
        /// </summary>
        /// <param name="request">Filter criteria for parking lots.</param>
        /// <returns>List of parking lots.</returns>
        [HttpGet]
        public async Task<IActionResult> GetParkingLots([FromQuery] GetParkingLotListRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _parkingLotService.GetParkingLots(request);
            return Ok(result);
        }

        /// <summary>
        /// Deletes a parking lot.
        /// </summary>
        /// <param name="request">The request containing parking lot identifier.</param>
        /// <returns>Ok result if successful.</returns>
        [HttpDelete]
        public async Task<IActionResult> DeleteParkingLot([FromQuery] DeleteRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _parkingLotService.DeleteParkingLot(request);
            return Ok(new { message = "Parking lot deleted successfully" });
        }
    }}
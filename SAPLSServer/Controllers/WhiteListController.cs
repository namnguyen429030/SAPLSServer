using Microsoft.AspNetCore.Mvc;
using SAPLSServer.DTOs.Concrete;
using SAPLSServer.DTOs.PaginationDto;
using SAPLSServer.Services.Interfaces;

namespace SAPLSServer.Controllers
{
    /// <summary>
    /// Controller for managing whitelist operations including adding, updating, retrieving, and removing attendants.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class WhiteListController : ControllerBase
    {
        private readonly IWhitelistService _whitelistService;

        /// <summary>
        /// Initializes a new instance of the WhiteListController.
        /// </summary>
        /// <param name="whitelistService">The whitelist service dependency.</param>
        public WhiteListController(IWhitelistService whitelistService)
        {
            _whitelistService = whitelistService;
        }

        /// <summary>
        /// Adds an attendant to the whitelist.
        /// </summary>
        /// <param name="request">The request containing attendant details to add to whitelist.</param>
        /// <returns>Ok result if successful.</returns>
        [HttpPost("attendants")]
        public async Task<IActionResult> AddAttendantToWhitelist([FromBody] AddAttendantToWhiteListRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _whitelistService.AddAttendantToWhitelist(request);
            return Ok(new { message = "Attendant added to whitelist successfully" });
        }

        /// <summary>
        /// Updates the expiration date for a whitelist attendant.
        /// </summary>
        /// <param name="request">The request containing the updated expiration date details.</param>
        /// <returns>Ok result if successful.</returns>
        [HttpPut("attendants/expire-date")]
        public async Task<IActionResult> UpdateWhitelistAttendantExpireDate([FromBody] UpdateWhiteListAttendantExpireDateRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _whitelistService.UpdateWhitelistAttendantExpireDate(request);
            return Ok(new { message = "Attendant expiration date updated successfully" });
        }

        /// <summary>
        /// Retrieves the details of a specific whitelist attendant.
        /// </summary>
        /// <param name="request">The request containing parking lot and client identifiers.</param>
        /// <returns>The whitelist attendant details or NotFound if not exists.</returns>
        [HttpGet("attendants/details")]
        public async Task<IActionResult> GetWhitelistAttendantDetails([FromQuery] GetWhiteListAttendantRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _whitelistService.GetWhitelistAttendantDetails(request);
            if (result == null)
                return NotFound(new { message = "Whitelist attendant not found" });

            return Ok(result);
        }

        /// <summary>
        /// Retrieves a paginated list of whitelist attendants for a specific parking lot.
        /// </summary>
        /// <param name="pageRequest">Pagination parameters.</param>
        /// <param name="listRequest">Filter criteria for whitelist attendants.</param>
        /// <returns>Paginated list of whitelist attendants.</returns>
        [HttpGet("attendants")]
        public async Task<IActionResult> GetWhitelistAttendantsPage(
            [FromQuery] PageRequest pageRequest,
            [FromQuery] GetWhiteListAttendantListRequest listRequest)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _whitelistService.GetWhitelistAttendantsPage(pageRequest, listRequest);
            return Ok(result);
        }

        /// <summary>
        /// Removes an attendant from the whitelist.
        /// </summary>
        /// <param name="request">The request containing attendant details to remove from whitelist.</param>
        /// <returns>Ok result if successful.</returns>
        [HttpDelete("attendants")]
        public async Task<IActionResult> RemoveAttendantFromWhitelist([FromBody] RemoveAttendantFromWhiteListRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _whitelistService.RemoveAttendantFromWhitelist(request);
            return Ok(new { message = "Attendant removed from whitelist successfully" });
        }
    }
}
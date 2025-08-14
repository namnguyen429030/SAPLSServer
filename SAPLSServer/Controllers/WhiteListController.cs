using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SAPLSServer.Constants;
using SAPLSServer.DTOs.Concrete.WhiteListDtos;
using SAPLSServer.DTOs.PaginationDto;
using SAPLSServer.Exceptions;
using SAPLSServer.Services.Interfaces;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SAPLSServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class WhiteListController : ControllerBase
    {
        private readonly IWhiteListService _whiteListService;
        public WhiteListController(IWhiteListService whiteListService, IParkingLotService parkingLotService)
        {
            _whiteListService = whiteListService;
        }

        /// <summary>
        /// Checks if a client is whitelisted in a parking lot.
        /// </summary>
        [HttpGet("check")]
        public async Task<IActionResult> IsClientWhitelisted([FromQuery] string parkingLotId, [FromQuery] string clientId)
        {
            var result = await _whiteListService.IsClientWhitelistedAsync(parkingLotId, clientId);
            return Ok(new { isWhitelisted = result });
        }

        /// <summary>
        /// Adds a client to the whitelist.
        /// </summary>
        [HttpPost]
        [Authorize(Policy = Accessibility.PARKING_LOT_OWNER_ACCESS)]
        public async Task<IActionResult> AddToWhiteList([FromBody] AddAttendantToWhiteListRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;
                await _whiteListService.AddToWhiteListAsync(request, currentUserId);
                return Ok(new { message = MessageKeys.CLIENT_ADDED_TO_WHITE_LIST });
            }
            catch (InvalidInformationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Updates the expiration date of a whitelist entry.
        /// </summary>
        [HttpPut("expire-date")]
        [Authorize(Policy = Accessibility.PARKING_LOT_OWNER_ACCESS)]
        public async Task<IActionResult> UpdateExpireAt([FromBody] UpdateWhiteListAttendantExpireDateRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;
                await _whiteListService.UpdateExpireAtAsync(request, currentUserId);
                return Ok(new { message = MessageKeys.WHITE_LIST_EXPIRE_DATE_UPDATED });
            }
            catch (InvalidInformationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Removes a client from the whitelist.
        /// </summary>
        [HttpDelete]
        [Authorize(Policy = Accessibility.PARKING_LOT_OWNER_ACCESS)]
        public async Task<IActionResult> RemoveFromWhiteList([FromBody] RemoveAttendantFromWhiteListRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;
                await _whiteListService.RemoveFromWhiteListAsync(request, currentUserId);
                return Ok(new { message = MessageKeys.CLIENT_REMOVED_FROM_WHITE_LIST });
            }
            catch (InvalidInformationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Gets a list of whitelisted attendants for a parking lot.
        /// </summary>
        [HttpGet]
        [Authorize(Policy = Accessibility.PARKING_LOT_OWNER_ACCESS)]
        public async Task<IActionResult> GetWhiteList([FromQuery] GetWhiteListAttendantListRequest request)
        {
            var result = await _whiteListService.GetWhiteListAsync(request);
            return Ok(result);
        }

        /// <summary>
        /// Gets a paginated list of whitelisted attendants for a parking lot.
        /// </summary>
        [HttpGet("page")]
        public async Task<IActionResult> GetWhiteListPage([FromQuery] PageRequest pageRequest, [FromQuery] GetWhiteListAttendantListRequest request)
        {
            var result = await _whiteListService.GetWhiteListPageAsync(pageRequest, request);
            return Ok(result);
        }
    }
}
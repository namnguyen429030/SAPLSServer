using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SAPLSServer.Constants;
using SAPLSServer.DTOs.Base;
using SAPLSServer.DTOs.Concrete;
using SAPLSServer.Services.Interfaces;

namespace SAPLSServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ParkingLotOwnersController : ControllerBase
    {
        private readonly IParkingLotOwnerService _parkingLotOwnerService;

        public ParkingLotOwnersController(IParkingLotOwnerService parkingLotOwnerService)
        {
            _parkingLotOwnerService = parkingLotOwnerService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateParkingLotOwnerProfileRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _parkingLotOwnerService.CreateParkingLotOwner(request);
            return Ok(new { message = MessageKeys.PARKING_LOT_OWNER_PROFILE_CREATED_SUCCESSFULLY });
        }

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> Update([FromBody] UpdateParkingLotOwnerProfileRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _parkingLotOwnerService.UpdateParkingLotOwner(request);
            return Ok(new { message = MessageKeys.PARKING_LOT_OWNER_PROFILE_UPDATED_SUCCESSFULLY });
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetDetails(string id)
        {
            var dto = await _parkingLotOwnerService.GetParkingLotOwnerProfileDetails(new GetDetailsRequest { Id = id });
            if (dto == null)
                return NotFound();
            return Ok(new
            {
                data = dto,
                message = MessageKeys.GET_PARKING_LOT_OWNER_PROFILE_DETAILS_SUCCESSFULLY
            });
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetPage([FromQuery] PageRequest pageRequest, [FromQuery] GetListRequest request)
        {
            var result = await _parkingLotOwnerService.GetParkingLotOwnersPage(pageRequest, request);
            return Ok(new
            {
                data = result,
                message = MessageKeys.GET_PARKING_LOT_OWNER_PROFILES_PAGE_SUCCESSFULLY
            });
        }
    }
}
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SAPLSServer.Constants;
using SAPLSServer.DTOs.Concrete;
using SAPLSServer.Services.Interfaces;

namespace SAPLSServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StaffsController : ControllerBase
    {
        private readonly IStaffService _staffService;

        public StaffsController(IStaffService staffService)
        {
            _staffService = staffService;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create([FromBody] CreateStaffProfileRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _staffService.CreateStaff(request);
            return Ok(new { message = MessageKeys.STAFF_PROFILE_CREATED_SUCCESSFULLY });
        }

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> Update([FromBody] UpdateStaffProfileRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _staffService.UpdateStaff(request);
            return Ok(new { message = MessageKeys.STAFF_PROFILE_UPDATED_SUCCESSFULLY });
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetDetails(string id)
        {
            var dto = await _staffService.GetStaffProfileDetails(id);
            if (dto == null)
                return NotFound();
            return Ok(new
            {
                data = dto,
                message = MessageKeys.GET_STAFF_PROFILE_DETAILS_SUCCESSFULLY
            });
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetPage([FromQuery] PageRequest pageRequest)
        {
            var result = await _staffService.GetStaffsPageByParkingLot(pageRequest);
            return Ok(new
            {
                data = result,
                message = MessageKeys.GET_STAFF_PROFILES_PAGE_SUCCESSFULLY
            });
        }
    }
}
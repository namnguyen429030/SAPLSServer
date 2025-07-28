using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SAPLSServer.Constants;
using SAPLSServer.DTOs.Base;
using SAPLSServer.DTOs.Concrete;
using SAPLSServer.Exceptions;
using SAPLSServer.Services.Interfaces;

namespace SAPLSServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdminsController : ControllerBase
    {
        private readonly IAdminService _adminService;

        public AdminsController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create([FromBody] CreateAdminProfileRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                await _adminService.CreateAdmin(request);
                return Ok(new { message = MessageKeys.ADMIN_PROFILE_CREATED_SUCCESSFULLY });
            }
            catch(InvalidInformationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateAdminProfileRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _adminService.UpdateAdmin(request);
            return Ok(new { message = MessageKeys.ADMIN_PROFILE_UPDATED_SUCCESSFULLY });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDetails(string id)
        {
            var dto = await _adminService.GetAdminProfileDetails(new GetDetailsRequest { Id = id });
            if (dto == null)
                return NotFound();
            return Ok(new
            {
                data = dto,
                message = MessageKeys.GET_ADMIN_PROFILE_DETAILS_SUCCESSFULLY
            });
        }

        [HttpGet]
        public async Task<IActionResult> GetPage([FromQuery] PageRequest pageRequest, [FromQuery] GetListRequest request)
        {
            var result = await _adminService.GetAdminProfilesPage(pageRequest, request);
            return Ok(new
            {
                data = result,
                message = MessageKeys.GET_ADMIN_PROFILES_PAGE_SUCCESSFULLY
            });
        }
    }
}
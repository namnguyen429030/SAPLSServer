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
    //TODO: Add Citizen card front image and back image to Client model
    public class ClientsController : ControllerBase
    {
        private readonly IClientService _clientService;

        public ClientsController(IClientService clientService)
        {
            _clientService = clientService;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Create([FromBody] CreateClientProfileRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _clientService.CreateClient(request);
            return Ok(new { message = MessageKeys.CLIENT_PROFILE_CREATED_SUCCESSFULLY });
        }

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> Update([FromBody] UpdateClientProfileRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _clientService.UpdateClient(request);
            return Ok(new { message = MessageKeys.CLIENT_PROFILE_UPDATED_SUCCESSFULLY });
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetDetails(string id)
        {
            var dto = await _clientService.GetClientProfileDetails(new GetDetailsRequest { Id = id });
            if (dto == null)
                return NotFound();
            return Ok(new
            {
                data = dto,
                message = MessageKeys.GET_CLIENT_PROFILE_DETAILS_SUCCESSFULLY
            });
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetPage([FromQuery] PageRequest pageRequest, [FromQuery] GetListRequest request)
        {
            var result = await _clientService.GetClientProfilesPage(pageRequest, request);
            return Ok(new
            {
                data = result,
                message = MessageKeys.GET_CLIENT_PROFILES_PAGE_SUCCESSFULLY
            });
        }
    }
}
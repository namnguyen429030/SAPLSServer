using Microsoft.AspNetCore.Mvc;
using SAPLSServer.DTOs.Base;
using SAPLSServer.DTOs.Concrete.UserDto;
using SAPLSServer.DTOs.PaginationDto;
using SAPLSServer.Services.Interfaces;

namespace SAPLSServer.Controllers
{
    /// <summary>
    /// Controller for managing client profile operations including creation, updates, and retrieval.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class ClientController : ControllerBase
    {
        private readonly IClientService _clientService;

        public ClientController(IClientService clientService)
        {
            _clientService = clientService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateClient([FromBody] CreateClientProfileRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _clientService.CreateClient(request);
            return Ok(new { message = "Client profile created successfully" });
        }

        [HttpPut]
        public async Task<IActionResult> UpdateClient([FromBody] UpdateClientProfileRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _clientService.UpdateClient(request);
            return Ok(new { message = "Client profile updated successfully" });
        }

        [HttpGet("details")]
        public async Task<IActionResult> GetClientProfileDetails([FromQuery] GetDetailsRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _clientService.GetClientProfileDetails(request);
            if (result == null)
                return NotFound(new { message = "Client profile not found" });

            return Ok(result);
        }

        [HttpGet("page")]
        public async Task<IActionResult> GetClientProfilesPage(
            [FromQuery] PageRequest pageRequest,
            [FromQuery] GetClientListRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _clientService.GetClientProfilesPage(pageRequest, request);
            return Ok(result);
        }

        /// <summary>
        /// Retrieves all client profiles with optional filtering.
        /// </summary>
        /// <param name="request">The filter criteria for client profiles.</param>
        /// <returns>List of all client profiles.</returns>
        [HttpGet("all")]
        public async Task<IActionResult> GetAllClients([FromQuery] GetClientListRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _clientService.GetAllClients(request);
            return Ok(result);
        }
    }
}
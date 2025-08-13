using Microsoft.AspNetCore.Mvc;
using SAPLSServer.DTOs.Concrete.OcrDtos;
using SAPLSServer.Exceptions;
using SAPLSServer.Services.Interfaces;

namespace SAPLSServer.Controllers
{
    [ApiController]
    [Route("api/ocr/vehicle-registration")]
    public class VehicleRegistrationCertOcrController : ControllerBase
    {
        private readonly IVehicleRegistrationCertOcrService _ocrService;

        public VehicleRegistrationCertOcrController(IVehicleRegistrationCertOcrService ocrService)
        {
            _ocrService = ocrService;
        }

        [HttpPost("base64")]
        public async Task<ActionResult<VehicleRegistrationOcrResponse>> ExtractFromBase64([FromBody] VehicleRegistrationOcrRequest request)
        {
            try
            {
                var result = await _ocrService.AttractDataFromBase64(request);
                return Ok(result);
            }
            catch(InvalidInformationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (EmptyConfigurationValueException ex)
            {
                return StatusCode(503, new
                {
                    ErrorMessage = ex.Message,
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    ErrorMessage = ex.Message
                });
            }
        }

        [HttpPost("file")]
        [RequestSizeLimit(10_000_000)] // 10MB limit, adjust as needed
        public async Task<ActionResult<VehicleRegistrationOcrResponse>> ExtractFromFile([FromForm] VehicleRegistrationOcrFileRequest request)
        {
            try
            {
                var result = await _ocrService.AttractDataFromFile(request);
                return Ok(result);
            }
            catch (InvalidInformationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (EmptyConfigurationValueException ex)
            {
                return StatusCode(503, new
                {
                    ErrorMessage = ex.Message,
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    ErrorMessage = ex.Message
                });
            }
        }
    }
}
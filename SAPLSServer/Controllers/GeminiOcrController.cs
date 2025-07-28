using Microsoft.AspNetCore.Mvc;
using SAPLSServer.DTOs.Concrete;
using SAPLSServer.Services.Interfaces;
using System.IO;

namespace SAPLSServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GeminiOcrController : ControllerBase
    {
        private readonly IGeminiOcrService _ocrService;

        public GeminiOcrController(IGeminiOcrService ocrService)
        {
            _ocrService = ocrService;
        }

        // Existing JSON DTO endpoint
        [HttpPost("citizen-id")]
        public async Task<ActionResult<CitizenIdOcrResponse>> ExtractCitizenId([FromBody] CitizenIdOcrRequest request)
        {
            try
            {
                var result = await _ocrService.ExtractCitizenIdDataAsync(request);
                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return UnprocessableEntity(new { error = ex.Message });
            }
        }

        // New file DTO endpoint
        [HttpPost("citizen-id/file")]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult<CitizenIdOcrResponse>> ExtractCitizenIdFromFile([FromForm] CitizenIdOcrFileRequest fileRequest)
        {
            var request = new CitizenIdOcrRequest
            {
                ImageFormat = fileRequest.ImageFormat,
                Language = fileRequest.Language,
                EnhanceAccuracy = fileRequest.EnhanceAccuracy
            };

            using (var ms = new MemoryStream())
            {
                await fileRequest.FrontImage.CopyToAsync(ms);
                request.FrontImageBase64 = Convert.ToBase64String(ms.ToArray());
            }
            using (var ms = new MemoryStream())
            {
                await fileRequest.BackImage.CopyToAsync(ms);
                request.BackImageBase64 = Convert.ToBase64String(ms.ToArray());
            }

            try
            {
                var result = await _ocrService.ExtractCitizenIdDataAsync(request);
                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return UnprocessableEntity(new { error = ex.Message });
            }
        }

        // Existing JSON DTO endpoint
        [HttpPost("vehicle-registration")]
        public async Task<ActionResult<VehicleRegistrationOcrResponse>> ExtractVehicleRegistration([FromBody] VehicleRegistrationOcrRequest request)
        {
            try
            {
                var result = await _ocrService.ExtractVehicleRegistrationDataAsync(request);
                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return UnprocessableEntity(new { error = ex.Message });
            }
        }

        // New file DTO endpoint
        [HttpPost("vehicle-registration/file")]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult<VehicleRegistrationOcrResponse>> ExtractVehicleRegistrationFromFile([FromForm] VehicleRegistrationOcrFileRequest fileRequest)
        {
            var request = new VehicleRegistrationOcrRequest
            {
                ImageFormat = fileRequest.ImageFormat,
                Language = fileRequest.Language,
                EnhanceAccuracy = fileRequest.EnhanceAccuracy
            };

            using (var ms = new MemoryStream())
            {
                await fileRequest.FrontImage.CopyToAsync(ms);
                request.FrontImageBase64 = Convert.ToBase64String(ms.ToArray());
            }
            using (var ms = new MemoryStream())
            {
                await fileRequest.BackImage.CopyToAsync(ms);
                request.BackImageBase64 = Convert.ToBase64String(ms.ToArray());
            }

            try
            {
                var result = await _ocrService.ExtractVehicleRegistrationDataAsync(request);
                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return UnprocessableEntity(new { error = ex.Message });
            }
        }
    }
}


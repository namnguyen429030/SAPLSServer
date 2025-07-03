using Microsoft.AspNetCore.Mvc;
using SAPLSServer.DTOs.Concrete.GeminiOcr;
using SAPLSServer.Services.Interfaces;

namespace SAPLSServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GeminiOcrController : ControllerBase
    {
        private readonly IGeminiOcrService _geminiOcrService;
        private readonly ILogger<GeminiOcrController> _logger;

        public GeminiOcrController(
            IGeminiOcrService geminiOcrService,
            ILogger<GeminiOcrController> logger)
        {
            _geminiOcrService = geminiOcrService;
            _logger = logger;
        }

        /// <summary>
        /// Extract data from Citizen ID Card image using Gemini Vision OCR
        /// </summary>
        /// <param name="dto">Citizen ID Card OCR request</param>
        /// <returns>Extracted citizen information</returns>
        [HttpPost("citizen-id/extract")]
        public async Task<ActionResult<CitizenIdOcrResponse>> ExtractCitizenIdData([FromBody] CitizenIdOcrRequest dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                _logger.LogInformation("Processing Citizen ID OCR extraction request");
                
                var result = await _geminiOcrService.ExtractCitizenIdDataAsync(dto);
                
                _logger.LogInformation("Citizen ID OCR extraction completed successfully with confidence: {Confidence}", 
                    result.ConfidenceScore);
                
                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "Invalid operation during Citizen ID OCR extraction");
                return BadRequest(new { error = ex.Message });
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "HTTP request failed during Citizen ID OCR extraction");
                return StatusCode(502, new { error = "External service unavailable", details = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error during Citizen ID OCR extraction");
                return StatusCode(500, new { error = "Internal server error occurred while processing the request" });
            }
        }

        /// <summary>
        /// Extract data from Vehicle Registration Certificate image using Gemini Vision OCR
        /// </summary>
        /// <param name="dto">Vehicle Registration OCR request</param>
        /// <returns>Extracted vehicle registration information</returns>
        [HttpPost("vehicle-registration/extract")]
        public async Task<ActionResult<VehicleRegistrationOcrResponse>> ExtractVehicleRegistrationData([FromBody] VehicleRegistrationOcrRequest dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                _logger.LogInformation("Processing Vehicle Registration OCR extraction request");
                
                var result = await _geminiOcrService.ExtractVehicleRegistrationDataAsync(dto);
                
                _logger.LogInformation("Vehicle Registration OCR extraction completed successfully with confidence: {Confidence}", 
                    result.ConfidenceScore);
                
                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "Invalid operation during Vehicle Registration OCR extraction");
                return BadRequest(new { error = ex.Message });
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "HTTP request failed during Vehicle Registration OCR extraction");
                return StatusCode(502, new { error = "External service unavailable", details = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error during Vehicle Registration OCR extraction");
                return StatusCode(500, new { error = "Internal server error occurred while processing the request" });
            }
        }

        /// <summary>
        /// Validate and correct OCR extracted data using AI
        /// </summary>
        /// <param name="dto">OCR validation request</param>
        /// <returns>Validated and corrected data</returns>
        [HttpPost("validate")]
        public async Task<ActionResult<OcrValidationResponse>> ValidateOcrData([FromBody] OcrValidationRequest dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                _logger.LogInformation("Processing OCR data validation for document type: {DocumentType}", dto.DocumentType);
                
                var result = await _geminiOcrService.ValidateOcrDataAsync(dto);
                
                _logger.LogInformation("OCR data validation completed. Valid: {IsValid}, Confidence: {Confidence}", 
                    result.IsValid, result.OverallConfidence);
                
                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "Invalid operation during OCR data validation");
                return BadRequest(new { error = ex.Message });
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "HTTP request failed during OCR data validation");
                return StatusCode(502, new { error = "External service unavailable", details = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error during OCR data validation");
                return StatusCode(500, new { error = "Internal server error occurred while processing the request" });
            }
        }

        /// <summary>
        /// Extract data from multiple document images in batch
        /// </summary>
        /// <param name="dto">Batch OCR request</param>
        /// <returns>Batch extraction results</returns>
        [HttpPost("batch/extract")]
        public async Task<ActionResult<BatchOcrResponse>> ExtractBatchDocuments([FromBody] BatchOcrRequest dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (dto.Documents == null || !dto.Documents.Any())
                {
                    return BadRequest(new { error = "At least one document is required for batch processing" });
                }

                if (dto.Documents.Count > 10) // Limit batch size to prevent abuse
                {
                    return BadRequest(new { error = "Maximum batch size is 10 documents" });
                }

                _logger.LogInformation("Processing batch OCR extraction for {DocumentCount} documents", dto.Documents.Count);
                
                var result = await _geminiOcrService.ExtractBatchDocumentsAsync(dto);
                
                _logger.LogInformation("Batch OCR extraction completed. Total: {Total}, Success: {Success}, Failed: {Failed}", 
                    result.TotalProcessed, result.SuccessfulExtractions, result.FailedExtractions);
                
                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "Invalid operation during batch OCR extraction");
                return BadRequest(new { error = ex.Message });
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "HTTP request failed during batch OCR extraction");
                return StatusCode(502, new { error = "External service unavailable", details = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error during batch OCR extraction");
                return StatusCode(500, new { error = "Internal server error occurred while processing the request" });
            }
        }

        /// <summary>
        /// Check OCR service health and available models
        /// </summary>
        /// <returns>OCR service status</returns>
        [HttpGet("health")]
        public async Task<ActionResult<OcrServiceHealthDto>> CheckOcrServiceHealth()
        {
            try
            {
                _logger.LogInformation("Checking Gemini OCR service health");
                
                var result = await _geminiOcrService.CheckOcrServiceHealthAsync();
                
                _logger.LogInformation("Gemini OCR service health check completed. Status: {Status}", result.ServiceStatus);
                
                if (result.IsHealthy)
                {
                    return Ok(result);
                }
                else
                {
                    return StatusCode(503, result); // Service Unavailable
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during Gemini OCR service health check");
                return StatusCode(500, new { 
                    error = "Health check failed", 
                    details = ex.Message,
                    timestamp = DateTime.UtcNow 
                });
            }
        }

        /// <summary>
        /// Get supported document types and formats
        /// </summary>
        /// <returns>Information about supported document types and image formats</returns>
        [HttpGet("supported-formats")]
        public ActionResult<object> GetSupportedFormats()
        {
            try
            {
                var supportedFormats = new
                {
                    DocumentTypes = new[]
                    {
                        new { Type = "CitizenId", Description = "Vietnamese Citizen ID Card" },
                        new { Type = "VehicleRegistration", Description = "Vietnamese Vehicle Registration Certificate" }
                    },
                    ImageFormats = new[] { "jpeg", "jpg", "png", "webp" },
                    Languages = new[]
                    {
                        new { Code = "vi", Name = "Vietnamese" },
                        new { Code = "en", Name = "English" }
                    },
                    Limits = new
                    {
                        MaxBatchSize = 10,
                        MaxImageSizeMB = 10,
                        TimeoutMinutes = 2
                    }
                };

                return Ok(supportedFormats);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving supported formats information");
                return StatusCode(500, new { error = "Failed to retrieve supported formats" });
            }
        }

        /// <summary>
        /// Upload and extract data from document image file
        /// </summary>
        /// <param name="file">Document image file</param>
        /// <param name="documentType">Type of document (CitizenId or VehicleRegistration)</param>
        /// <param name="language">Language for OCR processing (default: vi)</param>
        /// <param name="enhanceAccuracy">Whether to enhance accuracy (default: true)</param>
        /// <returns>Extracted document data</returns>
        [HttpPost("upload/extract")]
        public async Task<ActionResult<object>> ExtractFromUploadedFile(
            IFormFile file,
            [FromForm] string documentType = "CitizenId",
            [FromForm] string language = "vi",
            [FromForm] bool enhanceAccuracy = true)
        {
            try
            {
                if (file == null || file.Length == 0)
                {
                    return BadRequest(new { error = "No file uploaded" });
                }

                // Validate file size (10MB limit)
                if (file.Length > 10 * 1024 * 1024)
                {
                    return BadRequest(new { error = "File size exceeds 10MB limit" });
                }

                // Validate file type
                var allowedTypes = new[] { "image/jpeg", "image/jpg", "image/png", "image/webp" };
                if (!allowedTypes.Contains(file.ContentType.ToLower()))
                {
                    return BadRequest(new { error = "Unsupported file type. Only JPEG, PNG, and WebP are allowed" });
                }

                // Convert file to base64
                using var memoryStream = new MemoryStream();
                await file.CopyToAsync(memoryStream);
                var imageBase64 = Convert.ToBase64String(memoryStream.ToArray());
                var imageFormat = file.ContentType.Split('/')[1]; // Extract format from MIME type

                _logger.LogInformation("Processing uploaded file OCR extraction. DocumentType: {DocumentType}, Size: {Size}KB", 
                    documentType, file.Length / 1024);

                // Process based on document type
                if (documentType.Equals("CitizenId", StringComparison.OrdinalIgnoreCase))
                {
                    var citizenRequest = new CitizenIdOcrRequest
                    {
                        ImageBase64 = imageBase64,
                        ImageFormat = imageFormat,
                        Language = language,
                        EnhanceAccuracy = enhanceAccuracy
                    };

                    var result = await _geminiOcrService.ExtractCitizenIdDataAsync(citizenRequest);
                    return Ok(result);
                }
                else if (documentType.Equals("VehicleRegistration", StringComparison.OrdinalIgnoreCase))
                {
                    var vehicleRequest = new VehicleRegistrationOcrRequest
                    {
                        ImageBase64 = imageBase64,
                        ImageFormat = imageFormat,
                        Language = language,
                        EnhanceAccuracy = enhanceAccuracy
                    };

                    var result = await _geminiOcrService.ExtractVehicleRegistrationDataAsync(vehicleRequest);
                    return Ok(result);
                }
                else
                {
                    return BadRequest(new { error = $"Unsupported document type: {documentType}" });
                }
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "Invalid operation during uploaded file OCR extraction");
                return BadRequest(new { error = ex.Message });
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "HTTP request failed during uploaded file OCR extraction");
                return StatusCode(502, new { error = "External service unavailable", details = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error during uploaded file OCR extraction");
                return StatusCode(500, new { error = "Internal server error occurred while processing the uploaded file" });
            }
        }
    }
}


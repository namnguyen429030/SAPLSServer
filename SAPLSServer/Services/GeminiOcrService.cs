using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SAPLSServer.DTOs.Concrete.GeminiOcr;
using SAPLSServer.Services.Interfaces;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace SAPLSServer.Services.Implementations
{
    public class GeminiOcrService : IGeminiOcrService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly ILogger<GeminiOcrService> _logger;
        private readonly string _apiKey;
        private readonly string _baseUrl;

        public GeminiOcrService(
            IHttpClientFactory httpClientFactory,
            IConfiguration configuration,
            ILogger<GeminiOcrService> logger)
        {
            _httpClient = httpClientFactory.CreateClient();
            _configuration = configuration;
            _logger = logger;
            // Try to get API key from Key Vault first, then fall back to appsettings
            _apiKey = _configuration["GeminiApi-ApiKey"] // Key Vault secret name
                     ?? _configuration["GeminiApi:ApiKey"] // appsettings fallback
                     ?? throw new InvalidOperationException("Gemini API key is not configured");

            _baseUrl = _configuration["GeminiApi:BaseUrl"]
                      ?? "https://generativelanguage.googleapis.com/v1beta";

            ConfigureHttpClient();
        }

        private void ConfigureHttpClient()
        {
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _httpClient.Timeout = TimeSpan.FromMinutes(2); // Set timeout for image processing
        }

        public async Task<CitizenIdOcrResponse> ExtractCitizenIdDataAsync(CitizenIdOcrRequest dto)
        {
            var stopwatch = Stopwatch.StartNew();
            
            try
            {
                _logger.LogInformation("Starting Citizen ID OCR extraction");

                var prompt = CreateCitizenIdPrompt(dto.Language, dto.EnhanceAccuracy);
                var geminiRequest = CreateGeminiVisionRequest(prompt, dto.ImageBase64, dto.ImageFormat);
                
                var response = await SendGeminiRequestAsync(geminiRequest);
                var extractedData = ParseCitizenIdResponse(response);
                
                stopwatch.Stop();
                
                var result = new CitizenIdOcrResponse
                {
                    Id = Guid.NewGuid().ToString(),
                    CitizenId = extractedData.GetValueOrDefault("citizenId", ""),
                    FullName = extractedData.GetValueOrDefault("fullName", ""),
                    DateOfBirth = DateTime.TryParse(extractedData.GetValueOrDefault("dateOfBirth"), out var dob) ? dob : DateTime.MinValue,
                    Sex = extractedData.GetValueOrDefault("sex", ""),
                    Nationality = extractedData.GetValueOrDefault("nationality", ""),
                    PlaceOfOrigin = extractedData.GetValueOrDefault("placeOfOrigin", ""),
                    PlaceOfResidence = extractedData.GetValueOrDefault("placeOfResidence", ""),
                    ExpiryDate = DateTime.TryParse(extractedData.GetValueOrDefault("expiryDate"), out var exp) ? exp : null,
                    ConfidenceScore = CalculateConfidenceScore(extractedData),
                    FieldConfidences = CreateFieldConfidences(extractedData),
                    ProcessedAt = DateTime.UtcNow,
                    ProcessingTimeMs = stopwatch.ElapsedMilliseconds.ToString()
                };

                _logger.LogInformation("Citizen ID OCR extraction completed successfully in {ElapsedMs}ms", stopwatch.ElapsedMilliseconds);
                return result;
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                _logger.LogError(ex, "Error during Citizen ID OCR extraction");
                throw new InvalidOperationException($"Failed to extract Citizen ID data: {ex.Message}", ex);
            }
        }

        public async Task<VehicleRegistrationOcrResponse> ExtractVehicleRegistrationDataAsync(VehicleRegistrationOcrRequest dto)
        {
            var stopwatch = Stopwatch.StartNew();
            
            try
            {
                _logger.LogInformation("Starting Vehicle Registration OCR extraction");

                var prompt = CreateVehicleRegistrationPrompt(dto.Language, dto.EnhanceAccuracy);
                var geminiRequest = CreateGeminiVisionRequest(prompt, dto.ImageBase64, dto.ImageFormat);
                
                var response = await SendGeminiRequestAsync(geminiRequest);
                var extractedData = ParseVehicleRegistrationResponse(response);
                
                stopwatch.Stop();
                
                var result = new VehicleRegistrationOcrResponse
                {
                    Id = Guid.NewGuid().ToString(),
                    LicensePlate = extractedData.GetValueOrDefault("licensePlate", ""),
                    Brand = extractedData.GetValueOrDefault("brand", ""),
                    Model = extractedData.GetValueOrDefault("model", ""),
                    EngineNumber = extractedData.GetValueOrDefault("engineNumber", ""),
                    ChassisNumber = extractedData.GetValueOrDefault("chassisNumber", ""),
                    Color = extractedData.GetValueOrDefault("color", ""),
                    OwnerVehicleFullName = extractedData.GetValueOrDefault("ownerFullName", ""),
                    RegistrationDate = DateTime.TryParse(extractedData.GetValueOrDefault("registrationDate"), out var regDate) ? regDate : null,
                    VehicleType = extractedData.GetValueOrDefault("vehicleType", ""),
                    PlateType = extractedData.GetValueOrDefault("plateType"),
                    ConfidenceScore = CalculateConfidenceScore(extractedData),
                    FieldConfidences = CreateFieldConfidences(extractedData),
                    ProcessedAt = DateTime.UtcNow,
                    ProcessingTimeMs = stopwatch.ElapsedMilliseconds.ToString()
                };

                _logger.LogInformation("Vehicle Registration OCR extraction completed successfully in {ElapsedMs}ms", stopwatch.ElapsedMilliseconds);
                return result;
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                _logger.LogError(ex, "Error during Vehicle Registration OCR extraction");
                throw new InvalidOperationException($"Failed to extract Vehicle Registration data: {ex.Message}", ex);
            }
        }

        public async Task<OcrValidationResponse> ValidateOcrDataAsync(OcrValidationRequest dto)
        {
            try
            {
                _logger.LogInformation("Starting OCR data validation for document type: {DocumentType}", dto.DocumentType);

                var prompt = CreateValidationPrompt(dto.DocumentType, dto.ExtractedData);
                var geminiRequest = CreateGeminiTextRequest(prompt);
                
                var response = await SendGeminiRequestAsync(geminiRequest);
                var validationResult = ParseValidationResponse(response);

                var result = new OcrValidationResponse
                {
                    Id = Guid.NewGuid().ToString(),
                    IsValid = validationResult.GetValueOrDefault("isValid", "false") == "true",
                    CorrectedData = ParseCorrectedData(validationResult.GetValueOrDefault("correctedData", "{}")),
                    ValidationErrors = ParseStringArray(validationResult.GetValueOrDefault("errors", "[]")),
                    Suggestions = ParseStringArray(validationResult.GetValueOrDefault("suggestions", "[]")),
                    OverallConfidence = double.TryParse(validationResult.GetValueOrDefault("confidence"), out var conf) ? conf : 0.0,
                    ValidatedAt = DateTime.UtcNow
                };

                _logger.LogInformation("OCR data validation completed");
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during OCR data validation");
                throw new InvalidOperationException($"Failed to validate OCR data: {ex.Message}", ex);
            }
        }

        public async Task<BatchOcrResponse> ExtractBatchDocumentsAsync(BatchOcrRequest dto)
        {
            var stopwatch = Stopwatch.StartNew();
            var results = new List<BatchOcrResult>();
            int successful = 0;
            int failed = 0;

            try
            {
                _logger.LogInformation("Starting batch OCR extraction for {DocumentCount} documents", dto.Documents.Count);

                foreach (var document in dto.Documents)
                {
                    try
                    {
                        var batchResult = new BatchOcrResult
                        {
                            ReferenceId = document.ReferenceId,
                            DocumentType = document.DocumentType
                        };

                        if (document.DocumentType.Equals("CitizenId", StringComparison.OrdinalIgnoreCase))
                        {
                            var citizenRequest = new CitizenIdOcrRequest
                            {
                                ImageBase64 = document.ImageBase64,
                                ImageFormat = document.ImageFormat,
                                Language = dto.Language,
                                EnhanceAccuracy = dto.EnhanceAccuracy
                            };
                            
                            var citizenResult = await ExtractCitizenIdDataAsync(citizenRequest);
                            batchResult.IsSuccess = true;
                            batchResult.ExtractedData = citizenResult;
                            batchResult.ConfidenceScore = citizenResult.ConfidenceScore;
                            successful++;
                        }
                        else if (document.DocumentType.Equals("VehicleRegistration", StringComparison.OrdinalIgnoreCase))
                        {
                            var vehicleRequest = new VehicleRegistrationOcrRequest
                            {
                                ImageBase64 = document.ImageBase64,
                                ImageFormat = document.ImageFormat,
                                Language = dto.Language,
                                EnhanceAccuracy = dto.EnhanceAccuracy
                            };
                            
                            var vehicleResult = await ExtractVehicleRegistrationDataAsync(vehicleRequest);
                            batchResult.IsSuccess = true;
                            batchResult.ExtractedData = vehicleResult;
                            batchResult.ConfidenceScore = vehicleResult.ConfidenceScore;
                            successful++;
                        }
                        else
                        {
                            batchResult.IsSuccess = false;
                            batchResult.ErrorMessage = $"Unsupported document type: {document.DocumentType}";
                            failed++;
                        }

                        results.Add(batchResult);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error processing document {ReferenceId}", document.ReferenceId);
                        results.Add(new BatchOcrResult
                        {
                            ReferenceId = document.ReferenceId,
                            DocumentType = document.DocumentType,
                            IsSuccess = false,
                            ErrorMessage = ex.Message,
                            ConfidenceScore = 0.0
                        });
                        failed++;
                    }
                }

                stopwatch.Stop();

                var result = new BatchOcrResponse
                {
                    Id = Guid.NewGuid().ToString(),
                    Results = results,
                    TotalProcessed = dto.Documents.Count,
                    SuccessfulExtractions = successful,
                    FailedExtractions = failed,
                    ProcessedAt = DateTime.UtcNow,
                    TotalProcessingTimeMs = stopwatch.ElapsedMilliseconds.ToString()
                };

                _logger.LogInformation("Batch OCR extraction completed. Total: {Total}, Success: {Success}, Failed: {Failed}",
                    dto.Documents.Count, successful, failed);

                return result;
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                _logger.LogError(ex, "Error during batch OCR extraction");
                throw new InvalidOperationException($"Failed to process batch OCR extraction: {ex.Message}", ex);
            }
        }

        public async Task<OcrServiceHealthDto> CheckOcrServiceHealthAsync()
        {
            var stopwatch = Stopwatch.StartNew();
            
            try
            {
                _logger.LogInformation("Checking Gemini OCR service health");

                // Simple health check by calling the models endpoint
                var url = $"{_baseUrl}/models?key={_apiKey}";
                var response = await _httpClient.GetAsync(url);
                
                stopwatch.Stop();

                var isHealthy = response.IsSuccessStatusCode;
                var responseContent = await response.Content.ReadAsStringAsync();
                
                var result = new OcrServiceHealthDto
                {
                    Id = Guid.NewGuid().ToString(),
                    IsHealthy = isHealthy,
                    ServiceStatus = isHealthy ? "Healthy" : "Unhealthy",
                    AvailableModels = isHealthy ? ParseAvailableModels(responseContent) : new List<string>(),
                    ResponseTimeMs = stopwatch.ElapsedMilliseconds,
                    CheckedAt = DateTime.UtcNow,
                    ErrorMessage = isHealthy ? null : $"HTTP {response.StatusCode}: {response.ReasonPhrase}",
                    AdditionalInfo = new Dictionary<string, object>
                    {
                        { "HttpStatusCode", (int)response.StatusCode },
                        { "ApiEndpoint", _baseUrl }
                    }
                };

                _logger.LogInformation("Gemini OCR service health check completed. Status: {Status}", result.ServiceStatus);
                return result;
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                _logger.LogError(ex, "Error during Gemini OCR service health check");
                
                return new OcrServiceHealthDto
                {
                    Id = Guid.NewGuid().ToString(),
                    IsHealthy = false,
                    ServiceStatus = "Unhealthy",
                    AvailableModels = new List<string>(),
                    ResponseTimeMs = stopwatch.ElapsedMilliseconds,
                    CheckedAt = DateTime.UtcNow,
                    ErrorMessage = ex.Message,
                    AdditionalInfo = new Dictionary<string, object>
                    {
                        { "ExceptionType", ex.GetType().Name }
                    }
                };
            }
        }

        #region Private Helper Methods

        private string CreateCitizenIdPrompt(string language, bool enhanceAccuracy)
        {
            var prompt = language.ToLower() switch
            {
                "vi" => @"Phân tích hình ảnh Căn cước công dân Việt Nam và trích xuất thông tin sau dưới dạng JSON:
{
  ""citizenId"": ""số căn cước"",
  ""fullName"": ""họ và tên"",
  ""dateOfBirth"": ""ngày sinh (DD/MM/YYYY)"",
  ""sex"": ""giới tính"",
  ""nationality"": ""quốc tịch"",
  ""placeOfOrigin"": ""quê quán"",
  ""placeOfResidence"": ""nơi thường trú"",
  ""expiryDate"": ""ngày hết hạn (DD/MM/YYYY)""
}",
                _ => @"Analyze the Vietnamese Citizen ID card image and extract the following information in JSON format:
{
  ""citizenId"": ""citizen ID number"",
  ""fullName"": ""full name"",
  ""dateOfBirth"": ""date of birth (DD/MM/YYYY)"",
  ""sex"": ""gender"",
  ""nationality"": ""nationality"",
  ""placeOfOrigin"": ""place of origin"",
  ""placeOfResidence"": ""place of residence"",
  ""expiryDate"": ""expiry date (DD/MM/YYYY)""
}"
            };

            if (enhanceAccuracy)
            {
                prompt += language.ToLower() == "vi" 
                    ? "\n\nLưu ý: Hãy đọc kỹ và chính xác nhất có thể. Nếu không rõ thông tin nào, hãy để trống."
                    : "\n\nNote: Please read carefully and be as accurate as possible. If any information is unclear, leave it empty.";
            }

            return prompt;
        }

        private string CreateVehicleRegistrationPrompt(string language, bool enhanceAccuracy)
        {
            var prompt = language.ToLower() switch
            {
                "vi" => @"Phân tích hình ảnh Đăng ký xe/Giấy chứng nhận đăng ký xe Việt Nam và trích xuất thông tin sau dưới dạng JSON:
{
  ""licensePlate"": ""biển số xe"",
  ""brand"": ""nhãn hiệu"",
  ""model"": ""số loại"",
  ""engineNumber"": ""số máy"",
  ""chassisNumber"": ""số khung"",
  ""color"": ""màu sơn"",
  ""ownerFullName"": ""tên chủ sở hữu"",
  ""registrationDate"": ""ngày đăng ký (DD/MM/YYYY)"",
  ""vehicleType"": ""loại phương tiện"",
  ""plateType"": ""loại biển số""
}",
                _ => @"Analyze the Vietnamese Vehicle Registration Certificate image and extract the following information in JSON format:
{
  ""licensePlate"": ""license plate number"",
  ""brand"": ""vehicle brand"",
  ""model"": ""vehicle model"",
  ""engineNumber"": ""engine number"",
  ""chassisNumber"": ""chassis number"",
  ""color"": ""vehicle color"",
  ""ownerFullName"": ""owner full name"",
  ""registrationDate"": ""registration date (DD/MM/YYYY)"",
  ""vehicleType"": ""vehicle type"",
  ""plateType"": ""plate type""
}"
            };

            if (enhanceAccuracy)
            {
                prompt += language.ToLower() == "vi" 
                    ? "\n\nLưu ý: Hãy đọc kỹ và chính xác nhất có thể. Nếu không rõ thông tin nào, hãy để trống."
                    : "\n\nNote: Please read carefully and be as accurate as possible. If any information is unclear, leave it empty.";
            }

            return prompt;
        }

        private string CreateValidationPrompt(string documentType, Dictionary<string, string> extractedData)
        {
            var dataJson = JsonSerializer.Serialize(extractedData);
            return $@"Validate the following OCR extracted data for a {documentType} document and return a JSON response:

Extracted Data: {dataJson}

Please validate each field and return:
{{
  ""isValid"": ""true/false"",
  ""correctedData"": {{corrected fields if any}},
  ""errors"": [""list of validation errors""],
  ""suggestions"": [""list of suggestions for improvement""],
  ""confidence"": ""overall confidence score (0.0-1.0)""
}}

Focus on format validation, logical consistency, and typical patterns for Vietnamese documents.";
        }

        private object CreateGeminiVisionRequest(string prompt, string imageBase64, string imageFormat)
        {
            return new
            {
                contents = new[]
                {
                    new
                    {
                        parts = new object[]
                        {
                            new { text = prompt },
                            new
                            {
                                inline_data = new
                                {
                                    mime_type = $"image/{imageFormat}",
                                    data = imageBase64
                                }
                            }
                        }
                    }
                },
                generationConfig = new
                {
                    temperature = 0.1,
                    topK = 32,
                    topP = 1,
                    maxOutputTokens = 2048
                }
            };
        }

        private object CreateGeminiTextRequest(string prompt)
        {
            return new
            {
                contents = new[]
                {
                    new
                    {
                        parts = new[]
                        {
                            new { text = prompt }
                        }
                    }
                },
                generationConfig = new
                {
                    temperature = 0.1,
                    topK = 32,
                    topP = 1,
                    maxOutputTokens = 2048
                }
            };
        }

        private async Task<string> SendGeminiRequestAsync(object request)
        {
            var model = "gemini-1.5-flash"; // or gemini-pro-vision
            var url = $"{_baseUrl}/models/{model}:generateContent?key={_apiKey}";
            
            var jsonContent = JsonSerializer.Serialize(request);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            
            var response = await _httpClient.PostAsync(url, content);
            
            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Gemini API request failed: {response.StatusCode} - {errorContent}");
            }
            
            return await response.Content.ReadAsStringAsync();
        }

        private Dictionary<string, string> ParseCitizenIdResponse(string response)
        {
            return ParseGeminiResponse(response);
        }

        private Dictionary<string, string> ParseVehicleRegistrationResponse(string response)
        {
            return ParseGeminiResponse(response);
        }

        private Dictionary<string, string> ParseValidationResponse(string response)
        {
            return ParseGeminiResponse(response);
        }

        private Dictionary<string, string> ParseGeminiResponse(string response)
        {
            try
            {
                using var doc = JsonDocument.Parse(response);
                var candidates = doc.RootElement.GetProperty("candidates");
                if (candidates.GetArrayLength() > 0)
                {
                    var content = candidates[0].GetProperty("content");
                    var parts = content.GetProperty("parts");
                    if (parts.GetArrayLength() > 0)
                    {
                        var text = parts[0].GetProperty("text").GetString() ?? "";
                        
                        // Extract JSON from the response text
                        var jsonStart = text.IndexOf('{');
                        var jsonEnd = text.LastIndexOf('}');
                        
                        if (jsonStart != -1 && jsonEnd != -1 && jsonEnd > jsonStart)
                        {
                            var jsonText = text.Substring(jsonStart, jsonEnd - jsonStart + 1);
                            return JsonSerializer.Deserialize<Dictionary<string, string>>(jsonText) ?? new Dictionary<string, string>();
                        }
                    }
                }
                
                return new Dictionary<string, string>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error parsing Gemini response: {Response}", response);
                return new Dictionary<string, string>();
            }
        }

        private double CalculateConfidenceScore(Dictionary<string, string> extractedData)
        {
            if (!extractedData.Any()) return 0.0;
            
            var filledFields = extractedData.Values.Count(v => !string.IsNullOrWhiteSpace(v));
            return (double)filledFields / extractedData.Count;
        }

        private List<OcrFieldConfidenceDto> CreateFieldConfidences(Dictionary<string, string> extractedData)
        {
            return extractedData.Select(kvp => new OcrFieldConfidenceDto
            {
                FieldName = kvp.Key,
                ExtractedValue = kvp.Value,
                Confidence = string.IsNullOrWhiteSpace(kvp.Value) ? 0.0 : 0.85, // Simple confidence scoring
                AlternativeValues = null
            }).ToList();
        }

        private Dictionary<string, string> ParseCorrectedData(string correctedDataJson)
        {
            try
            {
                return JsonSerializer.Deserialize<Dictionary<string, string>>(correctedDataJson) ?? new Dictionary<string, string>();
            }
            catch
            {
                return new Dictionary<string, string>();
            }
        }

        private List<string> ParseStringArray(string jsonArray)
        {
            try
            {
                return JsonSerializer.Deserialize<List<string>>(jsonArray) ?? new List<string>();
            }
            catch
            {
                return new List<string>();
            }
        }

        private List<string> ParseAvailableModels(string responseContent)
        {
            try
            {
                using var doc = JsonDocument.Parse(responseContent);
                var models = new List<string>();
                
                if (doc.RootElement.TryGetProperty("models", out var modelsArray))
                {
                    foreach (var model in modelsArray.EnumerateArray())
                    {
                        if (model.TryGetProperty("name", out var nameElement))
                        {
                            var name = nameElement.GetString();
                            if (!string.IsNullOrEmpty(name))
                            {
                                models.Add(name);
                            }
                        }
                    }
                }
                
                return models;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error parsing available models response");
                return new List<string> { "gemini-1.5-flash", "gemini-pro-vision" };
            }
        }

        #endregion

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _httpClient?.Dispose();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}

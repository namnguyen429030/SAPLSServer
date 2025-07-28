using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SAPLSServer.DTOs.Concrete;
using SAPLSServer.Services.Interfaces;
using System.Diagnostics;
using System.Globalization;
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
        private const string DEFAULT_BASE_URL = "https://generativelanguage.googleapis.com/v1beta";

        public GeminiOcrService(
            IHttpClientFactory httpClientFactory,
            IConfiguration configuration,
            ILogger<GeminiOcrService> logger)
        {
            _httpClient = httpClientFactory.CreateClient();
            _configuration = configuration;
            _logger = logger;

            _apiKey = _configuration["GeminiApi:ApiKey"]
                      ?? throw new InvalidOperationException("Gemini API key is not configured");

            _baseUrl = _configuration["GeminiApi:BaseUrl"]
                      ?? DEFAULT_BASE_URL;

            ConfigureHttpClient();
        }

        private void ConfigureHttpClient()
        {
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _httpClient.Timeout = TimeSpan.FromMinutes(2);
        }

        public async Task<CitizenIdOcrResponse> ExtractCitizenIdDataAsync(CitizenIdOcrRequest request)
        {
            var stopwatch = Stopwatch.StartNew();
            try
            {
                _logger.LogInformation("Starting Citizen ID OCR extraction");

                var prompt = CreateCitizenIdPrompt(request.Language, request.EnhanceAccuracy);
                var geminiRequest = CreateGeminiVisionRequest(prompt, request.FrontImageBase64, request.BackImageBase64, request.ImageFormat);

                var response = await SendGeminiRequestAsync(geminiRequest);
                var extractedData = ParseCitizenIdResponse(response);

                stopwatch.Stop();

                // Check for model errors or empty/invalid results
                if (extractedData.Count == 0 || extractedData.Values.All(string.IsNullOrWhiteSpace))
                    throw new InvalidOperationException("Image is not clear enough or not a valid Citizen ID card.");

                var result = new CitizenIdOcrResponse
                {
                    Id = Guid.NewGuid().ToString(),
                    CitizenId = extractedData.GetValueOrDefault("citizenId", ""),
                    FullName = extractedData.GetValueOrDefault("fullName", ""),
                    DateOfBirth = DateOnly.TryParseExact(extractedData.GetValueOrDefault("dateOfBirth"), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var dob) ? dob : null,
                    Sex = extractedData.GetValueOrDefault("sex", ""),
                    Nationality = extractedData.GetValueOrDefault("nationality", ""),
                    PlaceOfOrigin = extractedData.GetValueOrDefault("placeOfOrigin", ""),
                    PlaceOfResidence = extractedData.GetValueOrDefault("placeOfResidence", ""),
                    ExpiryDate = DateOnly.TryParseExact(extractedData.GetValueOrDefault("expiryDate"), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var exp) ? exp : null
                };

                _logger.LogInformation("Citizen ID OCR extraction completed successfully in {ElapsedMs}ms", stopwatch.ElapsedMilliseconds);
                return result;
            }
            catch (InvalidOperationException)
            {
                throw;
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                _logger.LogError(ex, "Error during Citizen ID OCR extraction");
                throw new InvalidOperationException($"Failed to extract Citizen ID data: {ex.Message}", ex);
            }
        }

        public async Task<VehicleRegistrationOcrResponse> ExtractVehicleRegistrationDataAsync(VehicleRegistrationOcrRequest request)
        {
            var stopwatch = Stopwatch.StartNew();
            try
            {
                _logger.LogInformation("Starting Vehicle Registration OCR extraction");

                var prompt = CreateVehicleRegistrationPrompt(request.Language, request.EnhanceAccuracy);
                var geminiRequest = CreateGeminiVisionRequest(prompt, request.FrontImageBase64, request.BackImageBase64, request.ImageFormat);

                var response = await SendGeminiRequestAsync(geminiRequest);
                var extractedData = ParseVehicleRegistrationResponse(response);

                stopwatch.Stop();

                // Check for model errors or empty/invalid results
                if (extractedData.Count == 0 || extractedData.Values.All(string.IsNullOrWhiteSpace))
                    throw new InvalidOperationException("Image is not clear enough or not a valid Vehicle Registration Certificate.");

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
                    RegistrationDate = DateOnly.TryParseExact(extractedData.GetValueOrDefault("registrationDate"), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var regDate) ? regDate : null,
                    VehicleType = extractedData.GetValueOrDefault("vehicleType", "")
                };

                _logger.LogInformation("Vehicle Registration OCR extraction completed successfully in {ElapsedMs}ms", stopwatch.ElapsedMilliseconds);
                return result;
            }
            catch (InvalidOperationException)
            {
                throw;
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                _logger.LogError(ex, "Error during Vehicle Registration OCR extraction");
                throw new InvalidOperationException($"Failed to extract Vehicle Registration data: {ex.Message}", ex);
            }
        }

        #region Private Helper Methods

        private object CreateGeminiVisionRequest(string prompt, string frontImageBase64, string backImageBase64, string imageFormat)
        {
            var imageParts = new List<object>
            {
                new { text = prompt },
                new
                {
                    inline_data = new
                    {
                        mime_type = $"image/{imageFormat}",
                        data = frontImageBase64
                    }
                },
                new
                {
                    inline_data = new
                    {
                        mime_type = $"image/{imageFormat}",
                        data = backImageBase64
                    }
                }
            };

            return new
            {
                contents = new[]
                {
                    new
                    {
                        parts = imageParts.ToArray()
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

        private static string CreateCitizenIdPrompt(string language, bool enhanceAccuracy)
        {
            var prompt = language.ToLower() switch
            {
                "vi" => @"Phân tích hình ảnh mặt trước và mặt sau của Căn cước công dân Việt Nam và trích xuất thông tin sau dưới dạng JSON:
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
                _ => @"Analyze the front and back images of the Vietnamese Citizen ID card and extract the following information in JSON format:
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

        private static string CreateVehicleRegistrationPrompt(string language, bool enhanceAccuracy)
        {
            var prompt = language.ToLower() switch
            {
                "vi" => @"Phân tích hình ảnh mặt trước và mặt sau của Đăng ký xe/Giấy chứng nhận đăng ký xe Việt Nam và trích xuất thông tin sau dưới dạng JSON:
{
  ""licensePlate"": ""biển số xe"",
  ""brand"": ""nhãn hiệu"",
  ""model"": ""số loại"",
  ""engineNumber"": ""số máy"",
  ""chassisNumber"": ""số khung"",
  ""color"": ""màu sơn"",
  ""ownerFullName"": ""tên chủ sở hữu"",
  ""registrationDate"": ""ngày đăng ký (DD/MM/YYYY)"",
  ""vehicleType"": ""loại phương tiện""
}",
                _ => @"Analyze the front and back images of the Vietnamese Vehicle Registration Certificate and extract the following information in JSON format:
{
  ""licensePlate"": ""license plate number"",
  ""brand"": ""vehicle brand"",
  ""model"": ""vehicle model"",
  ""engineNumber"": ""engine number"",
  ""chassisNumber"": ""chassis number"",
  ""color"": ""vehicle color"",
  ""ownerFullName"": ""owner full name"",
  ""registrationDate"": ""registration date (DD/MM/YYYY)"",
  ""vehicleType"": ""vehicle type""
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

        private async Task<string> SendGeminiRequestAsync(object request)
        {
            var model = "gemini-2.5-flash";
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

        private Dictionary<string, string> ParseCitizenIdResponse(string response) => ParseGeminiResponse(response);
        private Dictionary<string, string> ParseVehicleRegistrationResponse(string response) => ParseGeminiResponse(response);

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

        #endregion
    }
}

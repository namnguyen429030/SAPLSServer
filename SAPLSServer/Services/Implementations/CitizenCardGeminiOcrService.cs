using SAPLSServer.Constants;
using SAPLSServer.DTOs.Concrete.OcrDtos;
using SAPLSServer.Exceptions;
using SAPLSServer.Services.Interfaces;
using System.Globalization;
using System.Text.Json;

namespace SAPLSServer.Services.Implementations
{
    public class CitizenCardGeminiOcrService : ICitizenCardOcrService
    {
        private readonly IHttpClientService _httpClientService;
        private readonly IPromptProviderService _promptProviderService;
        private readonly ILogger<CitizenCardGeminiOcrService> _logger;
        private readonly string _baseUrl;
        private readonly string _modelName;
        private readonly string _apiKey;
        public CitizenCardGeminiOcrService(IPromptProviderService promptProviderService,
            ILogger<CitizenCardGeminiOcrService> logger,
            IHttpClientService httpClientService, 
            ICitizenCardOcrSettings settings)
        {
            _promptProviderService = promptProviderService;
            _logger = logger;
            _httpClientService = httpClientService;
            _baseUrl = settings.OcrBaseUrl;
            _modelName = settings.OcrModelName;
            _apiKey = settings.OcrApiKey;
        }
        public async Task<CitizenIdOcrResponse> ExtractDataFromBase64(CitizenIdOcrRequest request)
        {
            var prompt = _promptProviderService.CitizenCardPrompt;
            var geminiRequest = CreateGeminiRequest(prompt, request.FrontImageBase64, request.BackImageBase64, request.FrontImageFormat, request.BackImageFormat);
            var url = $"{_baseUrl}/models/{_modelName}:generateContent?key={_apiKey}";

            var response = await _httpClientService.PostAsync(url, JsonSerializer.Serialize(geminiRequest));
            _logger.LogInformation("Gemini OCR response: {Response}", response);
            if (string.IsNullOrWhiteSpace(response))
            {
                throw new InvalidOperationException(MessageKeys.GEMINI_OCR_SERVICE_IS_UNAVAILABLE);
            }
            string jsonContent;
            try
            {
                jsonContent = ExtractJsonFromResponse(response);
            }
            catch (JsonException)
            {
                throw new InvalidInformationException();
            }
            return ParseCitizenIdResponse(jsonContent);
        }

        public async Task<CitizenIdOcrResponse> ExtractDataFromFile(CitizenIdOcrFileRequest request)
        {
            if(request.FrontImage == null || request.BackImage == null)
            {
                throw new InvalidInformationException();//should not run here
            }
            var (frontImageBase64, frontMimeType) = await GetImageAsBase64Async(request.FrontImage);
            var (backImageBase64, backMimeType) = await GetImageAsBase64Async(request.BackImage);

            var base64Request = new CitizenIdOcrRequest
            {
                FrontImageBase64 = frontImageBase64,
                BackImageBase64 = backImageBase64,
                FrontImageFormat = frontMimeType.Replace("image/", ""),
                BackImageFormat = backMimeType.Replace("image/", "")
            };

            return await ExtractDataFromBase64(base64Request);
        }

        private static object CreateGeminiRequest(string prompt, string frontImageBase64, string backImageBase64, string frontImageFormat, string backImageFormat)
        {
            var imageParts = new List<object>
            {
                new { text = prompt },
                new
                {
                    inline_data = new
                    {
                        mime_type = $"image/{frontImageFormat}",
                        data = frontImageBase64
                    }
                }
            };

            if (!string.IsNullOrWhiteSpace(backImageBase64))
            {
                imageParts.Add(new
                {
                    inline_data = new
                    {
                        mime_type = $"image/{backImageFormat}",
                        data = backImageBase64
                    }
                });
            }

            return new
            {
                contents = new[]
                {
                    new { parts = imageParts.ToArray() }
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

        private static string ExtractJsonFromResponse(string response)
        {
            using var doc = JsonDocument.Parse(response);
            if(!doc.RootElement.TryGetProperty("candidates", out var candidates) || candidates.ValueKind != JsonValueKind.Array)
            {
                return "{}"; // Return empty JSON if no candidates found
            }
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
                        return text.Substring(jsonStart, jsonEnd - jsonStart + 1);
                    }
                }
            }
            return "{}";
        }

        private static CitizenIdOcrResponse ParseCitizenIdResponse(string jsonContent)
        {
            using var doc = JsonDocument.Parse(jsonContent);
            var root = doc.RootElement;

            var extractedData = root.TryGetProperty("extractedData", out var data) ? data : default;

            if (root.TryGetProperty("validationErrors", out var errorsElement)
                && errorsElement.ValueKind == JsonValueKind.Array
                && errorsElement.GetArrayLength() > 0)
            {
                var error = errorsElement.EnumerateArray().FirstOrDefault();
                throw new InvalidMediaException(error.GetString() ?? string.Empty);
            }

            // Throw NullValueDataException if any required field is missing
            var requiredFields = new[] { "citizenId", "fullName", "dateOfBirth", "sex", "nationality", "placeOfOrigin", "placeOfResidence", "expiryDate" };
            foreach (var field in requiredFields)
            {
                if (!extractedData.TryGetProperty(field, out var prop) || prop.ValueKind == JsonValueKind.Null || (prop.ValueKind == JsonValueKind.String && string.IsNullOrWhiteSpace(prop.GetString())))
                {
                    throw new NullValueDataException();
                }
            }

            return new CitizenIdOcrResponse
            {
                CitizenId = GetStringValue(extractedData, "citizenId"),
                FullName = GetStringValue(extractedData, "fullName"),
                DateOfBirth = ParseDate(GetStringValue(extractedData, "dateOfBirth")),
                Sex = GetStringValue(extractedData, "sex"),
                Nationality = GetStringValue(extractedData, "nationality"),
                PlaceOfOrigin = GetStringValue(extractedData, "placeOfOrigin"),
                PlaceOfResidence = GetStringValue(extractedData, "placeOfResidence"),
                ExpiryDate = ParseDate(GetStringValue(extractedData, "expiryDate")),
            };
        }

        private static string GetStringValue(JsonElement element, string propertyName)
        {
            return element.TryGetProperty(propertyName, out var prop) ? prop.GetString() ?? throw new NullValueDataException() : "Unknown";
        }

        private static DateOnly? ParseDate(string dateString)
        {
            if (string.IsNullOrWhiteSpace(dateString))
                return null;

            return DateOnly.TryParseExact(dateString, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var date) ? date : null;
        }

        private static async Task<(string base64Image, string mimeType)> GetImageAsBase64Async(IFormFile file)
        {
            using var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);
            var imageBytes = memoryStream.ToArray();
            return (Convert.ToBase64String(imageBytes), file.ContentType);
        }
    }
}

using SAPLSServer.DTOs.Concrete.GeminiOcr;

namespace SAPLSServer.Services.Interfaces
{
    public interface IGeminiOcrService
    {
        /// <summary>
        /// Extract data from Citizen ID Card image using Gemini Vision OCR
        /// </summary>
        /// <param name="dto">Citizen ID Card OCR request</param>
        /// <returns>Extracted citizen information</returns>
        Task<CitizenIdOcrResponse> ExtractCitizenIdDataAsync(CitizenIdOcrRequest dto);

        /// <summary>
        /// Extract data from Vehicle Registration Certificate image using Gemini Vision OCR
        /// </summary>
        /// <param name="dto">Vehicle Registration OCR request</param>
        /// <returns>Extracted vehicle registration information</returns>
        Task<VehicleRegistrationOcrResponse> ExtractVehicleRegistrationDataAsync(VehicleRegistrationOcrRequest dto);

        /// <summary>
        /// Validate and correct OCR extracted data using AI
        /// </summary>
        /// <param name="dto">OCR validation request</param>
        /// <returns>Validated and corrected data</returns>
        Task<OcrValidationResponse> ValidateOcrDataAsync(OcrValidationRequest dto);

        /// <summary>
        /// Extract data from multiple document images in batch
        /// </summary>
        /// <param name="dto">Batch OCR request</param>
        /// <returns>Batch extraction results</returns>
        Task<BatchOcrResponse> ExtractBatchDocumentsAsync(BatchOcrRequest dto);

        /// <summary>
        /// Check OCR service health and available models
        /// </summary>
        /// <returns>OCR service status</returns>
        Task<OcrServiceHealthDto> CheckOcrServiceHealthAsync();
    }
}


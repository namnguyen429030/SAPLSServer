using SAPLSServer.DTOs.Concrete;
using SAPLSServer.DTOs.Concrete.OcrDto;

namespace SAPLSServer.Services.Interfaces
{
    /// <summary>
    /// Provides OCR extraction services for Vietnamese Citizen ID cards and Vehicle Registration Certificates using Gemini.
    /// </summary>
    public interface IGeminiOcrService
    {
        /// <summary>
        /// Extracts structured data from the front and back images of a Vietnamese Citizen ID card.
        /// </summary>
        /// <param name="request">The request containing base64-encoded front and back images, image format, and accuracy options.</param>
        /// <returns>A <see cref="CitizenIdOcrResponse"/> containing the extracted fields from the ID card.</returns>
        Task<CitizenIdOcrResponse> ExtractCitizenIdDataAsync(CitizenIdOcrRequest request);

        /// <summary>
        /// Extracts structured data from the front and back images of a Vietnamese Vehicle Registration Certificate.
        /// </summary>
        /// <param name="request">The request containing base64-encoded front and back images, image format, and accuracy options.</param>
        /// <returns>A <see cref="VehicleRegistrationOcrResponse"/> containing the extracted fields from the registration certificate.</returns>
        Task<VehicleRegistrationOcrResponse> ExtractVehicleRegistrationDataAsync(VehicleRegistrationOcrRequest request);
    }
}


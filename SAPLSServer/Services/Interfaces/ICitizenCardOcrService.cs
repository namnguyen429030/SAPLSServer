using SAPLSServer.DTOs.Concrete.OcrDtos;

namespace SAPLSServer.Services.Interfaces
{
    public interface ICitizenCardOcrService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<CitizenIdOcrResponse> AttractDataFromFile(CitizenIdOcrFileRequest request);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<CitizenIdOcrResponse> AttractDataFromBase64(CitizenIdOcrRequest request);
    }
}

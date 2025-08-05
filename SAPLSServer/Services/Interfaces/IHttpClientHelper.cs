namespace SAPLSServer.Services.Interfaces
{
    public interface IHttpClientHelper
    {
        Task<T?> GetAsync<T>(string url, Dictionary<string, string>? headers = null);
        Task<T?> PostAsync<T>(string url, object data, Dictionary<string, string>? headers = null);
        Task<T?> PutAsync<T>(string url, object data, Dictionary<string, string>? headers = null);
        Task<bool> DeleteAsync(string url, Dictionary<string, string>? headers = null);
    }
}

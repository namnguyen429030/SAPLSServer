namespace SAPLSServer.DTOs.Concrete.FileUploadDtos
{
    public class FileDownloadResponse
    {
        public Stream FileStream { get; set; } = null!;
        public string FileName { get; set; } = string.Empty;
        public string ContentType { get; set; } = string.Empty;
        public long FileSize { get; set; }
    }
}
namespace ISummationPOC.Service
{
    public interface IFileUploadService
    {
        Task<string> UploadFileAsync(Stream fileStream, string fileName);
    }
}

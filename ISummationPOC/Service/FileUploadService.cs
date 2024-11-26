using Azure.Storage.Blobs;

namespace ISummationPOC.Service
{
    public class FileUploadService : IFileUploadService
    {
        private readonly BlobContainerClient _blobContainerClient;

        public FileUploadService(IConfiguration configuration)
        {
            var connectionString = configuration.GetValue<string>("AzureBlobStorage:ConnectionString");
            var containerName = configuration.GetValue<string>("AzureBlobStorage:ContainerName");

            var blobServiceClient = new BlobServiceClient(connectionString);
            _blobContainerClient = blobServiceClient.GetBlobContainerClient(containerName);
        }

        public async Task<string> UploadFileAsync(Stream fileStream, string fileName)
        {
            var blobClient = _blobContainerClient.GetBlobClient(fileName);
            var blobHttpHeaders = new Azure.Storage.Blobs.Models.BlobHttpHeaders
            {
                ContentType = "image/png"
            };

            await blobClient.UploadAsync(fileStream, new Azure.Storage.Blobs.Models.BlobUploadOptions
            {
                HttpHeaders = blobHttpHeaders,

            });

            return blobClient.Uri.ToString();
        }

        public Task<string> UploadFileAsync(IFormFile image)
        {
            throw new NotImplementedException();
        }
    }
}

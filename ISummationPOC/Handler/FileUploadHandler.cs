using ISummationPOC.Command;
using ISummationPOC.Service;

namespace ISummationPOC.Handler
{
    public class FileUploadHandler
    {
        private readonly IFileUploadService _fileuploadservice;

        public FileUploadHandler(IFileUploadService fileuploadservice)
        {
            _fileuploadservice = fileuploadservice;
        }

        public async Task<string> HandleAsync(FileUploadCommand command)
        {
            if (command.File == null || command.File.Length == 0)
            {
                throw new InvalidDataException("No file uploaded.");
            }

            var fileName = Path.GetFileName(command.File.FileName);

            // Upload the file to Blob Storage and return the URL
            using (var stream = command.File.OpenReadStream())
            {
                var fileUrl = await _fileuploadservice.UploadFileAsync(stream, fileName);
                return fileUrl;
            }
        }
    }
}

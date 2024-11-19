namespace ISummationPOC.Command
{
    public class FileUploadCommand
    {
        public IFormFile File { get; set; }

        public FileUploadCommand(IFormFile file)
        {
            File = file;
        }
    }
}

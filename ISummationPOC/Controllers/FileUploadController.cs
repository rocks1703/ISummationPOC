using ISummationPOC.Command;
using ISummationPOC.Handler;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ISummationPOC.Controllers
{
    [Route("fileuploadcontroller")]
    [ApiController]
    public class FileUploadController : Controller
    {
        private readonly FileUploadHandler _fileUploadHandler;

        public FileUploadController(FileUploadHandler fileUploadHandler)
        {
            _fileUploadHandler = fileUploadHandler;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadImage(IFormFile file)
        {
            if (file == null)
            {
                return BadRequest("No file uploaded.");
            }

            var command = new FileUploadCommand(file);
            var fileUrl = await _fileUploadHandler.HandleAsync(command);

            return Ok(new { FileUrl = fileUrl });
        }
    }
}

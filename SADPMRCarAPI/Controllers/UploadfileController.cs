using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SADPMRCarAPI.DTO.UploadfileDto;
using SADPMRCarAPI.Services.UploadFile;

namespace SADPMRCarAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadfileController : ControllerBase
    {
        private readonly IUploadfileService _fileUploadService;

        public UploadfileController(IUploadfileService fileUploadService)
        {
            _fileUploadService = fileUploadService;
        }

        [HttpPost]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("Invalid file");
            }

            var fileDto = new AddUploadfileDto
            {
                FileName = file.FileName,
                FileContent = await GetFileContent(file)
            };

            var fileId = await _fileUploadService.UploadFileAsync(fileDto);

            return Ok(fileId);
        }

        private static async Task<byte[]> GetFileContent(IFormFile file)
        {
            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                return memoryStream.ToArray();
            }
        }
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.Exchange.WebServices.Data;
using SADPMRCarAPI.Data;
using SADPMRCarAPI.DTO.UploadfileDto;
using SADPMRCarAPI.Model;
using System.Web.Mvc;

namespace SADPMRCarAPI.Services.UploadFile
{
    public class UploadfileService : IUploadfileService
    {
        private readonly ApplicationDbContext _dbContext;

        private readonly IWebHostEnvironment _hostEnvironment;

        public UploadfileService(ApplicationDbContext dbContext, IWebHostEnvironment hostEnvironment)
        {
            _dbContext = dbContext;
            _hostEnvironment = hostEnvironment;
        }

        public async Task<int> UploadFileAsync(AddUploadfileDto fileDto)
        {
            var uploadPath = Path.Combine(_hostEnvironment.ContentRootPath, "Resource/documents");
            Directory.CreateDirectory(uploadPath);

            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(fileDto.FileName);
            var filePath = Path.Combine(uploadPath, fileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await fileStream.WriteAsync(fileDto.FileContent, 0, fileDto.FileContent.Length);
            }

            var uploadFile = new Uploadfile
            {
                FileName = fileName,
                UploadFileTime = DateTime.Now
            };

            _dbContext.Uploadfiles.Add(uploadFile);
            await _dbContext.SaveChangesAsync();

            return uploadFile.UploadfileId;
        }




    }


}


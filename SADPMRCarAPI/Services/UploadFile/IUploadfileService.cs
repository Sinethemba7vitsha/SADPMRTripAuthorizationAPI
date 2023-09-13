using Microsoft.AspNetCore.Mvc;
using SADPMRCarAPI.DTO.UploadfileDto;

namespace SADPMRCarAPI.Services.UploadFile
{
    public interface IUploadfileService
    {
        Task<int> UploadFileAsync(AddUploadfileDto fileDto);
       
    }
}

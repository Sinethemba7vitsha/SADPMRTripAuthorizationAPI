using System.ComponentModel.DataAnnotations.Schema;

namespace SADPMRCarAPI.DTO.UploadfileDto
{
    public class AddUploadfileDto
    {
        public string? FileName { get; set; }
        public byte[]? FileContent { get; set; }
    }
}

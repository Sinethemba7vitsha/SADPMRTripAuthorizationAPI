namespace SADPMRCarAPI.DTO.UploadfileDto
{
    public class GetUploadfileDto
    {
        public int UploadfileId { get; set; }
        public string? FileName { get; set; }
        public int UploadfileSize { get; set; }
        public DateTime UploadFileTime { get; set; }

    }
}

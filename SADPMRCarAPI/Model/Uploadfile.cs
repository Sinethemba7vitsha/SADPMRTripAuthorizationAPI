namespace SADPMRCarAPI.Model
{
    public class Uploadfile
    {
        public int UploadfileId { get; set; }
        public byte[]? FileContent { get; set; }
        public string? FileName { get; set; }    
        public DateTime? UploadFileTime { get; set; }
    }
}

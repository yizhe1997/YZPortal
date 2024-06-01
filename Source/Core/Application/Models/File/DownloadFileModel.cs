namespace Application.Models.File
{
    public class DownloadFileModel : FileModel
    {
        public MemoryStream? Stream { get; set; }
        public string? ContentType { get; set; }
        public string? FileName { get; set; }
    }
}

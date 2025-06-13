namespace WebApiWhatsF.Models
{
    public class PdfDocument
    {
        public int Id { get; set; }
        public string? FileName { get; set; }
        public byte[]? FileData { get; set; }
        public string? FilePath { get; set; }
        public string? ContentType { get; set; }
    }
}

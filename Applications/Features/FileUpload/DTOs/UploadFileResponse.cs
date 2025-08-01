namespace Applications.Features.FileUpload.DTOs
{
    public class UploadFileResponse
    {
        public string FileKey { get; set; } = default!;
        public string FileUrl { get; set; } = default!; // 👉 Signed URL
    }
}

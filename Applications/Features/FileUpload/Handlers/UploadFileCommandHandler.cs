using Applications.Features.FileUpload.Commands;
using Applications.Features.FileUpload.DTOs;
using Applications.Services.Interfaces;
using Core.Enumerables;
using MediatR;

namespace Applications.Features.FileUpload.Handlers;

public class UploadFileCommandHandler : IRequestHandler<UploadFileCommand, UploadFileResponse>
{
    private readonly IStorageService _storage;

    public UploadFileCommandHandler(IStorageService storage)
    {
        _storage = storage;
    }

    public async Task<UploadFileResponse> Handle(UploadFileCommand request, CancellationToken cancellationToken)
    {
        if (request.File == null || request.File.Length == 0)
        {
            throw new ArgumentException("File không hợp lệ.");
        }

        var stream = request.File.OpenReadStream();
        var fileName = $"{Guid.NewGuid()}_{request.File.FileName}";
        var contentType = request.File.ContentType;

        // 👉 Xác định loại file để phân quyền tự động
        //var isPublicFile = contentType.StartsWith("image/") || contentType == "video/mp4";
        var visibility = FileVisibility.Private;// isPublicFile ? FileVisibility.Public : FileVisibility.Private;

        // 📦 Upload file lên storage
        var fileKey = await _storage.UploadFileAsync(
            stream,
            fileName,
            contentType,
            visibility,
            cancellationToken
        );

        // 🔐 Trả signed URL (dù file là private vẫn xem được tạm thời)
        var signedUrl = _storage.GenerateSignedUrl(fileKey, TimeSpan.FromMinutes(30));

        return new UploadFileResponse
        {
            FileKey = fileKey,
            FileUrl = signedUrl // ✅ Trả signed URL đúng mục đích
        };
    }
}

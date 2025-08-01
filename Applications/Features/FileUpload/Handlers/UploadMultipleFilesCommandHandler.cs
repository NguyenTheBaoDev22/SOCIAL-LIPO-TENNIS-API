using Applications.Features.FileUpload.Commands;
using Applications.Features.FileUpload.DTOs;
using Applications.Services.Interfaces;
using Core.Enumerables;
using MediatR;

namespace Applications.Features.FileUpload.Handlers;

public class UploadMultipleFilesCommandHandler : IRequestHandler<UploadMultipleFilesCommand, List<UploadFileResponse>>
{
    private readonly IStorageService _storage;

    public UploadMultipleFilesCommandHandler(IStorageService storage)
    {
        _storage = storage;
    }

    public async Task<List<UploadFileResponse>> Handle(UploadMultipleFilesCommand request, CancellationToken cancellationToken)
    {
        var results = new List<UploadFileResponse>();

        foreach (var file in request.Files)
        {
            if (file == null || file.Length == 0)
                continue;

            var fileName = $"{Guid.NewGuid()}_{file.FileName}";
            var contentType = file.ContentType;
           // var isPublic = contentType.StartsWith("image/") || contentType == "video/mp4";
            var visibility = FileVisibility.Private;// isPublic ? FileVisibility.Public : FileVisibility.Private;

            var fileKey = await _storage.UploadFileAsync(
                file.OpenReadStream(),
                fileName,
                contentType,
                visibility,
                cancellationToken
            );

            var signedUrl = _storage.GenerateSignedUrl(fileKey, TimeSpan.FromMinutes(30));

            results.Add(new UploadFileResponse
            {
                FileKey = fileKey,
                FileUrl = signedUrl
            });
        }

        return results;
    }
}

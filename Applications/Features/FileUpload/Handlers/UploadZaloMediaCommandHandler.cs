using Applications.Features.FileUpload.Commands;
using Applications.Features.FileUpload.DTOs;
using Applications.Services.Interfaces;
using Core.Enumerables;
using MediatR;
using Serilog;
using Shared.Interfaces;
using Shared.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.Features.FileUpload.Handlers
{
    public class UploadZaloMediaCommandHandler : IRequestHandler<UploadZaloMediaCommand, BaseResponse<UploadZaloMediaResponse>>
    {
        private readonly IStorageService _storageService;
        private readonly ICurrentUserService _currentUser;

        public UploadZaloMediaCommandHandler(IStorageService storageService, ICurrentUserService currentUser)
        {
            _storageService = storageService;
            _currentUser = currentUser;
        }

        public async Task<BaseResponse<UploadZaloMediaResponse>> Handle(UploadZaloMediaCommand request, CancellationToken cancellationToken)
        {
            var traceId = _currentUser.TraceId ?? Guid.NewGuid().ToString("N");
            var result = new UploadZaloMediaResponse();

            if (request.Files == null || request.Files.Count == 0)
            {
                Log.Warning("📷 [ZaloUpload] Không có file nào được gửi lên {@TraceId}", traceId);
                return BaseResponse<UploadZaloMediaResponse>.Error("Không có file nào được gửi lên", traceId);
            }

            foreach (var file in request.Files)
            {
                using var stream = file.OpenReadStream();
                var url = await _storageService.UploadFileAsync(
                    stream,
                    fileName: $"{Guid.NewGuid():N}_{file.FileName}",
                    contentType: file.ContentType,
                    visibility: FileVisibility.Public,
                    cancellationToken
                );

                result.Urls.Add(url);
            }

            Log.Information("✅ [ZaloUpload] Upload thành công {Count} file | {@TraceId} | @Urls: {@Urls}", result.Urls.Count, traceId, result.Urls);

            return BaseResponse<UploadZaloMediaResponse>.Success(result, traceId);
        }
    }
}

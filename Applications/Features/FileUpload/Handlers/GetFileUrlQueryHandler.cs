using Applications.Features.FileUpload.Queries;
using Applications.Services.Interfaces;
using MediatR;

namespace Applications.Features.FileUpload.Handlers
{

    public class GetFileUrlQueryHandler : IRequestHandler<GetFileUrlQuery, string>
    {
        private readonly IStorageService _storage;

        public GetFileUrlQueryHandler(IStorageService storage)
        {
            _storage = storage;
        }

        public Task<string> Handle(GetFileUrlQuery request, CancellationToken cancellationToken)
        {
            var signedUrl = _storage.GenerateSignedUrl(request.FileKey, TimeSpan.FromMinutes(30));
            return Task.FromResult(signedUrl);
        }
    }
}

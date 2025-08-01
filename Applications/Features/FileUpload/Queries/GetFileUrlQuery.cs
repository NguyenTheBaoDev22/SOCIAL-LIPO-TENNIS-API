using MediatR;

namespace Applications.Features.FileUpload.Queries
{
    public class GetFileUrlQuery : IRequest<string>
    {
        public string FileKey { get; set; } = default!;
    }
}

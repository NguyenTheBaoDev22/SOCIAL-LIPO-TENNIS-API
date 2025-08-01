using Applications.Features.FileUpload.DTOs;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Applications.Features.FileUpload.Commands
{

    public class UploadFileCommand : IRequest<UploadFileResponse>
    {
        public IFormFile File { get; set; }

        public UploadFileCommand(IFormFile file)
        {
            File = file;
        }
    }
}

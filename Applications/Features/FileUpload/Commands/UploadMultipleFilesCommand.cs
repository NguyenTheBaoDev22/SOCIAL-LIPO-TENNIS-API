using Applications.Features.FileUpload.DTOs;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Applications.Features.FileUpload.Commands
{
    public record UploadMultipleFilesCommand(List<IFormFile> Files) : IRequest<List<UploadFileResponse>>;

}

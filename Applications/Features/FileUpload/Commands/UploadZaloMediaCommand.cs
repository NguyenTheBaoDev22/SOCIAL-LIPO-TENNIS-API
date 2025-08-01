using Applications.Features.FileUpload.DTOs;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.Features.FileUpload.Commands
{
    public class UploadZaloMediaCommand : IRequest<BaseResponse<UploadZaloMediaResponse>>

    {
        public List<IFormFile> Files { get; set; }
    }
}

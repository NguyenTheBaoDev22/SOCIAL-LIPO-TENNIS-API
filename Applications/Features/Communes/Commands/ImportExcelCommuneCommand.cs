using Applications.DTOs;
using MediatR;
using Microsoft.AspNetCore.Http;
using Shared.Results;

namespace Applications.Features.Communes.Commands
{
    // Sửa lại BaseResponse thành BaseResponse<List<Commune>> để phản ánh dữ liệu trả về là một danh sách Commune
    public class ImportExcelCommuneCommand : IRequest<BaseResponse<List<CommuneDto>>>
    {
        public IFormFile File { get; set; }
    }
}

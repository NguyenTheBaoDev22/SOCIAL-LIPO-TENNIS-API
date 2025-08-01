using Applications.DTOs;
using Applications.Interfaces.Services;
using Core.Entities;
using Shared.Results;

namespace Applications.Services.Interfaces
{
    public interface ICommuneService : IBaseService<CommuneDto, Commune>
    {
        // Thêm phương thức đặc thù nếu cần
        Task<BaseResponse<CommuneDto>> GetByCodeAsync(string code);
    }
}

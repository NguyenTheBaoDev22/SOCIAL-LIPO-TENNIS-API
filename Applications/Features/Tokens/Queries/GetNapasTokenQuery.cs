using Applications.DTOs;
using MediatR;
using Shared.Results;

namespace Applications.Features.Tokens.Queries
{
    public class GetNapasTokenQuery : IRequest<BaseResponse<NapasTokenResponse>>
    {
        // Có thể mở rộng filter sau này nếu cần
    }
}

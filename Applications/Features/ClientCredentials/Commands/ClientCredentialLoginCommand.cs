using Applications.Features.ClientCredentials.Dtos;
using MediatR;
using Shared.Results;

namespace Applications.Features.ClientCredentials.Commands
{
    public class ClientCredentialLoginCommand : IRequest<BaseResponse<LoginResponseDto>>
    {
        public string ClientId { get; set; } = null!;
        public string ClientSecret { get; set; } = null!;
    }
}

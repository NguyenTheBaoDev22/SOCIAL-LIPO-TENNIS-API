using Core.Enumerables;
using MediatR;
using Shared.Results;

namespace Applications.Features.ClientCredentials.Commands
{
    public class CreateClientCredentialCommand : IRequest<BaseResponse<Guid>>
    {
        public string ClientId { get; set; } = null!;
        public string ClientSecret { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Role { get; set; } = RoleEnum.Unknown;
    }
}

using MediatR;
using Shared.Results;

namespace Applications.Features.ClientCredentials.Commands
{
    public class ResetClientSecretCommand : IRequest<BaseResponse<string>>
    {
        public Guid Id { get; set; }
    }
}

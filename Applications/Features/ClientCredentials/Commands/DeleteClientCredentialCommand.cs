using MediatR;
using Shared.Results;

namespace Applications.Features.ClientCredentials.Commands
{
    public class DeleteClientCredentialCommand : IRequest<BaseResponse<bool>>
    {
        public Guid Id { get; set; }
    }
}

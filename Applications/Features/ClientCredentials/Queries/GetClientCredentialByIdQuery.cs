using Applications.DTOs;
using MediatR;
using Shared.Results;

namespace Applications.Features.ClientCredentials.Queries
{
    public class GetClientCredentialByIdQuery : IRequest<BaseResponse<ClientCredentialDto>>
    {
        public Guid Id { get; set; }
    }
}

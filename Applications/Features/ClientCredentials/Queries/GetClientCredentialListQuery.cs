using Applications.DTOs;
using MediatR;
using Shared.Results;

namespace Applications.Features.ClientCredentials.Queries
{
    public class GetClientCredentialListQuery : IRequest<BaseResponse<List<ClientCredentialDto>>> { }
}

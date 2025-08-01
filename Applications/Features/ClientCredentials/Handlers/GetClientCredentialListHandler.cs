using Applications.DTOs;
using Applications.Features.ClientCredentials.Queries;
using Applications.Interfaces.Repositories;
using AutoMapper;
using MediatR;
using Shared.Results;

namespace Applications.Features.ClientCredentials.Handlers
{
    public class GetClientCredentialListHandler : IRequestHandler<GetClientCredentialListQuery, BaseResponse<List<ClientCredentialDto>>>
    {
        private readonly IClientCredentialRepository _repository;
        private readonly IMapper _mapper;

        public GetClientCredentialListHandler(IClientCredentialRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<BaseResponse<List<ClientCredentialDto>>> Handle(GetClientCredentialListQuery request, CancellationToken cancellationToken)
        {
            var data = await _repository.GetAllPaginatedAsync();
            var list = _mapper.Map<List<ClientCredentialDto>>(data);
            return BaseResponse<List<ClientCredentialDto>>.Success(list);
        }
    }
}

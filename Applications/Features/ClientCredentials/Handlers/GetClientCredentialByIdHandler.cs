using Applications.DTOs;
using Applications.Features.ClientCredentials.Queries;
using Applications.Interfaces.Repositories;
using AutoMapper;
using MediatR;
using Shared.Results;
using System.Diagnostics;

namespace Applications.Features.ClientCredentials.Handlers
{
    public class GetClientCredentialByIdHandler : IRequestHandler<GetClientCredentialByIdQuery, BaseResponse<ClientCredentialDto>>
    {
        private readonly IClientCredentialRepository _repository;
        private readonly IMapper _mapper;

        public GetClientCredentialByIdHandler(IClientCredentialRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<BaseResponse<ClientCredentialDto>> Handle(GetClientCredentialByIdQuery request, CancellationToken cancellationToken)
        {
            var traceId = Activity.Current?.Id ?? Guid.NewGuid().ToString();

            var entity = await _repository.GetByIdAsync(request.Id, cancellationToken);
            if (entity == null)
            {
                return BaseResponse<ClientCredentialDto>.Error("Client credential not found", "404", traceId);
            }

            var dto = _mapper.Map<ClientCredentialDto>(entity);
            return BaseResponse<ClientCredentialDto>.Success(dto, "Retrieved successfully", "00");
        }
    }

}

using Applications.Features.ClientCredentials.Commands;
using Applications.Interfaces.Repositories;
using AutoMapper;
using Core.Entities;
using MediatR;
using Shared.Results;

namespace Applications.Features.ClientCredentials.Handlers
{
    public class CreateClientCredentialHandler : IRequestHandler<CreateClientCredentialCommand, BaseResponse<Guid>>
    {
        private readonly IClientCredentialRepository _repository;
        private readonly IMapper _mapper;

        public CreateClientCredentialHandler(IClientCredentialRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<BaseResponse<Guid>> Handle(CreateClientCredentialCommand request, CancellationToken cancellationToken)
        {
            // Kiểm tra ClientId trùng
            var existing = await _repository.GetByClientIdAsync(request.ClientId);
            if (existing != null)
                return BaseResponse<Guid>.Error("ClientId already exists");

            // Map sang entity và mã hoá mật khẩu
            var entity = _mapper.Map<ClientCredential>(request);
            entity.ClientSecretHash = BCrypt.Net.BCrypt.HashPassword(request.ClientSecret);

            await _repository.AddAsync(entity);
            await _repository.SaveChangesAsync();

            return BaseResponse<Guid>.Success(entity.Id, "Created successfully");
        }
    }
}

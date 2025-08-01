using Applications.Features.ClientCredentials.Commands;
using Applications.Interfaces.Repositories;
using MediatR;
using Shared.Results;

namespace Applications.Features.ClientCredentials.Handlers
{
    using System.Diagnostics;
    using System.Security.Cryptography;

    public class ResetClientSecretHandler : IRequestHandler<ResetClientSecretCommand, BaseResponse<string>>
    {
        private readonly IClientCredentialRepository _repository;

        public ResetClientSecretHandler(IClientCredentialRepository repository)
        {
            _repository = repository;
        }

        public async Task<BaseResponse<string>> Handle(ResetClientSecretCommand request, CancellationToken cancellationToken)
        {
            var traceId = Activity.Current?.Id ?? Guid.NewGuid().ToString();

            var entity = await _repository.GetByIdAsync(request.Id, cancellationToken);
            if (entity == null)
            {
                return BaseResponse<string>.Error("Client credential not found", "404", traceId);
            }

            try
            {
                var newSecret = Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));
                entity.ClientSecretHash = newSecret;
                _repository.Update(entity);
                await _repository.SaveChangesAsync(cancellationToken);

                return BaseResponse<string>.Success(newSecret, "Secret reset successfully", "00");
            }
            catch (Exception ex)
            {
                return BaseResponse<string>.Error($"Failed to reset secret: {ex.Message}", "500", traceId);
            }
        }
    }

}

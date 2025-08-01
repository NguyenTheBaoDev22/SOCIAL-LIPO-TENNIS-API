using Applications.Features.ClientCredentials.Commands;
using Applications.Interfaces.Repositories;
using MediatR;
using Shared.Results;

namespace Applications.Features.ClientCredentials.Handlers
{
    using System.Diagnostics;

    public class DeleteClientCredentialHandler : IRequestHandler<DeleteClientCredentialCommand, BaseResponse<bool>>
    {
        private readonly IClientCredentialRepository _repository;

        public DeleteClientCredentialHandler(IClientCredentialRepository repository)
        {
            _repository = repository;
        }

        public async Task<BaseResponse<bool>> Handle(DeleteClientCredentialCommand request, CancellationToken cancellationToken)
        {
            var traceId = Activity.Current?.Id ?? Guid.NewGuid().ToString();

            var entity = await _repository.GetByIdAsync(request.Id, cancellationToken);
            if (entity == null)
            {
                return BaseResponse<bool>.Error("Client credential not found", "404", traceId);
            }

            try
            {
                _repository.Delete(entity);
                await _repository.SaveChangesAsync(cancellationToken);

                return BaseResponse<bool>.Success(true, "Deleted successfully", "00");
            }
            catch (Exception ex)
            {
                return BaseResponse<bool>.Error($"An error occurred while deleting: {ex.Message}", "500", traceId);
            }
        }
    }

}

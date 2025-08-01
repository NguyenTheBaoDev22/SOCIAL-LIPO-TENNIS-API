using Applications.Features.ClientCredentials.Commands;
using Applications.Interfaces.Repositories;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Shared.Results;
using System.Diagnostics;

public class UpdateClientCredentialHandler : IRequestHandler<UpdateClientCredentialCommand, BaseResponse<bool>>
{
    private readonly IClientCredentialRepository _repository;
    private readonly IMapper _mapper;
    private readonly ILogger<UpdateClientCredentialHandler> _logger;

    public UpdateClientCredentialHandler(
        IClientCredentialRepository repository,
        IMapper mapper,
        ILogger<UpdateClientCredentialHandler> logger)
    {
        _repository = repository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<BaseResponse<bool>> Handle(UpdateClientCredentialCommand request, CancellationToken cancellationToken)
    {
        var traceId = Activity.Current?.Id ?? Guid.NewGuid().ToString();
        using (_logger.BeginScope(new Dictionary<string, object> { { "TraceId", traceId } }))
        {
            _logger.LogInformation("🔄 [UpdateClientCredential] Bắt đầu cập nhật ClientCredential. Id: {Id}", request.Id);

            var entity = await _repository.GetByIdAsync(request.Id, cancellationToken);
            if (entity == null)
            {
                _logger.LogWarning("⚠️ [UpdateClientCredential] Không tìm thấy client với Id: {Id}", request.Id);
                return BaseResponse<bool>.Error("Client credential not found", "404", traceId);
            }

            try
            {
                _mapper.Map(request, entity);
                _repository.Update(entity);
                await _repository.SaveChangesAsync(cancellationToken);

                _logger.LogInformation("✅ [UpdateClientCredential] Cập nhật thành công cho ClientId: {ClientId}", entity.ClientId);
                return BaseResponse<bool>.Success(true, "Client credential updated successfully", "00");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ [UpdateClientCredential] Lỗi khi cập nhật ClientId: {ClientId}", entity.ClientId);
                return BaseResponse<bool>.Error($"Failed to update client credential: {ex.Message}", "500", traceId);
            }
        }
    }
}

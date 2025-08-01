using Applications.Features.ClientCredentials.Commands;
using Applications.Features.ClientCredentials.Dtos;
using Applications.Interfaces.Repositories;
using Core.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Serilog.Context;
using Shared.Options;
using Shared.Results;
using System.Diagnostics;
using System.Security.Claims;

namespace Applications.Features.ClientCredentials.Handlers
{
    public class ClientCredentialLoginHandler : IRequestHandler<ClientCredentialLoginCommand, BaseResponse<LoginResponseDto>>
    {
        private readonly IClientCredentialRepository _repository;
        private readonly IJwtTokenGenerator _tokenGenerator;
        private readonly ILogger<ClientCredentialLoginHandler> _logger;
        private readonly JwtServiceOptions _options;

        public ClientCredentialLoginHandler(
            IClientCredentialRepository repository,
            IJwtTokenGenerator tokenGenerator,
            ILogger<ClientCredentialLoginHandler> logger,
            IOptions<JwtSettings> jwtSettings)
        {
            _repository = repository;
            _tokenGenerator = tokenGenerator;
            _logger = logger;
            _options = jwtSettings.Value.Service;
        }

        public async Task<BaseResponse<LoginResponseDto>> Handle(ClientCredentialLoginCommand request, CancellationToken cancellationToken)
        {
            var traceId = Activity.Current?.Id ?? Guid.NewGuid().ToString();

            using (LogContext.PushProperty("TraceId", traceId))
            {
                try
                {
                    _logger.LogInformation("🔐 [Login] Login attempt for ClientId: {ClientId}", request.ClientId);

                    var client = await _repository.GetByClientIdAsync(request.ClientId);
                    if (client == null)
                    {
                        _logger.LogWarning("❌ [Login] ClientId not found - {ClientId}", request.ClientId);
                        return BaseResponse<LoginResponseDto>.Error("Invalid client credentials", "401", traceId);
                    }

                    if (!BCrypt.Net.BCrypt.Verify(request.ClientSecret, client.ClientSecretHash))
                    {
                        _logger.LogWarning("❌ [Login] Invalid secret for ClientId - {ClientId}", request.ClientId);
                        return BaseResponse<LoginResponseDto>.Error("Invalid client credentials", "401", traceId);
                    }

                    var claims = new List<Claim>
                {
                    new("client_id", client.ClientId),
                    new("sub", client.Id.ToString())
                };

                    var accessToken = _tokenGenerator.GenerateToken(claims);

                    _logger.LogInformation("✅ [Login] Token generated for ClientId: {ClientId}", client.ClientId);

                    var result = new LoginResponseDto
                    {
                        AccessToken = accessToken,
                        TokenType = "Bearer",
                        ExpiresIn = (int)TimeSpan.FromMinutes(_options.ExpiryInMinutes).TotalSeconds
                    };

                    return BaseResponse<LoginResponseDto>.Success(result, "Login successful", "00");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "🔥 [Login] Exception during login for ClientId: {ClientId}", request.ClientId);
                    return BaseResponse<LoginResponseDto>.Error("An unexpected error occurred", "500", traceId);
                }
            }
        }
    }

}

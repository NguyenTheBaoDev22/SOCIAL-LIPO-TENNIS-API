using Applications.Features.LarksuiteIntegrations.DTOs;
using Applications.Interfaces.Repositories;
using Applications.Services.Interfaces;
using Core.Entities.Integrations;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Shared.Configs;
using Shared.Interfaces;
using Shared.Results;
using System;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Applications.Services.Implementations
{
    // Application/Integrations/Larksuite/Services/LarkEmailService.cs
    public class LarkEmailService : ILarkEmailService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<LarkEmailService> _logger;
        private readonly IUnitOfWork _uow;
        private readonly ICurrentUserService _currentUser;
        private readonly ILarkSuiteConfig _config;

        public LarkEmailService(
            HttpClient httpClient,
            ILogger<LarkEmailService> logger,
        IUnitOfWork uow,
            ICurrentUserService currentUser,
            IOptions<LarkSuiteConfig> config)
        {
            _httpClient = httpClient;
            _logger = logger;
            _uow = uow;
            _currentUser = currentUser;
            _config = config.Value;
            _httpClient.BaseAddress = new Uri(_config.LarkMailApiBaseUrl);
        }

        public async Task<BaseResponse<string>> SendEmailAsync(LarkEmailDto dto, string accessToken, CancellationToken cancellationToken = default)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var payload = new
            {
                attachments = Array.Empty<object>(),
                body_html = dto.Body,
                body_plain_text = dto.Body,
                head_from = new { name = "ZenPay" },
                subject = dto.Subject,
                to = new[] { new { mail_address = dto.RecipientEmail, name = dto.RecipientEmail } }
            };

            var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");
            var apiUrl = $"{_config.LarkMailApiBaseUrl}/user_mailboxes/{dto.FromEmail}/messages/send";

            var response = await _httpClient.PostAsync(apiUrl, content, cancellationToken);
            var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);

            await _uow.ExecuteInTransactionAsync(async () =>
            {
                var log = new LarkEmailLog
                {
                    Id = Guid.NewGuid(),
                    Subject = dto.Subject,
                    Body = dto.Body,
                    Recipient = dto.RecipientEmail,
                    From = dto.FromEmail,
                    Response = responseContent,
                    Status = response.IsSuccessStatusCode ? "Success" : "Fail",
                    CreatedBy = _currentUser.UserId,
                    CreatedAt = DateTime.UtcNow
                };
                await _uow.LarkEmailLogRepositories.AddAsync(log);
            });

            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation("✅ Email sent: {Response}", responseContent);
                return BaseResponse<string>.Success("Email sent.");
            }

            _logger.LogError("❌ Failed to send email: {Status} - {Response}", response.StatusCode, responseContent);
            return BaseResponse<string>.Error("E-LARK-001", "Gửi email thất bại");
        }
    }
}

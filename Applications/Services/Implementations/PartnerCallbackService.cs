using Applications.Interfaces.Repositories;
using Applications.Services.Interfaces;
using AutoMapper;
using Core.Entities;
using Core.Entities.Partners;
using Core.Enumerables;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Serilog;
using System.Net.Http;
using System.Text;

namespace Applications.Services.Implementations;

public class PartnerCallbackService : IPartnerCallbackService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<PartnerCallbackService> _logger;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public PartnerCallbackService(
        IHttpClientFactory httpClientFactory,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILogger<PartnerCallbackService> logger,
        IHttpContextAccessor httpContextAccessor)
    {
        _httpClientFactory = httpClientFactory;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
        _httpContextAccessor = httpContextAccessor;
    }

    /// <summary>
    /// Gửi callback trạng thái duyệt merchant đến partner.
    /// Ghi log callback vào bảng PartnerMerchantStatusCallbackLog.
    /// </summary>
    public async Task<(int StatusCode, string ResponseContent, bool IsSuccess)> SendMerchantStatusCallbackAsync(
        Merchant merchant,
        MerchantBranch branch,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(branch.ActiveCallbackUrl))
        {
            _logger.LogWarning("❌ Callback URL is empty for branch {BranchId}", branch.Id);
            return (0, "Missing callback URL", false);
        }

        var traceId = _httpContextAccessor.HttpContext?.TraceIdentifier ?? Guid.NewGuid().ToString();

        var payload = new
        {
            merchantCode = merchant.MerchantCode,
            merchantId = merchant.Id,
            merchantBranchCode = branch.MerchantBranchCode,
            merchantBranchId = branch.Id,
            status = branch.Status
        };

        var json = JsonConvert.SerializeObject(payload);
        var httpClient = _httpClientFactory.CreateClient();

        var request = new HttpRequestMessage(HttpMethod.Post, branch.ActiveCallbackUrl)
        {
            Content = new StringContent(json, Encoding.UTF8, "application/json")
        };

        try
        {
            var response = await httpClient.SendAsync(request, cancellationToken);
            var responseBody = await response.Content.ReadAsStringAsync(cancellationToken);

            await _unitOfWork.PartnerMerchantStatusCallbackLogRepositories.AddAsync(new PartnerMerchantStatusCallbackLog
            {
                CallbackUrl = branch.ActiveCallbackUrl,
                Payload = json,
                HttpStatusCode = (int)response.StatusCode,
                ResponseContent = responseBody,
                IsSuccess = response.IsSuccessStatusCode,
                Status = response.IsSuccessStatusCode ? MerchantCallbackStatus.Success : MerchantCallbackStatus.Failed,
                SentAt = DateTime.UtcNow,
                RetryCount = 0,
                TraceId=traceId
               
            }, cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("[{TraceId}] ✅ Callback sent to {Url} - Status: {StatusCode}", traceId, branch.ActiveCallbackUrl, (int)response.StatusCode);

            return ((int)response.StatusCode, responseBody, response.IsSuccessStatusCode);
        }
        catch (Exception ex)
        {
            var errorMessage = $"Exception: {ex.GetType().Name} - {ex.Message}";

            await _unitOfWork.PartnerMerchantStatusCallbackLogRepositories.AddAsync(new PartnerMerchantStatusCallbackLog
            {
                CallbackUrl = branch.ActiveCallbackUrl,
                Payload = json,
                HttpStatusCode = 0,
                ResponseContent = errorMessage,
                IsSuccess = false,
                Status = MerchantCallbackStatus.Failed,
                SentAt = DateTime.UtcNow,
                RetryCount = 0
            }, cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogWarning(ex, "[{TraceId}] ❌ Callback failed to partner: {Url}", traceId, branch.ActiveCallbackUrl);

            return (0, errorMessage, false);
        }
    }

    /// <summary>
    /// TODO: Gửi callback khi có giao dịch liên kết thành công.
    /// </summary>
    public async Task<bool> SendTransactionCallbackAsync(PartnerOrder order, PartnerTransactionCallbackLog transaction, CancellationToken cancellationToken)
    {
        _logger.LogInformation("⚠️ SendTransactionCallbackAsync chưa được implement cho order {OrderId}", order.Id);
        return true;
    }
}

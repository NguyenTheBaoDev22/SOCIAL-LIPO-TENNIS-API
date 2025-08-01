using Applications.DTOs;
using Applications.Interfaces.Repositories;
using Applications.Interfaces.Services;
using Core.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Shared.Configs;
using Shared.Results;
using System.Diagnostics;
using System.Text;

namespace Infrastructure.Services
{
    public class MBFSmsBranchnameService : IMBFSmsBranchnameService
    {
        private readonly HttpClient _httpClient;
        private readonly MBFSMSConfig _config;
        private readonly ISmsLogRepository _logRepo;
        private readonly ISmsRetryQueueRepository _retryRepo;
        private readonly ILogger<MBFSmsBranchnameService> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public MBFSmsBranchnameService(
            HttpClient httpClient,
            IOptions<MBFSMSConfig> config,
            ISmsLogRepository logRepo,
            ISmsRetryQueueRepository retryRepo,
            ILogger<MBFSmsBranchnameService> logger,
            IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _config = config.Value;
            _logRepo = logRepo;
            _retryRepo = retryRepo;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }

        // Phương thức gửi SMS Unicode
        public Task<BaseResponse<bool>> SendUnicodeSmsAsync(string phone, string message)
            => SendSmsInternalAsync(phone, message, "1");

        // Phương thức gửi SMS Non-Unicode
        public Task<BaseResponse<bool>> SendNonUnicodeSmsAsync(string phone, string message)
            => SendSmsInternalAsync(phone, message, "2");

        // Phương thức gửi SMS nội bộ (bao gồm cả retry và log)
        public async Task<BaseResponse<bool>> SendSmsInternalAsync(string phone, string message, string unicode, bool isRetry = false, string useCase = "AccountRegistration")
        {
            var idRequest = Guid.NewGuid().ToString("N");

            // Lấy traceId từ HttpContext nếu có, fallback về Guid nếu gọi nội bộ
            var traceId = _httpContextAccessor.HttpContext?.TraceIdentifier ?? Guid.NewGuid().ToString();

            var payload = new
            {
                sendby = _config.Sendby,
                phone,
                password = _config.Password,
                message,
                idrequest = idRequest,
                brandname = _config.Brandname,
                username = _config.Username,
                unicode
            };

            var json = JsonConvert.SerializeObject(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var url = $"{_config.BaseUrl}{_config.SendSMSPath}";

            var stopwatch = Stopwatch.StartNew();
            var response = await _httpClient.PostAsync(url, content);
            stopwatch.Stop();

            var responseContent = await response.Content.ReadAsStringAsync();
            var parsed = JsonConvert.DeserializeObject<Dictionary<string, string>>(responseContent);

            var log = new SmsLog
            {
                Phone = phone,
                Message = message,
                UnicodeType = unicode,
                StatusCode = parsed?.GetValueOrDefault("result") ?? "ERR",
                StatusDetail = parsed?.GetValueOrDefault("detail") ?? "UNKNOWN",
                TransactionId = parsed?.GetValueOrDefault("idtran"),
                RequestPayload = json,
                RawResponse = responseContent,
                ResponseTimeMs = stopwatch.ElapsedMilliseconds,
                UseCase = useCase,
                SentAt = DateTime.UtcNow,
                RetryCount = isRetry ? 1 : 0,
                LastRetryAt = isRetry ? DateTime.UtcNow : null,
                BaseUrl = _config.BaseUrl,
                SendSMSPath = _config.SendSMSPath
            };

            await _logRepo.AddAsync(log);

            if (!log.IsSuccess)
            {
                await _retryRepo.AddAsync(new SmsRetryQueue
                {
                    SmsLog = log,
                    RetryCount = log.RetryCount,
                    LastTriedAt = log.LastRetryAt,
                    IsDone = false
                });
            }

            await _logRepo.SaveChangesAsync();

            _logger.LogInformation("📩 SMS gửi đến {Phone}, Success={IsSuccess}, UseCase={UseCase}, TraceId={TraceId}", phone, log.IsSuccess, log.UseCase, traceId);

            return log.IsSuccess
                ? new BaseResponse<bool>
                {
                    Code = "00",
                    Message = "Success",
                    Data = true,
                    TraceId = traceId
                }
                : new BaseResponse<bool>
                {
                    Code = "500",
                    Message = $"Gửi SMS thất bại: {log.StatusDetail}",
                    Data = false,
                    TraceId = traceId
                };
        }
        // Gửi OTP cho việc đăng ký tài khoản
        public async Task<BaseResponse<bool>> SendAccountRegistrationNonUnicodeSMSOtpAsync(AccountCreationSMSOtp req)
        {
            var message = req.ToSmsOtpMessage();
            const string unicode = "2"; // Unicode = 1, Non-Unicode = 2

            try
            {
                _logger.LogInformation("📩 Sending OTP SMS to {Phone}", req.PhoneNumber);
                return await SendSmsInternalAsync(req.PhoneNumber, message, unicode, false, "AccountRegistrationOtp");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Failed to send OTP SMS to {Phone}", req.PhoneNumber);
                return new BaseResponse<bool> { Code = "500", Message = "Lỗi hệ thống khi gửi OTP", Data = false };
            }
        }
        // Gửi SMS xác nhận tài khoản
        public async Task<BaseResponse<bool>> SendAccountCreationNonUnicodeSMSConfirmationAsync(AccountCreationSMSConfirmation req)
        {
            var message = req.ToNonUnicodeConfirmationMessage();
            const string unicode = "2";

            try
            {
                _logger.LogInformation("📩 Sending confirmation SMS to {Phone}", req.ShopOwnerPhone);
                return await SendSmsInternalAsync(req.ShopOwnerPhone, message, unicode, false, "AccountCreationConfirmation");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Failed to send confirmation SMS to {Phone}", req.ShopOwnerPhone);
                return new BaseResponse<bool> { Code = "500", Message = "Lỗi hệ thống khi gửi xác nhận", Data = false };
            }
        }
        // Gửi OTP cho việc đăng ký tài khoản
        public async Task<BaseResponse<bool>> SendAccountRegistrationUnicodeSMSOtpAsync(AccountCreationSMSOtp req)
        {
            var message = req.ToSmsOtpMessage();
            const string unicode = "1"; // Unicode = 1, Non-Unicode = 2

            try
            {
                _logger.LogInformation("📩 Sending OTP SMS to {Phone}", req.PhoneNumber);
                return await SendSmsInternalAsync(req.PhoneNumber, message, unicode, false, "AccountRegistrationOtp");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Failed to send OTP SMS to {Phone}", req.PhoneNumber);
                return new BaseResponse<bool> { Code = "500", Message = "Lỗi hệ thống khi gửi OTP", Data = false };
            }
        }
        // Gửi SMS xác nhận tài khoản
        public async Task<BaseResponse<bool>> SendAccountCreationUnicodeSMSConfirmationAsync(AccountCreationSMSConfirmation req)
        {
            var message = req.ToUnicodeConfirmationMessage();
            const string unicode = "1";

            try
            {
                _logger.LogInformation("📩 Sending confirmation SMS to {Phone}", req.ShopOwnerPhone);
                return await SendSmsInternalAsync(req.ShopOwnerPhone, message, unicode, false, "AccountCreationConfirmation");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Failed to send confirmation SMS to {Phone}", req.ShopOwnerPhone);
                return new BaseResponse<bool> { Code = "500", Message = "Lỗi hệ thống khi gửi xác nhận", Data = false };
            }
        }
    }
}

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Shared.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    /// <summary>
    /// Dịch vụ chống spam bằng Redis: nhận diện theo IP + Token + Endpoint + KeyPrefix
    /// </summary>
    public class RedisSpamProtectionService : ISpamProtectionService
    {
        private readonly IDistributedCache _cache;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<RedisSpamProtectionService> _logger;

        public RedisSpamProtectionService(
            IDistributedCache cache,
            IHttpContextAccessor httpContextAccessor,
            ILogger<RedisSpamProtectionService> logger)
        {
            _cache = cache;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        /// <summary>
        /// Lấy danh tính client: IP + UserId (nếu có)
        /// </summary>
        private string GetClientIdentifier()
        {
            var ip = _httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString() ?? "unknown_ip";
            var token = _httpContextAccessor.HttpContext?.User?.FindFirst("sub")?.Value ?? "anonymous";
            return $"{ip}_{token}";
        }

        /// <summary>
        /// Lấy endpoint theo định dạng: METHOD:/api/path
        /// </summary>
        private string GetEndpointKey()
        {
            var context = _httpContextAccessor.HttpContext;
            var method = context?.Request?.Method ?? "UNKNOWN";
            var path = context?.Request?.Path.ToString().ToLowerInvariant() ?? "unknown";
            return $"{method}:{path}";
        }

        /// <summary>
        /// Kiểm tra xem có bị spam không (dựa trên Redis key đã tồn tại chưa)
        /// </summary>
        public async Task<bool> IsSpamAsync(string keyPrefix, TimeSpan window, CancellationToken cancellationToken = default)
        {
            var compositeKey = $"{keyPrefix}:{GetEndpointKey()}:{GetClientIdentifier()}";

            try
            {
                var value = await _cache.GetStringAsync(compositeKey, cancellationToken);
                return !string.IsNullOrEmpty(value);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "[SpamProtection] Redis unavailable in IsSpamAsync – fallback to allow. Key: {Key}", compositeKey);
                return false; // fallback: không chặn
            }
        }

        /// <summary>
        /// Đánh dấu spam theo TTL để chặn tiếp tục gọi trong khoảng thời gian
        /// </summary>
        public async Task MarkAsync(string keyPrefix, TimeSpan window, CancellationToken cancellationToken = default)
        {
            var compositeKey = $"{keyPrefix}:{GetEndpointKey()}:{GetClientIdentifier()}";

            try
            {
                var options = new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = window
                };
                await _cache.SetStringAsync(compositeKey, "1", options, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "[SpamProtection] Redis unavailable in MarkAsync – skip write. Key: {Key}", compositeKey);
                // fallback: không ghi vào Redis nhưng hệ thống không bị crash
            }
        }
    }
}

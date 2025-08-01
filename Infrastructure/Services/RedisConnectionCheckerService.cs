using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    /// <summary>
    /// Dịch vụ kiểm tra Redis có khả dụng hay không khi khởi động
    /// </summary>
    public class RedisConnectionCheckerService : IHostedService
    {
        private readonly IDistributedCache _cache;
        private readonly ILogger<RedisConnectionCheckerService> _logger;

        public RedisConnectionCheckerService(IDistributedCache cache, ILogger<RedisConnectionCheckerService> logger)
        {
            _cache = cache;
            _logger = logger;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            try
            {
                var key = $"redis-check:{Guid.NewGuid()}";
                await _cache.SetStringAsync(key, "1", new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(10)
                }, cancellationToken);

                _logger.LogInformation("✅ Redis connected and operational.");
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "⚠️ Redis connection check failed. Anti-spam protection will fallback to memory.");
            }
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}

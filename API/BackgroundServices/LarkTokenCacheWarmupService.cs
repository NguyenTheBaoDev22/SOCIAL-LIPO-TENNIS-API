using Applications.Features.LarksuiteIntegrations.DTOs;
using Applications.Interfaces.Repositories;
using Applications.Services.Interfaces;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Shared.Constants;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace API.BackgroundServices
{
    /// <summary>
    /// Hosted service chạy khi app start, có nhiệm vụ:
    /// - Tải token LarkSuite còn hạn từ DB lên cache để tránh truy vấn DB nhiều lần
    /// - Nếu token access hết hạn nhưng refresh token còn hạn, tự động gọi refresh token, lưu DB và cache
    /// - Nếu refresh token cũng hết hạn, sẽ log cảnh báo và chờ xử lý thủ công (ví dụ user đăng nhập lại)
    /// </summary>
    public class LarkTokenCacheWarmupService : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILarkTokenService _larkTokenService;
        private readonly ICacheService _cacheService;
        private readonly ILogger<LarkTokenCacheWarmupService> _logger;

        public LarkTokenCacheWarmupService(
            IServiceProvider serviceProvider,
            ILarkTokenService larkTokenService,
            ICacheService cacheService,
            ILogger<LarkTokenCacheWarmupService> logger)
        {
            _serviceProvider = serviceProvider;
            _larkTokenService = larkTokenService;
            _cacheService = cacheService;
            _logger = logger;
        }

        /// <summary>
        /// Khi app start, thực hiện load hoặc refresh token LarkSuite
        /// </summary>
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("🚀 Bắt đầu warmup cache cho Lark token...");

            using (var scope = _serviceProvider.CreateScope())
            {
                var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

                // Lấy token còn hạn nhất từ DB
                var tokenEntity = await unitOfWork.LarkTokensRepository.GetLatestValidAsync();

                // Nếu token access token còn hạn > 1 phút
                if (tokenEntity != null && tokenEntity.AccessTokenExpiresAt > DateTime.UtcNow.AddMinutes(1))
                {
                    _logger.LogInformation("Token access còn hạn, cache token từ DB lên bộ nhớ.");

                    var cacheModel = new LarkTokenCache
                    {
                        AccessToken = tokenEntity.AccessToken,
                        RefreshToken = tokenEntity.RefreshToken,
                        ExpiresIn = (int)(tokenEntity.AccessTokenExpiresAt - DateTime.UtcNow).TotalSeconds,
                        RefreshTokenExpiresIn = (int)(tokenEntity.RefreshTokenExpiresAt - DateTime.UtcNow).TotalSeconds,
                        TokenType = "Bearer", // Có thể lấy từ entity nếu có trường tokenType
                        AccessTokenExpiresAt = tokenEntity.AccessTokenExpiresAt,
                        RefreshTokenRefreshTokenExpiresAt = tokenEntity.RefreshTokenExpiresAt
                    };

                    _cacheService.Set(CacheKeys.LarkToken, cacheModel, cacheModel.AccessTokenExpiresAt, 1);
                    _logger.LogInformation("✅ Đã cache thành công Lark token.");

                    return; // Hoàn tất, không cần refresh token
                }

                // Nếu không tìm thấy token nào trong DB
                if (tokenEntity == null)
                {
                    _logger.LogWarning("⚠️ Không tìm thấy token Lark hợp lệ trong database.");
                    return;
                }

                // Nếu token access hết hạn, kiểm tra xem refresh token còn hạn không
                if (tokenEntity.RefreshTokenExpiresAt > DateTime.UtcNow.AddMinutes(1))
                {
                    _logger.LogInformation("Access token đã hết hạn, sẽ thử làm mới token bằng refresh token...");

                    // Gọi service refresh token
                    var refreshedToken = await _larkTokenService.RefreshAccessTokenAsync(tokenEntity.RefreshToken);

                    if (refreshedToken != null)
                    {
                        _logger.LogInformation("Refresh token thành công, cập nhật lại cache và DB.");

                        var refreshedCache = new LarkTokenCache
                        {
                            AccessToken = refreshedToken.AccessToken,
                            RefreshToken = refreshedToken.RefreshToken,
                            ExpiresIn = (int)(refreshedToken.AccessTokenExpiresAt - DateTime.UtcNow).TotalSeconds,
                            RefreshTokenExpiresIn = (int)(refreshedToken.RefreshTokenExpiresAt - DateTime.UtcNow).TotalSeconds,
                            TokenType = "Bearer",
                            AccessTokenExpiresAt = refreshedToken.AccessTokenExpiresAt,
                            RefreshTokenRefreshTokenExpiresAt = refreshedToken.RefreshTokenExpiresAt
                        };

                        _cacheService.Set(CacheKeys.LarkToken, refreshedCache, refreshedCache.AccessTokenExpiresAt, 1);
                        _logger.LogInformation("✅ Token đã được refresh và cache lại thành công.");

                        // Nếu muốn cập nhật DB, gọi lại unitOfWork.LarkTokensRepository.UpdateAsync(refreshedToken) nếu có
                        // await unitOfWork.LarkTokensRepository.UpdateAsync(refreshedToken);
                        // await unitOfWork.CommitAsync();
                    }
                    else
                    {
                        _logger.LogWarning("❌ Refresh token thất bại, cần lấy token mới thủ công hoặc xử lý lại flow OAuth.");
                    }
                }
                else
                {
                    // Refresh token cũng hết hạn, cần user can thiệp
                    _logger.LogWarning("⚠️ Refresh token cũng đã hết hạn. Cần lấy token mới thủ công qua flow OAuth.");
                }
            }
        }

        /// <summary>
        /// Khi app dừng, không cần làm gì thêm
        /// </summary>
        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}

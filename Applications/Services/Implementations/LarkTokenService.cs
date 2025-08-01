//using Amazon.Runtime.Internal.Util;
//using Applications.Features.LarksuiteIntegrations.DTOs;
//using Applications.Interfaces.Repositories;
//using Applications.Services.Interfaces;
//using Core.Entities;
//using Microsoft.Extensions.Caching.Memory;
//using Microsoft.Extensions.Logging;
//using Shared.Configs;
//using Shared.Constants;
//using Shared.Interfaces;
//using System;
//using System.Text;
//using System.Text.Json;
//using System.Threading.Tasks;

//namespace Applications.Services.Implementations
//{
//    public class LarkTokenService : ILarkTokenService
//    {
//        private readonly HttpClient _httpClient;
//        private readonly ILogger<LarkTokenService> _logger;
//        private readonly ILarkSuiteConfig _config;
//        private readonly IUnitOfWork _unitOfWork;
//        private readonly ICurrentUserService _currentUser;
//        private readonly ICacheService _cacheService;
//        private const string CacheKey = CacheKeys.LarkToken;
//        public LarkTokenService(
//            HttpClient httpClient,
//            ILogger<LarkTokenService> logger,
//            ILarkSuiteConfig config,
//            IUnitOfWork unitOfWork,
//            ICurrentUserService currentUser,
//             ICacheService cacheService)
//        {
//            _httpClient = httpClient;
//            _logger = logger;
//            _config = config;
//            _unitOfWork = unitOfWork;
//            _currentUser = currentUser;
//            _cacheService = cacheService;
//        }

//        /// <summary>
//        /// Lấy access token từ Lark và lưu xuống database.
//        /// </summary>
//        public async Task<LarkTokens?> GetAccessTokenAsync(string code, string state)
//        {
//            var requestBody = new
//            {
//                grant_type = "authorization_code",
//                code = code,
//                app_id = _config.AppId,
//                app_secret = _config.AppSecret
//            };

//            var content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");
//            try
//            {
//                var response = await _httpClient.PostAsync(_config.TokenEndpoint, content);
//                response.EnsureSuccessStatusCode();

//                var responseContent = await response.Content.ReadAsStringAsync();
//                var tokenResponse = JsonSerializer.Deserialize<JsonElement>(responseContent);

//                if (tokenResponse.TryGetProperty("data", out var data))
//                {
//                    var accessToken = data.GetProperty("access_token").GetString();
//                    var refreshToken = data.GetProperty("refresh_token").GetString();
//                    var expiresIn = data.GetProperty("expires_in").GetInt32();
//                    var refreshTokenExpiresIn = data.GetProperty("refresh_expires_in").GetInt32();
//                    var tokenType = data.GetProperty("token_type").GetString();

//                    // Tính toán thời gian hết hạn token
//                    var now = DateTime.UtcNow;
//                    var accessTokenExpiresAt = now.AddSeconds(expiresIn);
//                    var refreshTokenExpiresAt = now.AddSeconds(refreshTokenExpiresIn);

//                    // Tạo entity lưu vào DB
//                    var larkToken = new LarkTokens
//                    {
//                        AccessToken = accessToken!,
//                        RefreshToken = refreshToken!,
//                        AccessTokenExpiresAt = accessTokenExpiresAt,
//                        RefreshTokenExpiresAt = refreshTokenExpiresAt,
//                        Status = "Active",
//                        RequestTime = now,
//                        RawData = responseContent,

//                        // Audit fields
//                        CreatedAt = now,
//                        CreatedBy = _currentUser.UserId,
//                        IpAddress = _currentUser.IpAddress,
//                        TraceId = _currentUser.TraceId
//                    };

//                    // Lưu vào DB
//                    await _unitOfWork.LarkTokensRepository.AddAsync(larkToken);
//                    await _unitOfWork.SaveChangesAsync();
//                    _logger.LogInformation("✅ Đã lấy và lưu access token thành công");
//                    // Tạo cache object
//                    var larkCache = new LarkTokenCache
//                    {
//                        AccessToken = accessToken!,
//                        RefreshToken = refreshToken!,
//                        ExpiresIn = expiresIn,
//                        RefreshTokenExpiresIn = refreshTokenExpiresIn,
//                        TokenType = tokenType!,
//                        AccessTokenExpiresAt = accessTokenExpiresAt,
//                        RefreshTokenRefreshTokenExpiresAt = refreshTokenExpiresAt
//                    };
//                    // Đặt key cache, ví dụ nếu nhiều tenant/user thì truyền thêm userId hoặc merchantId
//                    //_cache.Set("some_key", someObj, new MemoryCacheEntryOptions
//                    //{
//                    //    AbsoluteExpiration = ...,
//                    //    Size = 1 // mỗi item size 1 đơn vị, hoặc tính MB
//                    //});
//                    _cacheService.Set(
//                    CacheKeys.LarkToken,
//                    larkCache,
//                    accessTokenExpiresAt,1 // Set expire đúng thời điểm access token hết hạn
//                        );
//                    _logger.LogInformation("✅ Đã lấy và lưu access token thành công cho user {UserId}", _currentUser.UserId);

//                    return larkToken; // Nếu trả về ra ngoài, hãy map sang DTO che thông tin nhạy cảm!
//                }

//                _logger.LogError("Không tìm thấy data trong response: {Response}", responseContent);
//                return null;
//            }
//            catch (HttpRequestException ex)
//            {
//                _logger.LogError(ex, "HTTP error khi lấy token từ Lark");
//                return null;
//            }
//            catch (JsonException ex)
//            {
//                _logger.LogError(ex, "JSON error khi parse token từ Lark");
//                return null;
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Lỗi không xác định khi lấy/lưu token");
//                return null;
//            }
//        }
//        /// <summary>
//        /// Làm mới access token với refresh token, có thể bổ sung lưu DB nếu muốn.
//        /// </summary>
//        public async Task<LarkTokens?> RefreshAccessTokenAsync(string refreshToken)
//        {
//            if (string.IsNullOrEmpty(refreshToken))
//            {
//                _logger.LogError("Refresh token is missing.");
//                return null;
//            }

//            try
//            {
//                // 1. Lấy App Access Token
//                var appTokenRequest = new
//                {
//                    app_id = _config.AppId,
//                    app_secret = _config.AppSecret
//                };
//                var appTokenContent = new StringContent(JsonSerializer.Serialize(appTokenRequest), Encoding.UTF8, "application/json");
//                var appTokenResponse = await _httpClient.PostAsync("https://open.larksuite.com/open-apis/auth/v3/app_access_token/internal/", appTokenContent);
//                appTokenResponse.EnsureSuccessStatusCode();
//                var appTokenResponseContent = await appTokenResponse.Content.ReadAsStringAsync();
//                var appTokenJson = JsonDocument.Parse(appTokenResponseContent);

//                if (!appTokenJson.RootElement.TryGetProperty("app_access_token", out var appAccessTokenElement))
//                {
//                    _logger.LogError("Không lấy được app_access_token từ response: {Response}", appTokenResponseContent);
//                    return null;
//                }
//                var appAccessToken = appAccessTokenElement.GetString();

//                // 2. Gọi API refresh token với app_access_token làm Bearer token
//                var refreshRequestBody = new
//                {
//                    grant_type = "refresh_token",
//                    refresh_token = refreshToken
//                };
//                var refreshContent = new StringContent(JsonSerializer.Serialize(refreshRequestBody), Encoding.UTF8, "application/json");

//                var refreshRequest = new HttpRequestMessage(HttpMethod.Post, _config.RefreshTokenEndpoint)
//                {
//                    Content = refreshContent
//                };
//                refreshRequest.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", appAccessToken);

//                var refreshResponse = await _httpClient.SendAsync(refreshRequest);
//                refreshResponse.EnsureSuccessStatusCode();

//                var refreshResponseContent = await refreshResponse.Content.ReadAsStringAsync();
//                var refreshJson = JsonDocument.Parse(refreshResponseContent);

//                if (!refreshJson.RootElement.TryGetProperty("data", out var data))
//                {
//                    _logger.LogError("Không parse được data khi refresh token: {Response}", refreshResponseContent);
//                    return null;
//                }

//                var newAccessToken = data.GetProperty("access_token").GetString();
//                var newRefreshToken = data.GetProperty("refresh_token").GetString();
//                var expiresIn = data.GetProperty("expires_in").GetInt32();
//                var refreshTokenExpiresIn = data.GetProperty("refresh_expires_in").GetInt32();
//                var tokenType = data.GetProperty("token_type").GetString();

//                var now = DateTime.UtcNow;
//                var accessTokenExpiresAt = now.AddSeconds(expiresIn);
//                var refreshTokenExpiresAt = now.AddSeconds(refreshTokenExpiresIn);

//                var larkToken = new LarkTokens
//                {
//                    AccessToken = newAccessToken!,
//                    RefreshToken = newRefreshToken!,
//                    AccessTokenExpiresAt = accessTokenExpiresAt,
//                    RefreshTokenExpiresAt = refreshTokenExpiresAt,
//                    Status = "Active",
//                    RequestTime = now,
//                    RawData = refreshResponseContent,
//                    CreatedAt = now,
//                    CreatedBy = _currentUser.UserId,
//                    IpAddress = _currentUser.IpAddress,
//                    TraceId = _currentUser.TraceId
//                };

//                await _unitOfWork.LarkTokensRepository.AddAsync(larkToken);
//                await _unitOfWork.SaveChangesAsync();

//                // Cập nhật cache
//                var cacheModel = MapToCache(larkToken);
//                _cacheService.Set(CacheKeys.LarkToken, cacheModel, cacheModel.AccessTokenExpiresAt, 1);

//                _logger.LogInformation("✅ Đã refresh và lưu access token thành công cho user {UserId}", _currentUser.UserId);

//                return larkToken;
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Lỗi khi refresh token");
//                return null;
//            }
//        }


//        /// <summary>
//        /// Lấy token từ cache hoặc DB, nếu hết hạn (hoặc gần hết hạn) thì tự động làm mới token.
//        /// </summary>
//        public async Task<LarkTokenCache?> GetValidTokenAsync()
//        {
//            // 1. Lấy token từ cache
//            var cachedToken = _cacheService.Get<LarkTokenCache>(CacheKey);
//            if (cachedToken != null)
//            {
//                if (cachedToken.AccessTokenExpiresAt > DateTime.UtcNow.AddMinutes(1))
//                {
//                    _logger.LogInformation("Lấy token từ cache hợp lệ.");
//                    return cachedToken;
//                }
//                _logger.LogInformation("Token cache sắp hết hạn hoặc đã hết hạn, cần làm mới.");
//            }

//            // 2. Lấy token mới nhất hợp lệ từ DB
//            var tokenEntity = await _unitOfWork.LarkTokensRepository.GetLatestValidAsync();

//            if (tokenEntity != null)
//            {
//                if (tokenEntity.AccessTokenExpiresAt > DateTime.UtcNow.AddMinutes(1))
//                {
//                    var cacheModel = MapToCache(tokenEntity);
//                    _cacheService.Set(CacheKey, cacheModel, cacheModel.AccessTokenExpiresAt, 1);
//                    _logger.LogInformation("Lấy token từ DB hợp lệ và cache lại.");
//                    return cacheModel;
//                }

//                // 3. Access token hết hạn, kiểm tra refresh token còn hạn không
//                if (tokenEntity.RefreshTokenExpiresAt > DateTime.UtcNow.AddMinutes(1))
//                {
//                    var refreshedTokenEntity = await RefreshAccessTokenAsync(tokenEntity.RefreshToken);
//                    if (refreshedTokenEntity != null)
//                    {
//                        var refreshedCache = MapToCache(refreshedTokenEntity);
//                        _cacheService.Set(CacheKey, refreshedCache, refreshedCache.AccessTokenExpiresAt, 1);
//                        _logger.LogInformation("Đã làm mới token và cache lại.");
//                        return refreshedCache;
//                    }
//                    else
//                    {
//                        _logger.LogWarning("Làm mới token thất bại.");
//                        return null;
//                    }
//                }
//                else
//                {
//                    _logger.LogWarning("Refresh token cũng đã hết hạn.");
//                    return null;
//                }
//            }

//            _logger.LogWarning("Không có token hợp lệ để trả về.");
//            return null;
//        }


//        /// <summary>
//        /// Map từ entity sang cache model
//        /// </summary>
//        private LarkTokenCache MapToCache(LarkTokens entity)
//        {
//            return new LarkTokenCache
//            {
//                AccessToken = entity.AccessToken,
//                RefreshToken = entity.RefreshToken,
//                ExpiresIn = (int)(entity.AccessTokenExpiresAt - DateTime.UtcNow).TotalSeconds,
//                RefreshTokenExpiresIn = (int)(entity.RefreshTokenExpiresAt - DateTime.UtcNow).TotalSeconds,
//                TokenType = "Bearer", // hoặc lưu trong entity nếu có
//                AccessTokenExpiresAt = entity.AccessTokenExpiresAt,
//                RefreshTokenRefreshTokenExpiresAt = entity.RefreshTokenExpiresAt
//            };
//        }

//    }
//}


























using Amazon.Runtime.Internal.Util;
using Applications.Features.LarksuiteIntegrations.DTOs;
using Applications.Interfaces.Repositories;
using Applications.Services.Interfaces;
using Core.Entities;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Shared.Configs;
using Shared.Constants;
using Shared.Interfaces;
using System;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Applications.Services.Implementations
{
    public class LarkTokenService : ILarkTokenService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<LarkTokenService> _logger;
        private readonly ILarkSuiteConfig _config;
        private readonly ICurrentUserService _currentUser;
        private readonly ICacheService _cacheService;
        private readonly IServiceProvider _serviceProvider; // NEW
        private const string CacheKey = CacheKeys.LarkToken;

        public LarkTokenService(
            HttpClient httpClient,
            ILogger<LarkTokenService> logger,
            ILarkSuiteConfig config,
            ICurrentUserService currentUser,
            ICacheService cacheService,
            IServiceProvider serviceProvider // NEW
            )
        {
            _httpClient = httpClient;
            _logger = logger;
            _config = config;
            _currentUser = currentUser;
            _cacheService = cacheService;
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Lấy access token từ Lark và lưu xuống database.
        /// </summary>
        public async Task<LarkTokens?> GetAccessTokenAsync(string code, string state)
        {
            var requestBody = new
            {
                grant_type = "authorization_code",
                code = code,
                app_id = _config.AppId,
                app_secret = _config.AppSecret
            };

            var content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");
            try
            {
                var response = await _httpClient.PostAsync(_config.TokenEndpoint, content);
                response.EnsureSuccessStatusCode();

                var responseContent = await response.Content.ReadAsStringAsync();
                var tokenResponse = JsonSerializer.Deserialize<JsonElement>(responseContent);

                if (tokenResponse.TryGetProperty("data", out var data))
                {
                    var accessToken = data.GetProperty("access_token").GetString();
                    var refreshToken = data.GetProperty("refresh_token").GetString();
                    var expiresIn = data.GetProperty("expires_in").GetInt32();
                    var refreshTokenExpiresIn = data.GetProperty("refresh_expires_in").GetInt32();
                    var tokenType = data.GetProperty("token_type").GetString();

                    // Tính toán thời gian hết hạn token
                    var now = DateTime.UtcNow;
                    var accessTokenExpiresAt = now.AddSeconds(expiresIn);
                    var refreshTokenExpiresAt = now.AddSeconds(refreshTokenExpiresIn);

                    // Tạo entity lưu vào DB
                    var larkToken = new LarkTokens
                    {
                        AccessToken = accessToken!,
                        RefreshToken = refreshToken!,
                        AccessTokenExpiresAt = accessTokenExpiresAt,
                        RefreshTokenExpiresAt = refreshTokenExpiresAt,
                        Status = "Active",
                        RequestTime = now,
                        RawData = responseContent,

                        // Audit fields
                        CreatedAt = now,
                        CreatedBy = _currentUser.UserId,
                        IpAddress = _currentUser.IpAddress,
                        TraceId = _currentUser.TraceId
                    };

                    // Lưu vào DB (scope đúng chuẩn)
                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
                        await unitOfWork.LarkTokensRepository.AddAsync(larkToken);
                        await unitOfWork.SaveChangesAsync();
                    }
                    _logger.LogInformation("✅ Đã lấy và lưu access token thành công");

                    // Tạo cache object
                    var larkCache = new LarkTokenCache
                    {
                        AccessToken = accessToken!,
                        RefreshToken = refreshToken!,
                        ExpiresIn = expiresIn,
                        RefreshTokenExpiresIn = refreshTokenExpiresIn,
                        TokenType = tokenType!,
                        AccessTokenExpiresAt = accessTokenExpiresAt,
                        RefreshTokenRefreshTokenExpiresAt = refreshTokenExpiresAt
                    };

                    _cacheService.Set(
                        CacheKeys.LarkToken,
                        larkCache,
                        accessTokenExpiresAt, 1 // Set expire đúng thời điểm access token hết hạn
                    );
                    _logger.LogInformation("✅ Đã lấy và lưu access token thành công cho user {UserId}", _currentUser.UserId);

                    return larkToken; // Nếu trả về ra ngoài, hãy map sang DTO che thông tin nhạy cảm!
                }

                _logger.LogError("Không tìm thấy data trong response: {Response}", responseContent);
                return null;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "HTTP error khi lấy token từ Lark");
                return null;
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "JSON error khi parse token từ Lark");
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi không xác định khi lấy/lưu token");
                return null;
            }
        }

        /// <summary>
        /// Làm mới access token với refresh token, có thể bổ sung lưu DB nếu muốn.
        /// </summary>
        public async Task<LarkTokens?> RefreshAccessTokenAsync(string refreshToken)
        {
            if (string.IsNullOrEmpty(refreshToken))
            {
                _logger.LogError("Refresh token is missing.");
                return null;
            }

            try
            {
                // 1. Lấy App Access Token
                var appTokenRequest = new
                {
                    app_id = _config.AppId,
                    app_secret = _config.AppSecret
                };
                var appTokenContent = new StringContent(JsonSerializer.Serialize(appTokenRequest), Encoding.UTF8, "application/json");
                var appTokenResponse = await _httpClient.PostAsync("https://open.larksuite.com/open-apis/auth/v3/app_access_token/internal/", appTokenContent);
                appTokenResponse.EnsureSuccessStatusCode();
                var appTokenResponseContent = await appTokenResponse.Content.ReadAsStringAsync();
                var appTokenJson = JsonDocument.Parse(appTokenResponseContent);

                if (!appTokenJson.RootElement.TryGetProperty("app_access_token", out var appAccessTokenElement))
                {
                    _logger.LogError("Không lấy được app_access_token từ response: {Response}", appTokenResponseContent);
                    return null;
                }
                var appAccessToken = appAccessTokenElement.GetString();

                // 2. Gọi API refresh token với app_access_token làm Bearer token
                var refreshRequestBody = new
                {
                    grant_type = "refresh_token",
                    refresh_token = refreshToken
                };
                var refreshContent = new StringContent(JsonSerializer.Serialize(refreshRequestBody), Encoding.UTF8, "application/json");

                var refreshRequest = new HttpRequestMessage(HttpMethod.Post, _config.RefreshTokenEndpoint)
                {
                    Content = refreshContent
                };
                refreshRequest.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", appAccessToken);

                var refreshResponse = await _httpClient.SendAsync(refreshRequest);
                refreshResponse.EnsureSuccessStatusCode();

                var refreshResponseContent = await refreshResponse.Content.ReadAsStringAsync();
                var refreshJson = JsonDocument.Parse(refreshResponseContent);

                if (!refreshJson.RootElement.TryGetProperty("data", out var data))
                {
                    _logger.LogError("Không parse được data khi refresh token: {Response}", refreshResponseContent);
                    return null;
                }

                var newAccessToken = data.GetProperty("access_token").GetString();
                var newRefreshToken = data.GetProperty("refresh_token").GetString();
                var expiresIn = data.GetProperty("expires_in").GetInt32();
                var refreshTokenExpiresIn = data.GetProperty("refresh_expires_in").GetInt32();
                var tokenType = data.GetProperty("token_type").GetString();

                var now = DateTime.UtcNow;
                var accessTokenExpiresAt = now.AddSeconds(expiresIn);
                var refreshTokenExpiresAt = now.AddSeconds(refreshTokenExpiresIn);

                var larkToken = new LarkTokens
                {
                    AccessToken = newAccessToken!,
                    RefreshToken = newRefreshToken!,
                    AccessTokenExpiresAt = accessTokenExpiresAt,
                    RefreshTokenExpiresAt = refreshTokenExpiresAt,
                    Status = "Active",
                    RequestTime = now,
                    RawData = refreshResponseContent,
                    CreatedAt = now,
                    CreatedBy = _currentUser.UserId,
                    IpAddress = _currentUser.IpAddress,
                    TraceId = _currentUser.TraceId
                };

                // Lưu vào DB (scope đúng chuẩn)
                using (var scope = _serviceProvider.CreateScope())
                {
                    var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
                    await unitOfWork.LarkTokensRepository.AddAsync(larkToken);
                    await unitOfWork.SaveChangesAsync();
                }

                // Cập nhật cache
                var cacheModel = MapToCache(larkToken);
                _cacheService.Set(CacheKeys.LarkToken, cacheModel, cacheModel.AccessTokenExpiresAt, 1);

                _logger.LogInformation("✅ Đã refresh và lưu access token thành công cho user {UserId}", _currentUser.UserId);

                return larkToken;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi refresh token");
                return null;
            }
        }

        /// <summary>
        /// Lấy token từ cache hoặc DB, nếu hết hạn (hoặc gần hết hạn) thì tự động làm mới token.
        /// </summary>
        public async Task<LarkTokenCache?> GetValidTokenAsync()
        {
            // 1. Lấy token từ cache
            var cachedToken = _cacheService.Get<LarkTokenCache>(CacheKey);
            if (cachedToken != null)
            {
                if (cachedToken.AccessTokenExpiresAt > DateTime.UtcNow.AddMinutes(1))
                {
                    _logger.LogInformation("Lấy token từ cache hợp lệ.");
                    return cachedToken;
                }
                _logger.LogInformation("Token cache sắp hết hạn hoặc đã hết hạn, cần làm mới.");
            }

            // 2. Lấy token mới nhất hợp lệ từ DB
            using (var scope = _serviceProvider.CreateScope())
            {
                var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
                var tokenEntity = await unitOfWork.LarkTokensRepository.GetLatestValidAsync();

                if (tokenEntity != null)
                {
                    if (tokenEntity.AccessTokenExpiresAt > DateTime.UtcNow.AddMinutes(1))
                    {
                        var cacheModel = MapToCache(tokenEntity);
                        _cacheService.Set(CacheKey, cacheModel, cacheModel.AccessTokenExpiresAt, 1);
                        _logger.LogInformation("Lấy token từ DB hợp lệ và cache lại.");
                        return cacheModel;
                    }

                    // 3. Access token hết hạn, kiểm tra refresh token còn hạn không
                    if (tokenEntity.RefreshTokenExpiresAt > DateTime.UtcNow.AddMinutes(1))
                    {
                        var refreshedTokenEntity = await RefreshAccessTokenAsync(tokenEntity.RefreshToken);
                        if (refreshedTokenEntity != null)
                        {
                            var refreshedCache = MapToCache(refreshedTokenEntity);
                            _cacheService.Set(CacheKey, refreshedCache, refreshedCache.AccessTokenExpiresAt, 1);
                            _logger.LogInformation("Đã làm mới token và cache lại.");
                            return refreshedCache;
                        }
                        else
                        {
                            _logger.LogWarning("Làm mới token thất bại.");
                            return null;
                        }
                    }
                    else
                    {
                        _logger.LogWarning("Refresh token cũng đã hết hạn.");
                        return null;
                    }
                }
            }

            _logger.LogWarning("Không có token hợp lệ để trả về.");
            return null;
        }

        /// <summary>
        /// Map từ entity sang cache model
        /// </summary>
        private LarkTokenCache MapToCache(LarkTokens entity)
        {
            return new LarkTokenCache
            {
                AccessToken = entity.AccessToken,
                RefreshToken = entity.RefreshToken,
                ExpiresIn = (int)(entity.AccessTokenExpiresAt - DateTime.UtcNow).TotalSeconds,
                RefreshTokenExpiresIn = (int)(entity.RefreshTokenExpiresAt - DateTime.UtcNow).TotalSeconds,
                TokenType = "Bearer", // hoặc lưu trong entity nếu có
                AccessTokenExpiresAt = entity.AccessTokenExpiresAt,
                RefreshTokenRefreshTokenExpiresAt = entity.RefreshTokenExpiresAt
            };
        }
    }
}

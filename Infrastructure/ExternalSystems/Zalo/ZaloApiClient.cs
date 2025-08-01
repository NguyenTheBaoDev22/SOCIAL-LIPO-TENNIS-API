using Applications.Features.ZaloIntegrations.ZaloAuth.DTOs;
using Applications.Services.Interfaces;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Serilog;
using Shared.Configs;
using Shared.Results;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.ExternalSystems.Zalo
{
    public class ZaloApiClient : IZaloApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly ZaloConfig _config;

        public ZaloApiClient(HttpClient httpClient, IOptions<ZaloConfig> config)
        {
            _httpClient = httpClient;
            _config = config.Value;
        }

        public async Task<BaseResponse<ZaloPhoneNumberData>> GetPhoneNumberAsync(
            string accessToken, string phoneTokenCode, string? inputTraceId = null, CancellationToken cancellationToken = default)
        {
            var traceId = string.IsNullOrWhiteSpace(inputTraceId) ? Guid.NewGuid().ToString("N") : inputTraceId;
            var url = $"{_config.BaseUrl}/me/info";

            try
            {
                using var request = new HttpRequestMessage(HttpMethod.Get, url);

                // Thêm các header bắt buộc theo tài liệu
                request.Headers.Add("access_token", accessToken);
                request.Headers.Add("code", phoneTokenCode);
                request.Headers.Add("secret_key", _config.AppSecretKey);

                Log.Information("TraceId: {TraceId} - Zalo Get Phone Number request to {Url}", traceId, url);

                var response = await _httpClient.SendAsync(request, cancellationToken);
                response.EnsureSuccessStatusCode();

                // Đọc response ra chuỗi và deserialize bằng Newtonsoft.Json
                var jsonString = await response.Content.ReadAsStringAsync();
                var parsed = JsonConvert.DeserializeObject<ZaloBaseResponse<ZaloPhoneNumberData>>(jsonString);

                // Kiểm tra lỗi logic dựa trên trường "error"
                if (parsed?.Error != 0)
                {
                    Log.Warning(
                        "TraceId: {TraceId} - Zalo API returned an error. Code: {ErrorCode}, Message: {ErrorMessage}",
                        traceId, parsed.Error, parsed.Message);
                    return BaseResponse<ZaloPhoneNumberData>.Error($"ZALO_API_ERROR_{parsed.Error}", parsed.Message, traceId);
                }

                // Log thành công và trả về data
                Log.Information("TraceId: {TraceId} - Successfully retrieved phone number info from Zalo.", traceId);
                return BaseResponse<ZaloPhoneNumberData>.Success(parsed.Data, traceId);
            }
            catch (HttpRequestException ex)
            {
                Log.Error(ex, "TraceId: {TraceId} - HTTP request to Zalo API failed.", traceId);
                return BaseResponse<ZaloPhoneNumberData>.Error("ZALO_HTTP_ERROR", $"Lỗi mạng khi gọi Zalo: {ex.Message}", traceId);
            }
            catch (JsonException ex) // Bắt exception từ Newtonsoft.Json
            {
                Log.Error(ex, "TraceId: {TraceId} - Failed to deserialize Zalo API response.", traceId);
                return BaseResponse<ZaloPhoneNumberData>.Error("ZALO_DESERIALIZE_ERROR", "Không thể phân tích phản hồi từ Zalo.", traceId);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "TraceId: {TraceId} - An unexpected exception occurred when calling Zalo API.", traceId);
                return BaseResponse<ZaloPhoneNumberData>.Error("ZALO_UNEXPECTED_EXCEPTION", ex.Message, traceId);
            }
        }

        public async Task<BaseResponse<LocationResultDto>> GetLocationAsync(
    string accessToken, string locationToken, string? traceId = null, CancellationToken cancellationToken = default)
        {
            traceId ??= Guid.NewGuid().ToString("N");
            var url = $"{_config.BaseUrl}/me/info";

            try
            {
                using var request = new HttpRequestMessage(HttpMethod.Get, url);
                request.Headers.Add("access_token", accessToken);
                request.Headers.Add("code", locationToken);
                request.Headers.Add("secret_key", _config.AppSecretKey);

                Log.Information("TraceId: {TraceId} - Zalo Get Location request to {Url}", traceId, url);
                var response = await _httpClient.SendAsync(request, cancellationToken);
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                var parsed = JsonConvert.DeserializeObject<ZaloBaseResponse<dynamic>>(json);

                if (parsed?.Error != 0)
                {
                    Log.Warning("TraceId: {TraceId} - Zalo error {Code}: {Message}", traceId, parsed.Error, parsed.Message);
                    return BaseResponse<LocationResultDto>.Error($"ZALO_API_ERROR_{parsed.Error}", parsed.Message, traceId);
                }

                var data = parsed.Data;
                var location = new LocationResultDto
                {
                    Provider = data.provider,
                    Latitude = double.Parse((string)data.latitude),
                    Longitude = double.Parse((string)data.longitude),
                    Timestamp = long.Parse((string)data.timestamp)
                };

                Log.Information("TraceId: {TraceId} - Zalo location retrieved {@Location}", traceId, location);
                return BaseResponse<LocationResultDto>.Success(location, traceId);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "TraceId: {TraceId} - Error getting location from Zalo", traceId);
                return BaseResponse<LocationResultDto>.Error("ZALO_LOCATION_FAILED", ex.Message, traceId);
            }
        }

    }
}
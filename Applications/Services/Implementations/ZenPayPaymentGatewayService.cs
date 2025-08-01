using Applications.DTOs;
using Applications.Features.Payments.QRCodes.Commands;
using Applications.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Serilog;
using Shared.Results;
using System.Net.Http.Headers;
using System.Text;

namespace Applications.Services.Implementations;

/// <summary>
/// Service gọi đến hệ thống Payment Gateway (PG) của ZenPay để tạo mã QR.
/// </summary>
public class ZenPayPaymentGatewayService : IPaymentGatewayService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private const string Endpoint = "https://uat-tingting-api.zenpay.com.vn/generate-qr";

    public ZenPayPaymentGatewayService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    /// <summary>
    /// Gửi yêu cầu sinh mã QR đến hệ thống Payment Gateway và trả về QR content.
    /// </summary>
    /// <param name="request">Thông tin cần thiết để tạo mã QR</param>
    /// <param name="cancellationToken">Token hỗ trợ hủy bỏ</param>
    /// <returns>Chuỗi QR đã sinh ra từ PG</returns>
    public async Task<string> GenerateQrAsync(PaymentGatewayQrRequest request, CancellationToken cancellationToken)
    {
        var traceId = Guid.NewGuid().ToString("N");
        var client = _httpClientFactory.CreateClient();

        var jsonPayload = JsonConvert.SerializeObject(request);
        var httpRequest = new HttpRequestMessage(HttpMethod.Post, Endpoint)
        {
            Content = new StringContent(jsonPayload, Encoding.UTF8, "application/json")
        };
        httpRequest.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("*/*"));

        Log.Information("[{TraceId}] 📤 Đang gửi yêu cầu sinh QR đến PG: {Endpoint}", traceId, Endpoint);
        Log.Debug("[{TraceId}] ➡️ Payload gửi đi: {Payload}", traceId, jsonPayload);

        try
        {
            var response = await client.SendAsync(httpRequest, cancellationToken);
            var responseBody = await response.Content.ReadAsStringAsync(cancellationToken);

            Log.Information("[{TraceId}] 📥 Nhận phản hồi từ PG với StatusCode: {StatusCode}", traceId, response.StatusCode);
            Log.Debug("[{TraceId}] ⬅️ Nội dung phản hồi: {Body}", traceId, responseBody);

            if (!response.IsSuccessStatusCode)
                throw new InvalidOperationException($"PG trả về lỗi {response.StatusCode}: {responseBody}");

            var parsed = JsonConvert.DeserializeObject<BaseResponse<string>>(responseBody);

            if (parsed == null || parsed.Code != "00")
                throw new InvalidOperationException($"Lỗi tạo QR: {parsed?.Message}");

            var qrString = parsed.Data;
            return qrString; // ✅ QUAN TRỌNG: return QR content
        }
        catch (Exception ex)
        {
            Log.Error(ex, "[{TraceId}] ❌ Lỗi trong quá trình gọi PG để tạo QR", traceId);
            throw;
        }
    }


}

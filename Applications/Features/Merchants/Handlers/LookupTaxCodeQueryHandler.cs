using Applications.Features.Merchants.DTOs;
using Applications.Features.Merchants.Queries;
using Applications.Interfaces.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Serilog;
using Shared.Interfaces;
using Shared.Options;
using Shared.Results;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Applications.Features.Merchants.Handlers
{
    public class LookupTaxCodeQueryHandler : IRequestHandler<LookupTaxCodeQuery, BaseResponse<TaxCodeLookupResponse>>
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<LookupTaxCodeQueryHandler> _logger;
        private readonly VietQrOptions _vietQrOptions;
      //  private readonly ISpamProtectionService _spamProtection;
        private readonly IUnitOfWork _unitOfWork;
        public LookupTaxCodeQueryHandler(IHttpClientFactory httpClientFactory,
            ILogger<LookupTaxCodeQueryHandler> logger,
            IOptions<VietQrOptions> vietQrOptions,
           // ISpamProtectionService spamProtection,
            IUnitOfWork unitOfWork)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
            _vietQrOptions = vietQrOptions.Value;
            //_spamProtection = spamProtection;
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Xử lý tra cứu thông tin mã số thuế thông qua API VietQR, bao gồm kiểm tra trùng nội bộ và chống spam.
        /// </summary>
        public async Task<BaseResponse<TaxCodeLookupResponse>> Handle(LookupTaxCodeQuery request, CancellationToken cancellationToken)
        {
            var traceId = Activity.Current?.TraceId.ToString();
            var taxCode = request.TaxCode;
            var spamKey = $"spam:taxcode:{taxCode}";

            try
            {
                //// 🛡️ Kiểm tra spam: ngăn người dùng tra cứu 1 mã số thuế nhiều lần trong 1 phút
                //if (await _spamProtection.IsSpamAsync(spamKey, TimeSpan.FromMinutes(1), cancellationToken))
                //{
                //    _logger.LogWarning("[TraceId: {TraceId}] ❌ Spam phát hiện: taxCode={TaxCode}", traceId, taxCode);
                //    return BaseResponse<TaxCodeLookupResponse>.Error(
                //        "Mỗi mã số thuế chỉ được tra cứu tối đa 1 lần/phút (theo IP + Token).",
                //        traceId
                //    );
                //}

                //// 🔐 Đánh dấu key spam để giới hạn request tiếp theo
                //await _spamProtection.MarkAsync(spamKey, TimeSpan.FromMinutes(1), cancellationToken);

                // 🔍 Kiểm tra nội bộ xem mã số thuế đã tồn tại trong hệ thống chưa
                if (await _unitOfWork.MerchantRepositories.ExistsByBusinessNoAsync(taxCode))
                {
                    _logger.LogWarning("[TraceId: {TraceId}] ⚠️ Mã số thuế đã tồn tại trong hệ thống: {TaxCode}", traceId, taxCode);
                    return BaseResponse<TaxCodeLookupResponse>.Error(
                        $"Mã số thuế {taxCode} đã được đăng ký trên hệ thống.",ErrorCodes.MerchantBranch_DuplicateBranchTaxNo,
                        traceId
                    );
                }

                // 🌐 Tạo URL tra cứu VietQR từ cấu hình
                var endpoint = _vietQrOptions.TaxCodeLookupEndpoint.Replace("{taxCode}", taxCode);
                var url = $"{_vietQrOptions.BaseUrl.TrimEnd('/')}/{endpoint.TrimStart('/')}";

                _logger.LogInformation("[TraceId: {TraceId}] ▶️ Gửi request tra cứu mã số thuế: {TaxCode} -> {Url}", traceId, taxCode, url);

                // 📡 Gửi request đến VietQR API
                var client = _httpClientFactory.CreateClient();
                var response = await client.GetAsync(url, cancellationToken);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning("[TraceId: {TraceId}] ❌ VietQR trả về lỗi HTTP. TaxCode: {TaxCode}, StatusCode: {StatusCode}", traceId, taxCode, response.StatusCode);
                    return BaseResponse<TaxCodeLookupResponse>.Error("Tra cứu mã số thuế thất bại từ VietQR.", traceId);
                }

                // 📦 Đọc nội dung trả về
                var content = await response.Content.ReadAsStringAsync(cancellationToken);

                // 🧩 Parse JSON kết quả
                var parsed = JsonSerializer.Deserialize<VietQrResponse>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (parsed?.Code != "00" || parsed.Data == null)
                {
                    _logger.LogWarning("[TraceId: {TraceId}] ❌ VietQR trả về không hợp lệ. TaxCode: {TaxCode}, Code: {Code}, Desc: {Desc}",
                        traceId, taxCode, parsed?.Code, parsed?.Desc);
                    return BaseResponse<TaxCodeLookupResponse>.Error("Không tìm thấy thông tin mã số thuế.", traceId);
                }

                // ✅ Thành công: Lấy thông tin doanh nghiệp từ phản hồi
                var result = new TaxCodeLookupResponse
                {
                    CompanyName = parsed.Data.Name,
                    CompanyAddress = parsed.Data.Address
                };

                _logger.LogInformation("[TraceId: {TraceId}] ✅ Tra cứu thành công. TaxCode: {TaxCode}, CompanyName: {CompanyName}, Address: {CompanyAddress}",
                    traceId, taxCode, result.CompanyName, result.CompanyAddress);

                return BaseResponse<TaxCodeLookupResponse>.Success(result, traceId);
            }
            catch (Exception ex)
            {
                // 🔥 Log lỗi hệ thống nếu có exception
                _logger.LogError(ex, "[TraceId: {TraceId}] ❗ Lỗi hệ thống khi xử lý tra cứu mã số thuế: {Message}", traceId, ex.Message);
                return BaseResponse<TaxCodeLookupResponse>.Error("Lỗi hệ thống không xác định. Vui lòng thử lại sau.", traceId);
            }
        }

    }
}

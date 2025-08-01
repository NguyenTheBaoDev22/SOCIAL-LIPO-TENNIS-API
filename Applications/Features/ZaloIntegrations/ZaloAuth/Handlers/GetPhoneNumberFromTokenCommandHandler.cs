using Applications.Features.ZaloIntegrations.ZaloAuth.Commands;
using Applications.Features.ZaloIntegrations.ZaloAuth.DTOs;
using Applications.Interfaces.Repositories;
using Applications.Services.Interfaces;
using Core.Entities.Logs;
using MediatR;
using Microsoft.Extensions.Options;
using Serilog;
using Shared.Configs;
using Shared.Interfaces;
using Shared.Results;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Applications.Features.ZaloIntegrations.ZaloAuth.Handlers
{
    /// <summary>
    /// Handler xử lý lấy số điện thoại người dùng từ Zalo thông qua Access Token và Phone Token Code.
    /// Gọi tới service tích hợp Zalo, log lịch sử gọi, và trả về thông tin người dùng nếu thành công.
    /// </summary>
    public class GetPhoneNumberFromTokenCommandHandler
        : IRequestHandler<GetPhoneNumberFromTokenCommand, BaseResponse<UserZaloIdentityResponseModel>>
    {
        private readonly IZaloAuthenticationService _zaloAuthService;
        private readonly IBaseRepository<ZaloAuthLog> _zaloAuthLogRepository;
        private readonly ICurrentUserService _currentUser;
        private readonly ZaloConfig _zaloConfig;

        public GetPhoneNumberFromTokenCommandHandler(
            IZaloAuthenticationService zaloAuthService,
            IBaseRepository<ZaloAuthLog> zaloAuthLogRepository,
            ICurrentUserService currentUser,
            IOptions<ZaloConfig> zaloConfig)
        {
            _zaloAuthService = zaloAuthService;
            _zaloAuthLogRepository = zaloAuthLogRepository;
            _currentUser = currentUser;
            _zaloConfig = zaloConfig.Value;
        }

        public async Task<BaseResponse<UserZaloIdentityResponseModel>> Handle(GetPhoneNumberFromTokenCommand request, CancellationToken cancellationToken)
        {
            var traceId = string.IsNullOrWhiteSpace(_currentUser.TraceId) ? Guid.NewGuid().ToString("N") : _currentUser.TraceId;

            Log.Information("🔐 [ZaloAuth] Bắt đầu xử lý lấy SĐT từ Zalo {@TraceId} | AccessToken: {AccessToken} | @PhoneToken: {PhoneTokenCode}",
                traceId, request.AccessToken, request.PhoneTokenCode);

            // Gọi service xử lý chính
            var result = await _zaloAuthService.GetUserInfoFromZaloAsync(request.AccessToken, request.PhoneTokenCode);

            // Log kết quả gọi API
            if (result.IsSuccess)
            {
                Log.Information("✅ [ZaloAuth] Lấy SĐT thành công từ Zalo {@TraceId} | @Result: {@Result}",
                    traceId, result.Data);
            }
            else
            {
                Log.Warning("❌ [ZaloAuth] Lỗi khi gọi Zalo API {@TraceId} | Message: {Message} | @Result: {@Result}",
                    traceId, result.Message, result);
            }

            // Ghi log vào bảng ZaloAuthLog để audit
            var requestUrl = $"{_zaloConfig.BaseUrl}{_zaloConfig.GetUserInfoEndpoint}";
            var log = new ZaloAuthLog
            {
                TraceId = traceId,
                RequestUrl = requestUrl,
                Token = request.PhoneTokenCode,
                PhoneNumber = result.IsSuccess ? result.Data?.UserPhoneNumber : null,
                Success = result.IsSuccess,
                ErrorMessage = result.IsSuccess ? null : result.Message,
                CalledAt = DateTime.UtcNow,
                CreatedBy = _currentUser.UserId
            };

            await _zaloAuthLogRepository.AddAsync(log, cancellationToken);
            await _zaloAuthLogRepository.SaveChangesAsync(cancellationToken);

            Log.Information("📄 [ZaloAuth] Đã ghi log vào bảng ZaloAuthLog {@TraceId}", traceId);

            return result;
        }
    }
}

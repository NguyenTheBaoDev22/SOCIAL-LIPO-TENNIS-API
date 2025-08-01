using Applications.Features.ZaloIntegrations.ZaloAuth.DTOs;
using Applications.Services.Interfaces;
using Serilog;
using Shared.Interfaces;
using Shared.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.Services.Implementations
{
    public class ZaloAuthenticationService : IZaloAuthenticationService
    {
        private readonly IZaloApiClient _zaloApiClient;
        private readonly ICurrentUserService _currentUser;

        public ZaloAuthenticationService(
            IZaloApiClient zaloApiClient,
            ICurrentUserService currentUser)
        {
            _zaloApiClient = zaloApiClient;
            _currentUser = currentUser;
        }

        public async Task<BaseResponse<UserZaloIdentityResponseModel>> GetUserInfoFromZaloAsync(string accessToken, string zaloToken)
        {
            var traceId = _currentUser.TraceId;

            // 1. Sửa tên phương thức cho đúng
            var zaloResponse = await _zaloApiClient.GetPhoneNumberAsync(accessToken, zaloToken, traceId);

            // 2. Sửa lại cách kiểm tra và truy cập SĐT
            if (!zaloResponse.IsSuccess || string.IsNullOrWhiteSpace(zaloResponse.Data?.ZaloUserPhoneNumber))
            {
                return BaseResponse<UserZaloIdentityResponseModel>.Error("ZALO_INVALID_DATA", "Zalo did not return a valid phone number.", traceId);
            }

            // 3. Sửa lại cách lấy SĐT và chuẩn hóa
            var phoneNumberFromZalo = zaloResponse.Data.ZaloUserPhoneNumber;
            var normalizedPhone = NormalizePhoneNumber(phoneNumberFromZalo);

            // Lấy thông tin khách hàng từ DB
            //var customer = await _customerService.GetByPhoneAsync(normalizedPhone);

            //if (customer == null)
            //{
            //    Log.Warning("TraceId: {TraceId} - Customer with phone {Phone} not found.", traceId, normalizedPhone);
            //    return BaseResponse<UserZaloIdentityResponseModel>.Error("CUSTOMER_NOT_FOUND", "Customer not found.", traceId);
            //}

            // 4. Tạo đối tượng response mới thay vì sửa đổi đối tượng cũ
            var result = new UserZaloIdentityResponseModel
            {
                UserPhoneNumber = normalizedPhone

            };

            return BaseResponse<UserZaloIdentityResponseModel>.Success(result, traceId);
        }

        private string NormalizePhoneNumber(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return string.Empty;

            if (input.StartsWith("+84"))
                return "0" + input.Substring(3);
            if (input.StartsWith("84"))
                return "0" + input.Substring(2);
            return input;
        }
    }

}

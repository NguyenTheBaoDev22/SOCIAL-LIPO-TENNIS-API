using Applications.Features.ZaloIntegrations.ZaloAuth.DTOs;
using Shared.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.Services.Interfaces
{
    public interface IZaloApiClient
    {
        Task<BaseResponse<ZaloPhoneNumberData>> GetPhoneNumberAsync(
            string accessToken, string phoneTokenCode, string? inputTraceId = null, CancellationToken cancellationToken = default);
        Task<BaseResponse<LocationResultDto>> GetLocationAsync(
    string accessToken, string locationToken, string? traceId = null, CancellationToken cancellationToken = default);
    }

}

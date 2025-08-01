using Applications.Features.ZaloIntegrations.ZaloAuth.DTOs;
using Applications.Features.ZaloIntegrations.ZaloAuth.Queries;
using Applications.Interfaces.Repositories;
using Applications.Services.Interfaces;
using Core.Entities.Logs;
using MediatR;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Serilog;
using Shared.Configs;
using Shared.Results;
using Shared.Interfaces;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Applications.Features.ZaloIntegrations.ZaloAuth.Handlers
{
    /// <summary>
    /// Handler xử lý lấy vị trí người dùng từ Zalo thông qua AccessToken và LocationToken.
    /// Gọi API Zalo, log lịch sử, và trả về dữ liệu nếu thành công.
    /// </summary>
    public class GetLocationFromZaloCommandHandler : IRequestHandler<GetZaloLocationQuery, BaseResponse<LocationResultDto>>
    {
        private readonly IZaloApiClient _zaloClient;
        private readonly IBaseRepository<ZaloLocationLog> _locationLogRepo;
        private readonly ICurrentUserService _currentUser;

        public GetLocationFromZaloCommandHandler(
            IZaloApiClient zaloClient,
            IBaseRepository<ZaloLocationLog> locationLogRepo,
            ICurrentUserService currentUser)
        {
            _zaloClient = zaloClient;
            _locationLogRepo = locationLogRepo;
            _currentUser = currentUser;
        }

        public async Task<BaseResponse<LocationResultDto>> Handle(GetZaloLocationQuery request, CancellationToken cancellationToken)
        {
            var traceId = _currentUser.TraceId ?? Guid.NewGuid().ToString("N");

            var result = await _zaloClient.GetLocationAsync(request.AccessToken, request.Code, traceId, cancellationToken);

            await _locationLogRepo.AddAsync(new ZaloLocationLog
            {
                TraceId = traceId,
                RequestUrl = "https://graph.zalo.me/v2.0/me/info",
                Token = request.Code,
                Coordinates = result.Data != null ? $"{result.Data.Latitude},{result.Data.Longitude}" : null,
                Success = result.IsSuccess,
                ErrorMessage = result.IsSuccess ? null : result.Message,
                CalledAt = DateTime.UtcNow,
                CreatedBy = _currentUser.UserId
            }, cancellationToken);

            await _locationLogRepo.SaveChangesAsync(cancellationToken);

            return result;
        }
    }

}

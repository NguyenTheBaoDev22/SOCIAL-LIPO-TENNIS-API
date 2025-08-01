using Microsoft.AspNetCore.Http;
using Shared.Interfaces;
using System.Security.Claims;

namespace Shared.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private string? _traceId;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        private ClaimsPrincipal? User => _httpContextAccessor.HttpContext?.User;

        public Guid? UserId =>
            Guid.TryParse(User?.FindFirst("sub")?.Value, out var id) ? id : null;

        public string? PhoneNumber => User?.FindFirst("phone_number")?.Value;
        public string? Email => User?.FindFirst("email")?.Value;
        public Guid? TenantId => Guid.TryParse(User?.FindFirst("tenant_id")?.Value, out var id) ? id : null;
        public Guid? MerchantId => Guid.TryParse(User?.FindFirst("merchant_id")?.Value, out var id) ? id : null;
        public Guid? MerchantBranchId => Guid.TryParse(User?.FindFirst("branch_id")?.Value, out var id) ? id : null;

        public List<string> Roles => User?.FindAll("role").Select(c => c.Value).ToList() ?? new List<string>();
        public List<string> Permissions => User?.FindAll("permission").Select(c => c.Value).ToList() ?? new List<string>();

        public bool IsAuthenticated => User?.Identity?.IsAuthenticated ?? false;

        public string? TraceId => _traceId;
        public void SetTraceId(string traceId) => _traceId = traceId;

        // ✅ Bổ sung 2 thuộc tính này
        public string? IpAddress => _httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString();
        public string? UserAgent => _httpContextAccessor.HttpContext?.Request?.Headers["User-Agent"].FirstOrDefault();
    }


}

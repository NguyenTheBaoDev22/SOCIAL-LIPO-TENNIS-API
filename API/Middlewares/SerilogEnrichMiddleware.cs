using Microsoft.AspNetCore.Http;
using Serilog.Context;
using System.Threading.Tasks;

namespace API.Middlewares
{
    public class SerilogEnrichMiddleware
    {
        private readonly RequestDelegate _next;

        public SerilogEnrichMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context == null)
            {
                await _next(context); // fallback
                return;
            }

            var traceId = context.TraceIdentifier;
            var user = context.User;

            var userId = user?.Identity?.IsAuthenticated == true
                ? user.FindFirst("sub")?.Value
                : null;

            var tenantId = user?.FindFirst("tenantId")?.Value;
            var merchantId = user?.FindFirst("merchantId")?.Value;
            var branchId = user?.FindFirst("merchantBranchId")?.Value;

            var ipAddress = context.Connection?.RemoteIpAddress?.ToString() ?? "unknown";
            var userAgent = context.Request?.Headers["User-Agent"].ToString() ?? "unknown";

            var logProperties = new List<IDisposable>
            {
                LogContext.PushProperty("TraceId", traceId),
                LogContext.PushProperty("IpAddress", ipAddress),
                LogContext.PushProperty("UserAgent", userAgent)
            };

            if (!string.IsNullOrEmpty(userId))
                logProperties.Add(LogContext.PushProperty("UserId", userId));

            if (!string.IsNullOrEmpty(tenantId))
                logProperties.Add(LogContext.PushProperty("TenantId", tenantId));

            if (!string.IsNullOrEmpty(merchantId))
                logProperties.Add(LogContext.PushProperty("MerchantId", merchantId));

            if (!string.IsNullOrEmpty(branchId))
                logProperties.Add(LogContext.PushProperty("MerchantBranchId", branchId));

            using (new DisposableEnricher(logProperties))
            {
                await _next(context);
            }
        }

        private class DisposableEnricher : IDisposable
        {
            private readonly List<IDisposable> _disposables;

            public DisposableEnricher(List<IDisposable> disposables)
            {
                _disposables = disposables;
            }

            public void Dispose()
            {
                foreach (var d in _disposables)
                {
                    d.Dispose();
                }
            }
        }
    }
}

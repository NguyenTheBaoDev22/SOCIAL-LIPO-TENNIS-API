using Serilog.Context;
using Shared.Interfaces;

namespace API.Middlewares
{
    public class RequestEnrichmentMiddleware
    {
        private readonly RequestDelegate _next;

        public RequestEnrichmentMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, ICurrentUserService currentUserService)
        {
            var traceId = context.TraceIdentifier;
            var ipAddress = context.Connection.RemoteIpAddress?.ToString() ?? "unknown";
            var userAgent = context.Request.Headers["User-Agent"].ToString();

            var userId = currentUserService.UserId?.ToString() ?? "anonymous";
            var tenantId = currentUserService.TenantId?.ToString() ?? "unknown";

            using (LogContext.PushProperty("TraceId", traceId))
            using (LogContext.PushProperty("IpAddress", ipAddress))
            using (LogContext.PushProperty("UserAgent", userAgent))
            using (LogContext.PushProperty("UserId", userId))
            using (LogContext.PushProperty("TenantId", tenantId))
            {
                await _next(context);
            }
        }
    }
}

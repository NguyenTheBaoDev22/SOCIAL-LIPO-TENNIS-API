using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Shared.Interfaces;
using Shared.Results;
using System;
using System.Threading.Tasks;

namespace Zenshop.WebAPI.Filters
{
    /// <summary>
    /// Chống spam bằng cách khóa IP + token + endpoint theo TTL
    /// </summary>
    public class PreventSpamAttribute : ActionFilterAttribute
    {
        private readonly string _keyPrefix;
        private readonly TimeSpan _window;

        public PreventSpamAttribute(string keyPrefix, int seconds)
        {
            _keyPrefix = keyPrefix;
            _window = TimeSpan.FromSeconds(seconds);
        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var httpContext = context.HttpContext;
            var request = httpContext.Request;

            var spamService = httpContext.RequestServices.GetRequiredService<ISpamProtectionService>();
            var logger = httpContext.RequestServices.GetRequiredService<ILogger<PreventSpamAttribute>>();

            // Lấy thông tin client
            var ip = httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown_ip";

            // Ưu tiên lấy từ header nếu có proxy
            if (request.Headers.TryGetValue("X-Forwarded-For", out var forwardedFor))
            {
                ip = forwardedFor.ToString().Split(',')[0]?.Trim(); // lấy IP đầu tiên
            }

            var userId = httpContext.User?.FindFirst("sub")?.Value ?? "anonymous";
            var endpoint = $"{request.Method}:{request.Path}";
            var userAgent = request.Headers["User-Agent"].ToString() ?? "unknown_user_agent";
            var traceId = httpContext.TraceIdentifier;

            if (await spamService.IsSpamAsync(_keyPrefix, _window))
            {
                logger.LogWarning("🚨 [AntiSpam] Blocked request | IP: {IP} | UserId: {UserId} | Endpoint: {Endpoint} | Agent: {UserAgent} | TraceId: {TraceId} | Time: {Time}",
                    ip, userId, endpoint, userAgent, traceId, DateTime.UtcNow.ToString("o"));

                context.Result = new JsonResult(BaseResponse<bool>.Error("Too many requests. Please wait."))
                {
                    StatusCode = StatusCodes.Status429TooManyRequests
                };
                return;
            }

            await spamService.MarkAsync(_keyPrefix, _window);
            await next();
        }
    }
}
